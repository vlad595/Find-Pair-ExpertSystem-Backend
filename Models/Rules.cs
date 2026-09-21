using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ProductionRule
    {
        [Key]
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        [Required]
        public string ConditionFact { get; set; } = string.Empty; 
        [Required]
        public string ConditionValue { get; set; } = string.Empty;
        [Required]
        public string ResultFact { get; set; } = string.Empty;
        [Required]
        public string ResultValue { get; set; } = string.Empty;
    }
}