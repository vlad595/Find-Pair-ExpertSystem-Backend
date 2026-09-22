using System;
using Data;
using DTO;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public interface ICandidateService
    {
        public Task<CandidateDto> CreateCandidate(CreateCandidateDto candidate);
        public Task<List<CandidateDto>> GetAllCandidates();
    }
    public class CandidateService: ICandidateService
    {
        private readonly KPZContext _context;
        public CandidateService(KPZContext context)
        {
            _context = context;
        }
        public async Task<CandidateDto> CreateCandidate(CreateCandidateDto candidate)
        {
            var newCandidate = new CandidateProfile
            {
                Id = Guid.NewGuid(),
                FullName = candidate.FullName,
                Gender = candidate.Gender,
                Age = candidate.Age,
                Facts = candidate.Facts
            };
            
            _context.Candidates.Add(newCandidate);
            await _context.SaveChangesAsync();

            return new CandidateDto(newCandidate);
        }
        public async Task<List<CandidateDto>> GetAllCandidates()
        {
            var candidates = await _context.Candidates.ToListAsync();
            if (candidates == null)
            {
                throw new Exception("404");
            }
            var candidatesResult = candidates.Select(c => new CandidateDto(c)).ToList();
            return candidatesResult;
        }
    }
}