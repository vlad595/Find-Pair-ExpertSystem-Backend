using System.Collections.Generic;
using System.Linq;
using Models;

namespace Services
{
    public class InferenceEngineService
    {
        public Dictionary<string, string> RunInference(Dictionary<string, string> currentFacts, List<ProductionRule> rules)
        {
            currentFacts = new Dictionary<string, string>(currentFacts, System.StringComparer.OrdinalIgnoreCase);
            bool factsChanged;
            do
            {
                factsChanged = false;
                
                foreach (var rule in rules)
                {
                    if (currentFacts.TryGetValue(rule.ConditionFact, out var value) && value == rule.ConditionValue && (!currentFacts.ContainsKey(rule.ResultFact) || currentFacts[rule.ResultFact] != rule.ResultValue))
                    {
                        currentFacts[rule.ResultFact] = rule.ResultValue;
                        factsChanged = true;
                    }
                }
            } 
            while (factsChanged); 
            return currentFacts;
        }

        public List<ProductionRule> GetFiredRules(Dictionary<string, string> originalFacts, List<ProductionRule> rules)
        {
            var facts = new Dictionary<string, string>(originalFacts, System.StringComparer.OrdinalIgnoreCase);
            var fired = new List<ProductionRule>();
            bool changed;
            do
            {
                changed = false;
                foreach (var rule in rules)
                {
                    if (facts.TryGetValue(rule.ConditionFact, out var value) &&
                        string.Equals(value, rule.ConditionValue, System.StringComparison.OrdinalIgnoreCase) &&
                        (!facts.TryGetValue(rule.ResultFact, out var existing) || !string.Equals(existing, rule.ResultValue, System.StringComparison.OrdinalIgnoreCase)))
                    {
                        facts[rule.ResultFact] = rule.ResultValue;
                        if (!fired.Contains(rule)) fired.Add(rule);
                        changed = true;
                    }
                }
            } while (changed);
            return fired;
        }
    }
}
