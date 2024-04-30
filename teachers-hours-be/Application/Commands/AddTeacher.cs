using MediatR;
using teachers_hours_be.Application.Models;
using teachers_hours_be.Extensions.ModelConversion;
using TH.Dal;
using TH.Dal.Entities;

namespace teachers_hours_be.Application.Commands;

public static class AddTeacher
{
	public record Command(AddOrUpdateTeacherModel Teacher) : IRequest<Teacher>;

	internal class Handler : IRequestHandler<Command, Teacher>
	{
		private readonly TeachersHoursDbContext _dbContext;

		public Handler(TeachersHoursDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Teacher> Handle(Command request, CancellationToken cancellationToken)
		{
			var teacher = request.Teacher.ToTeacher();
			_dbContext.Teachers.Add(teacher);
			await _dbContext.SaveChangesAsync();

			return teacher;
		}
	}
}
