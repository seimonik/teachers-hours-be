using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TH.Dal.Entities;

namespace TH.Dal.ModelConfigurations;

internal sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
	public void Configure(EntityTypeBuilder<Document> builder)
	{
		builder.HasKey(e => e.Id);

		builder.Property(d => d.CreatedAt)
			.ValueGeneratedNever()
			.HasDefaultValueSql("now() at time zone 'utc'");
	}
}
