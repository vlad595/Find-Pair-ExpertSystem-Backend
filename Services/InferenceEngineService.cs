using System.Collections.Generic;
using Models;

namespace Services
{
    public class InferenceEngineService
    {
        public Dictionary<string, string> RunInference(Dictionary<string, string> currentFacts, List<ProductionRule> rules)
        {
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
    }
}