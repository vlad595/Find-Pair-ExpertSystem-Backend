using System;
using Models;

namespace DTO
{
    public class MatchResultDto
    {
        public CandidateProfile? BestMatch { get; set; }
        public int CompatibilityScore { get; set; }
        
        public Dictionary<string, string> InferredClientFacts { get; set; } = new();
    }
}