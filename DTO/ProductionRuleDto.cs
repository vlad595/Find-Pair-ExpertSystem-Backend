using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class CreateProductionRuleDto
    {
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string ConditionFact { get; set; } = string.Empty;
        [Required]
        [StringLength(500)]
        public string ConditionValue { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string ResultFact { get; set; } = string.Empty;
        [Required]
        [StringLength(500)]
        public string ResultValue { get; set; } = string.Empty;
    }
    public class UpdateProductionRuleDto : CreateProductionRuleDto
    {
    }
    public class ProductionRuleDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ConditionFact { get; set; } = string.Empty;
        public string ConditionValue { get; set; } = string.Empty;
        public string ResultFact { get; set; } = string.Empty;
        public string ResultValue { get; set; } = string.Empty;
    }
}