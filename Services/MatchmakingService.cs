using System;
using Data;
using System.Linq;
using System.Threading.Tasks;
using Models;
using DTO;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public interface IMatchmakingService
    {
        public Task<MatchResultDto> FindBestMatchAsync(Dictionary<string, string> clientFacts, string targetGender, int minAge, int maxAge);
    }
    public class MatchmakingService: IMatchmakingService
    {
        private readonly KPZContext _context;
        private readonly InferenceEngineService _inferenceEngine;
        public MatchmakingService(KPZContext context, InferenceEngineService inferenceEngine)
        {
            _context = context;
            _inferenceEngine = inferenceEngine;
        }
        public async Task<MatchResultDto> FindBestMatchAsync(Dictionary<string, string> clientFacts, string targetGender, int minAge, int maxAge)
        {
            if (minAge > maxAge) throw new ArgumentException("Мінімальний вік не може бути більшим за максимальний.");
            var rules = await _context.Rules.ToListAsync();

            var enrichedClientFacts = _inferenceEngine.RunInference(clientFacts, rules);

            var candidates = await _context.Candidates.Where(c => c.Gender == targetGender && c.Age >= minAge && c.Age <= maxAge).ToListAsync();

            CandidateProfile? bestMatch = null;
            int maxScore = -1;
            double bestPercent = -1;
            int comparedCriteria = 0;
            int matchedWeight = 0;
            int applicableWeight = 0;
            int missingCriteria = 0;
            var bestReasons = new List<string>();
            var bestConcerns = new List<string>();
            List<ProductionRule> bestFiredRules = new();
            Dictionary<string, string> bestCandidateFacts = new();

            foreach (var candidate in candidates)
            {
                var candidateFacts = _inferenceEngine.RunInference(candidate.Facts, rules);
                var firedRules = _inferenceEngine.GetFiredRules(candidate.Facts, rules);
                var applicableKeys = enrichedClientFacts.Keys.Union(candidateFacts.Keys, StringComparer.OrdinalIgnoreCase).ToList();
                var reasons = new List<string>();
                var concerns = new List<string>();
                int currentCompared = 0;
                int currentScore = 0;
                int currentWeight = 0;
                int currentApplicableWeight = 0;
                foreach (var key in applicableKeys)
                {
                    bool hasClient = enrichedClientFacts.TryGetValue(key, out var clientValue);
                    bool hasCandidate = candidateFacts.TryGetValue(key, out var candidateValue);
                    if (!hasClient || !hasCandidate)
                    {
                        if (hasClient) concerns.Add($"У кандидата немає даних про «{key}».");
                        continue;
                    }
                    int weight = GetCriterionWeight(key);
                    currentCompared++;
                    currentApplicableWeight += weight;
                    if (string.Equals(clientValue, candidateValue, StringComparison.OrdinalIgnoreCase))
                    {
                        currentScore += weight;
                        currentWeight += weight;
                        reasons.Add($"Збіг за критерієм «{key}» (вага {weight}): {clientValue}.");
                    }
                    else concerns.Add($"Різні значення за критерієм «{key}» (вага {weight}): {clientValue} / {candidateValue}.");
                }

                var currentPercent = currentApplicableWeight == 0 ? 0 : 100.0 * currentWeight / currentApplicableWeight;
                if (currentPercent > bestPercent || (Math.Abs(currentPercent - bestPercent) < 0.0001 && currentCompared > comparedCriteria))
                {
                    bestPercent = currentPercent;
                    maxScore = currentScore;
                    bestMatch = candidate;
                    comparedCriteria = currentCompared;
                    matchedWeight = currentWeight;
                    applicableWeight = currentApplicableWeight;
                    missingCriteria = Math.Max(0, enrichedClientFacts.Count - currentCompared);
                    bestReasons = reasons;
                    bestConcerns = concerns;
                    bestFiredRules = firedRules;
                    bestCandidateFacts = candidateFacts;
                }
            }

            return new MatchResultDto 
            {
                BestMatch = bestMatch,
                CompatibilityScore = maxScore,
                MatchedWeight = matchedWeight,
                ApplicableWeight = applicableWeight,
                CompatibilityPercent = applicableWeight == 0 ? 0 : (int)Math.Round(100.0 * matchedWeight / applicableWeight),
                Assessment = applicableWeight == 0 ? "Недостатньо спільних даних для оцінки." : matchedWeight * 100.0 / applicableWeight >= 75 ? "Висока сумісність за відомими критеріями." : matchedWeight * 100.0 / applicableWeight >= 50 ? "Помірна сумісність за відомими критеріями." : "Низька сумісність за відомими критеріями.",
                ComparedCriteria = comparedCriteria,
                MissingCriteria = missingCriteria,
                Reasons = bestReasons,
                Concerns = bestConcerns,
                FiredRules = bestFiredRules.Select(FormatRuleExplanation).ToList(),
                BestMatchFacts = bestCandidateFacts,
                InferredClientFacts = enrichedClientFacts
            };
        }

        private static int GetCriterionWeight(string fact)
        {
            var key = fact.ToLowerInvariant();
            if (new[] { "value", "цін", "relationship", "стосунк", "expect", "очікуван" }.Any(key.Contains)) return 3;
            if (new[] { "child", "дит", "lifestyle", "style", "житт", "спосіб" }.Any(key.Contains)) return 2;
            return 1;
        }

        private static string FormatRuleExplanation(ProductionRule rule)
        {
            var ruleText = $"Якщо «{rule.ConditionFact}» = «{rule.ConditionValue}», то «{rule.ResultFact}» = «{rule.ResultValue}».";
            return string.IsNullOrWhiteSpace(rule.Description) || int.TryParse(rule.Description, out _)
                ? ruleText
                : $"{rule.Description}: {ruleText}";
        }
    }
}
