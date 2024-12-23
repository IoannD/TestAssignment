using Microsoft.EntityFrameworkCore;
using TestAssignment.Domain.Entities;

namespace TestAssignment.DB;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    public DbSet<ClinicalTrial> ClinicalTrials { get; set; }
}