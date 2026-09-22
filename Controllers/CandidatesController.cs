using System;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidatesController : ControllerBase
    {
        private readonly CandidateService _service;
        public CandidatesController(CandidateService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<ActionResult<CandidateDto>> CreateCandidate([FromBody] CreateCandidateDto candidate)
        {
            if (candidate == null) return BadRequest();

            await _service.CreateCandidate(candidate);

            return Ok(candidate);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandidateDto>>> GetCandidates()
        {
            return await _service.GetAllCandidates();
        }
    }
}