using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TH.Dal.Entities;

namespace TH.Dal.ModelConfigurations;

public class TimeNormConfiguration : IEntityTypeConfiguration<TimeNorm>
{
	public void Configure(EntityTypeBuilder<TimeNorm> builder)
	{
		builder.HasKey(e => e.Id);

		builder.ToTable("TimeNorms", schema: "lookups");

		builder.HasIndex(e => e.Code);

		builder.HasData(new TimeNorm { Id = Guid.NewGuid(), Code = "ProductionPractice", Name = "Производственная практика" },
			new TimeNorm { Id = Guid.NewGuid(), Code = "EducationalPractice", Name = "Учебная практика (НИР)" },
			new TimeNorm { Id = Guid.NewGuid(), Code = "Coursework2", Name = "Норма часов по курсовым работам (2 курс)" },
			new TimeNorm { Id = Guid.NewGuid(), Code = "Coursework3", Name = "Норма часов по курсовым работам (3 курс)" },
			new TimeNorm { Id = Guid.NewGuid(), Code = "Coursework2PO", Name = "Норма часов по курсовым работам для ПО (2 курс)" },
			new TimeNorm { Id = Guid.NewGuid(), Code = "Coursework3PO", Name = "Норма часов по курсовым работам для ПО (3 курс)" },
			new TimeNorm { Id = Guid.NewGuid(), Code = "FinalQualifyingWorkBachelor", Name = "ВКР (бакалавриат)" },
			new TimeNorm { Id = Guid.NewGuid(), Code = "FinalQualifyingWorkMagistracy", Name = "ВКР (магистратура)" });
	}
}
