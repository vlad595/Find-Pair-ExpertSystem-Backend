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
        public Task<MatchResultDto> FindBestMatchAsync(Dictionary<string, string> clientFacts, string targetGender);
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
        public async Task<MatchResultDto> FindBestMatchAsync(Dictionary<string, string> clientFacts, string targetGender)
        {
            var rules = await _context.Rules.ToListAsync();

            var enrichedClientFacts = _inferenceEngine.RunInference(clientFacts, rules);

            var candidates = await _context.Candidates.Where(c => c.Gender == targetGender).ToListAsync();

            CandidateProfile? bestMatch = null;
            int maxScore = -1;

            foreach (var candidate in candidates)
            {
                var candidateFacts = _inferenceEngine.RunInference(candidate.Facts, rules);
                int currentScore = CalculateCompatibilityScore(enrichedClientFacts, candidateFacts);

                if (currentScore > maxScore)
                {
                    maxScore = currentScore;
                    bestMatch = candidate;
                }
            }

            return new MatchResultDto 
            {
                BestMatch = bestMatch,
                CompatibilityScore = maxScore,
                InferredClientFacts = enrichedClientFacts
            };
        }

        private int CalculateCompatibilityScore(Dictionary<string, string> clientFacts, Dictionary<string, string> candidateFacts)
        {
            int score = 0;
            
            foreach (var kvp in clientFacts)
            {
                if (candidateFacts.TryGetValue(kvp.Key, out var candidateValue) && candidateValue == kvp.Value)
                {
                    score += 1; 
                }
            }
            return score;
        }
    }
}