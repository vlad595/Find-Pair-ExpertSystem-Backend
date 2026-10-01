using System;
using Models;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class MatchResultDto
    {
        public CandidateProfile? BestMatch { get; set; }
        public int CompatibilityScore { get; set; }
        public int CompatibilityPercent { get; set; }
        public string Assessment { get; set; } = string.Empty;
        public int ComparedCriteria { get; set; }
        public int MatchedWeight { get; set; }
        public int ApplicableWeight { get; set; }
        public int MissingCriteria { get; set; }
        public List<string> Reasons { get; set; } = new();
        public List<string> Concerns { get; set; } = new();
        public List<string> FiredRules { get; set; } = new();
        public Dictionary<string, string> BestMatchFacts { get; set; } = new();
        public Dictionary<string, string> InferredClientFacts { get; set; } = new();
    }
    public class MatchRequestDto
    {
        [Required]
        public string TargetGender { get; set; } = string.Empty;
        
        public Dictionary<string, string> Facts { get; set; } = new();

        [Range(18, 120)]
        public int MinAge { get; set; } = 18;

        [Range(18, 120)]
        public int MaxAge { get; set; } = 120;
    }
}
