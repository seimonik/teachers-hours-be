using Microsoft.EntityFrameworkCore;
using TH.Dal.Entities;

namespace TH.Dal;

public class TeachersHoursDbContext : DbContext
{
	public DbSet<Document> Documents { get; protected set; } = null!;

	public TeachersHoursDbContext(DbContextOptions<TeachersHoursDbContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(TeachersHoursDbContext).Assembly);
	}
}
