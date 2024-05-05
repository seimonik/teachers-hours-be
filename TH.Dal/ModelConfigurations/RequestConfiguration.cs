using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TH.Dal.Entities;

namespace TH.Dal.ModelConfigurations;

public class RequestConfiguration : IEntityTypeConfiguration<Request>
{
	public void Configure(EntityTypeBuilder<Request> builder)
	{
		builder.HasKey(e => e.Id);
	}
}
