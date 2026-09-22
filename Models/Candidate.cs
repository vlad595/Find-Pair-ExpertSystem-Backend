using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class CandidateProfile
    {
        public Guid Id { get; set; }
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required]
        public string Gender { get; set; } = string.Empty;
        public int Age { get; set; }
        [Column(TypeName = "jsonb")]
        public Dictionary<string, string> Facts { get; set; } = new();
    }
}