using teachers_hours_be.Application.Models;
using TH.Dal.Entities;

namespace teachers_hours_be.Extensions.ModelConversion;

public static class TeacherConversionExtensions
{
	public static Teacher ToTeacher(this AddOrUpdateTeacherModel teacherModel) =>
		new Teacher
		{
			Name = teacherModel.Name,
			Surname = teacherModel.Surname,
			Patronymic = teacherModel.Patronymic,
			Rate = teacherModel.Rate,
			FullName = teacherModel.Surname + " " + teacherModel.Name + " " + teacherModel.Patronymic,
		};

	public static GetTeacherModel ToGetTeacherModel(this Teacher teacher) =>
		new GetTeacherModel
		{
			Id = teacher.Id,
			FullName = teacher.FullName,
			Name = teacher.Name,
			Surname = teacher.Surname,
			Patronymic = teacher.Patronymic,
			Rate = teacher.Rate
		};
}
