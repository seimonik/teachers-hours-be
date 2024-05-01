using MediatR;
using teachers_hours_be.Application.Models;
using TH.Dal;
using TH.Dal.Entities;

namespace teachers_hours_be.Application.Commands;

public static class UpdateTeacher
{
	public record Command(Guid TeacherId,
						  AddOrUpdateTeacherModel Teacher) : IRequest<Teacher>;

	internal class Handler : IRequestHandler<Command, Teacher>
	{
		private readonly TeachersHoursDbContext _dbContext;

		public Handler(TeachersHoursDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Teacher> Handle(Command request, CancellationToken cancellationToken)
		{
			var teacherDb = _dbContext.Teachers
				.Where(t => t.Id == request.TeacherId)
				.SingleOrDefault();

			if (teacherDb != null)
			{
				teacherDb.Surname = request.Teacher.Surname;
				teacherDb.Name = request.Teacher.Name;
				teacherDb.Patronymic = request.Teacher.Patronymic;
				teacherDb.Rate = request.Teacher.Rate;
				teacherDb.FullName = request.Teacher.Surname + " " + request.Teacher.Name + " " + request.Teacher.Patronymic;
			}

			await _dbContext.SaveChangesAsync();
			return teacherDb;
		}
	}
}
