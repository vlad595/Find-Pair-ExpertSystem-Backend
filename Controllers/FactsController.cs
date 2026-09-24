using System;
using Data;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FactsController: ControllerBase
    {
        private readonly KPZContext _context;
        public FactsController(KPZContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FactsDto>>> GetAllFacts()
        {
            List<FactsDto> factsResult = await _context.Rules.Select(r => r.ConditionFact).Distinct().Select(r => new FactsDto {Fact = r}).ToListAsync();

            foreach (FactsDto fact in factsResult)
            {
                fact.Values = await _context.Rules.Where(r => r.ConditionFact == fact.Fact).Select(r => r.ConditionValue).ToListAsync();
            }

            return Ok(factsResult);
        }
    }
}