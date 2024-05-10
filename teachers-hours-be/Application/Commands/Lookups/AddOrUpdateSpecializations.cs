using MediatR;
using Microsoft.EntityFrameworkCore;
using teachers_hours_be.Application.Models.Lookups;
using teachers_hours_be.Extensions.ModelConversion;
using TH.Dal;
using TH.Dal.Entities;

namespace teachers_hours_be.Application.Commands.Lookups;

public static class AddOrUpdateSpecializations
{
	public record Command(SpecializationModel Specialization) : IRequest<SpecializationModel>;

	internal class Handler : IRequestHandler<Command, SpecializationModel>
	{
		private readonly TeachersHoursDbContext _dbContext;

		public Handler(TeachersHoursDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<SpecializationModel> Handle(Command request, CancellationToken cancellationToken)
		{
			var specialisationDb = await GetSpecialization(request.Specialization.Code);

			if (specialisationDb != null)
			{
				specialisationDb.Name = request.Specialization.Name;
				specialisationDb.EducationLevel = request.Specialization.EducationLevel;
			}
			else
			{
				var specialisation = request.Specialization.ToSpecialization();
				_dbContext.Specializations.Add(specialisation);
			}
			await _dbContext.SaveChangesAsync();

			return request.Specialization;
		}

		private async Task<Specialization?> GetSpecialization(string code) =>
			await _dbContext.Specializations.SingleOrDefaultAsync(x => x.Code == code);
	}
}
