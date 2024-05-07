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

		builder.HasData(new TimeNorm { Id = new Guid("85744b33-9494-4436-ad8d-743b92135b17"), Code = "ProductionPractice", Name = "Производственная практика", Norm = 4 },
			new TimeNorm { Id = new Guid("41371a87-ec4c-4038-bd03-d07c80ff88a7"), Code = "EducationalPractice23", Name = "Учебная практика (НИР) 2 курс 3 семестр", Norm = 18 },
			new TimeNorm { Id = new Guid("3ec94a49-266d-4cf2-bfee-fe35516cbd79"), Code = "EducationalPractice35", Name = "Учебная практика (НИР) 3 курс 5 семестр", Norm = 18 },
			new TimeNorm { Id = new Guid("25d1411a-e920-4d44-a036-849698387abf"), Code = "EducationalPractice24", Name = "Учебная практика (НИР) 2 курс 4 семестр", Norm = 16 },
			new TimeNorm { Id = new Guid("386cfcb3-643d-4b0f-9135-b5ea4f5182cd"), Code = "EducationalPractice36", Name = "Учебная практика (НИР) 3 курс 6 семестр", Norm = 16 },
			new TimeNorm { Id = new Guid("760ecbf0-03be-484b-b076-c37059dc478b"), Code = "EducationalPractice47", Name = "Учебная практика (НИР) 4 курс 7 семестр", Norm = 14 },
			new TimeNorm { Id = new Guid("837a8528-d235-4649-aa32-1e3b01f33928"), Code = "Coursework2", Name = "Норма часов по курсовым работам (2 курс)", Norm = 5 },
			new TimeNorm { Id = new Guid("051f35df-bc4e-452f-9d1a-4a708372fc2f"), Code = "Coursework3", Name = "Норма часов по курсовым работам (3 курс)", Norm = 10 },
			new TimeNorm { Id = new Guid("d456e79d-c74d-4783-b9a1-8aecdfb31750"), Code = "Coursework2PO", Name = "Норма часов по курсовым работам для ПО (2 курс)", Norm = 3 },
			new TimeNorm { Id = new Guid("a90b91fa-b823-4643-8186-9e2cb90d7156"), Code = "Coursework3PO", Name = "Норма часов по курсовым работам для ПО (3 курс)", Norm = 10 },
			new TimeNorm { Id = new Guid("89bae101-3f08-4e1e-b93f-d2bd578b9361"), Code = "FinalQualifyingWorkBachelor", Name = "ВКР (бакалавриат)", Norm = 24.25 },
			new TimeNorm { Id = new Guid("39d825fb-5e2e-4bc8-b0f6-44d95a122fab"), Code = "FinalQualifyingWorkMagistracy", Name = "ВКР (магистратура)", Norm = 34.25 });
	}
}
