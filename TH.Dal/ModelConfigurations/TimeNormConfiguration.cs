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

		builder.HasData(new TimeNorm { Id = Guid.NewGuid(), Code = "ProductionPractice", Name = "Производственная практика", Norm = 4 },
			new TimeNorm { Id = Guid.NewGuid(), Code = "EducationalPractice23", Name = "Учебная практика (НИР) 2 курс 3 семестр", Norm = 18 },
			new TimeNorm { Id = Guid.NewGuid(), Code = "EducationalPractice35", Name = "Учебная практика (НИР) 3 курс 5 семестр", Norm = 18 },
			new TimeNorm { Id = Guid.NewGuid(), Code = "EducationalPractice24", Name = "Учебная практика (НИР) 2 курс 4 семестр", Norm = 16 },
			new TimeNorm { Id = Guid.NewGuid(), Code = "EducationalPractice36", Name = "Учебная практика (НИР) 3 курс 6 семестр", Norm = 16 },
			new TimeNorm { Id = Guid.NewGuid(), Code = "EducationalPractice47", Name = "Учебная практика (НИР) 4 курс 7 семестр", Norm = 14 },
			new TimeNorm { Id = Guid.NewGuid(), Code = "Coursework2", Name = "Норма часов по курсовым работам (2 курс)", Norm = 5 },
			new TimeNorm { Id = Guid.NewGuid(), Code = "Coursework3", Name = "Норма часов по курсовым работам (3 курс)", Norm = 10 },
			new TimeNorm { Id = Guid.NewGuid(), Code = "Coursework2PO", Name = "Норма часов по курсовым работам для ПО (2 курс)", Norm = 3 },
			new TimeNorm { Id = Guid.NewGuid(), Code = "Coursework3PO", Name = "Норма часов по курсовым работам для ПО (3 курс)", Norm = 10 },
			new TimeNorm { Id = Guid.NewGuid(), Code = "FinalQualifyingWorkBachelor", Name = "ВКР (бакалавриат)", Norm = 24.25 },
			new TimeNorm { Id = Guid.NewGuid(), Code = "FinalQualifyingWorkMagistracy", Name = "ВКР (магистратура)", Norm = 34.25 });
	}
}
