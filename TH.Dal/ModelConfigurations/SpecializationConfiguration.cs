using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TH.Dal.Entities;

namespace TH.Dal.ModelConfigurations;

internal sealed class SpecializationConfiguration : IEntityTypeConfiguration<Specialization>
{
	public void Configure(EntityTypeBuilder<Specialization> builder)
	{
		builder.HasKey(e => e.Code);

		builder.ToTable("Specializations", schema: "lookups");

		builder.HasData(new Specialization { Code = "02.03.02", Name = "ФИИТ", EducationLevel = Enums.EducationLevelTypes.Bachelor },
			new Specialization { Code = "02.03.03", Name = "МОАИС", EducationLevel = Enums.EducationLevelTypes.Bachelor },
			new Specialization { Code = "09.03.01", Name = "ИВТ", EducationLevel = Enums.EducationLevelTypes.Bachelor },
			new Specialization { Code = "09.03.03", Name = "ПИ", EducationLevel = Enums.EducationLevelTypes.Bachelor },
			new Specialization { Code = "27.03.03", Name = "САиУ", EducationLevel = Enums.EducationLevelTypes.Bachelor },
			new Specialization { Code = "44.03.01", Name = "ПО", EducationLevel = Enums.EducationLevelTypes.Bachelor },

			new Specialization { Code = "10.05.01", Name = "КБ", EducationLevel = Enums.EducationLevelTypes.Specialty },

			new Specialization { Code = "02.04.03", Name = "МОАИС", EducationLevel = Enums.EducationLevelTypes.Magistracy },
			new Specialization { Code = "09.04.01", Name = "ИВТ", EducationLevel = Enums.EducationLevelTypes.Magistracy },
			new Specialization { Code = "44.04.01", Name = "ПО", EducationLevel = Enums.EducationLevelTypes.Magistracy },

			new Specialization { Code = "1.1.5", Name = "МЛ", EducationLevel = Enums.EducationLevelTypes.Postgraduate },
			new Specialization { Code = "1.2.2", Name = "ММ", EducationLevel = Enums.EducationLevelTypes.Postgraduate },
			new Specialization { Code = "1.2.3", Name = "ТИ", EducationLevel = Enums.EducationLevelTypes.Postgraduate },
			new Specialization { Code = "2.3.1", Name = "СА", EducationLevel = Enums.EducationLevelTypes.Postgraduate },
			new Specialization { Code = "02.06.01", Name = "КИН", EducationLevel = Enums.EducationLevelTypes.Postgraduate },
			new Specialization { Code = "09.06.01", Name = "ИВТ", EducationLevel = Enums.EducationLevelTypes.Postgraduate },
			new Specialization { Code = "01.06.01", Name = "ММ", EducationLevel = Enums.EducationLevelTypes.Postgraduate });
	}
}
