using System;
using Models;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class MatchResultDto
    {
        public CandidateProfile? BestMatch { get; set; }
        public int CompatibilityScore { get; set; }
        
        public Dictionary<string, string> InferredClientFacts { get; set; } = new();
    }
    public class MatchRequestDto
    {
        [Required]
        public string TargetGender { get; set; } = string.Empty;
        
        public Dictionary<string, string> Facts { get; set; } = new();
    }
}