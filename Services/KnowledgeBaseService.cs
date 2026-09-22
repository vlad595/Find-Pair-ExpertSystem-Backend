using System;
using System.Data;
using Data;
using DTO;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public interface IKnowledgeBaseService
    {
        public Task<List<ProductionRuleDto>> GetAllRulesAsync();
        public Task<ProductionRuleDto> CreateRuleAsync(CreateProductionRuleDto rule);
        public Task<ProductionRuleDto> DeleteRuleAsync(Guid id);
    }
    public class KnowledgeBaseService: IKnowledgeBaseService
    {
        private readonly KPZContext _context;
        public KnowledgeBaseService(KPZContext context)
        {
            _context = context;
        }
        public async Task<List<ProductionRuleDto>> GetAllRulesAsync()
        {
            List<ProductionRule> rules = await _context.Rules.ToListAsync();
            if (rules == null)
            {
                throw new Exception("404");
            }
            List<ProductionRuleDto> rulesResult = rules.Select(r => new ProductionRuleDto(r)).ToList();
            return rulesResult;
        }
        public async Task<ProductionRuleDto> CreateRuleAsync(CreateProductionRuleDto rule)
        {
            var newRule = new ProductionRule
            {
                Id = Guid.NewGuid(),
                Description = rule.Description,
                ConditionFact = rule.ConditionFact,
                ConditionValue = rule.ConditionValue,
                ResultFact = rule.ResultFact,
                ResultValue = rule.ResultValue  
            };

            await _context.AddAsync(newRule);
            await _context.SaveChangesAsync();

            return new ProductionRuleDto(newRule);
        }
        public async Task<ProductionRuleDto> DeleteRuleAsync(Guid id)
        {
            var deleteRule = await _context.Rules.FirstOrDefaultAsync(r => r.Id == id);
            if (deleteRule == null)
            {
                throw new Exception("404");
            }
            _context.Rules.Remove(deleteRule);
            await _context.SaveChangesAsync();
            return new ProductionRuleDto(deleteRule);
        }
    }
}