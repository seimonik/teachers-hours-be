using MediatR;
using Microsoft.EntityFrameworkCore;
using TH.Dal;
using TH.Dal.Entities;

namespace teachers_hours_be.Application.Commands.Lookups;

public static class DeleteSpecialisation
{
	public record Command(string Code) : IRequest<Unit>;

	internal class Handler : IRequestHandler<Command, Unit>
	{
		private readonly TeachersHoursDbContext _dbContext;

		public Handler(TeachersHoursDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
		{
			var specialisationDb = await GetSpecialization(request.Code);

			if (specialisationDb != null)
			{
				_dbContext.Remove(specialisationDb);
				await _dbContext.SaveChangesAsync();
			}

			return Unit.Value;
		}

		private async Task<Specialization?> GetSpecialization(string code) =>
			await _dbContext.Specializations.SingleOrDefaultAsync(x => x.Code == code);
	}
}
