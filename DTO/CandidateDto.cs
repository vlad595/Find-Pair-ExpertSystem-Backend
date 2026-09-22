using System.ComponentModel.DataAnnotations;
using Models;

namespace DTO
{
    public class CreateCandidateDto
    {
        [Required]
        [StringLength(200)]
        public string FullName { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string Gender { get; set; } = string.Empty;
        [Range(18, 120)]
        public int Age { get; set; }
        public Dictionary<string, string> Facts { get; set; } = new();
    }
    public class UpdateCandidateDto : CreateCandidateDto
    {
    }
    public class CandidateDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int Age { get; set; }
        public Dictionary<string, string> Facts { get; set; } = new();
        public CandidateDto(CandidateProfile candidate)
        {
            this.Id = candidate.Id;
            this.FullName = candidate.FullName;
            this.Gender = candidate.Gender;
            this.Age = candidate.Age;
            this.Facts = candidate.Facts;
        }
    }
}