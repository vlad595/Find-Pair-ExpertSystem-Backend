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
    public class ConsultationController : ControllerBase
    {
        private readonly IMatchmakingService _matchmakingService;

        public ConsultationController(IMatchmakingService matchmakingService)
        {
            _matchmakingService = matchmakingService;
        }

        [HttpPost("match")]
        public async Task<ActionResult<MatchResultDto>> FindMatch([FromBody] MatchRequestDto request)
        {
            if (request == null || request.Facts == null || request.Facts.Count == 0)
            {
                return BadRequest();
            }

            var result = await _matchmakingService.FindBestMatchAsync(request.Facts, request.TargetGender);

            if (result.BestMatch == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}