using System;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class KPZContext : DbContext
    {
        public DbSet<ProductionRule> Rules { get; set; }
        public DbSet<CandidateProfile> Candidates { get; set; }
        public KPZContext(DbContextOptions<KPZContext> options) : base(options) {}
    }
}