using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services;
using DTO;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RulesController : ControllerBase
    {
        private readonly KnowledgeBaseService _service;

        public RulesController(KnowledgeBaseService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductionRule>>> GetRules()
        {
            try{
               return Ok(await _service.GetAllRulesAsync());
            }catch(Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductionRule>> CreateRule([FromBody] CreateProductionRuleDto rule)
        {
            if (rule == null) return BadRequest();

            await _service.CreateRuleAsync(rule);

            return Created();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRule(string id)
        {
            try
            {
                return Ok(await _service.DeleteRuleAsync(Guid.Parse(id)));
            }catch(Exception ex)
            {
                return NotFound();
            }
        }
    }
}