using MediatR;
using Microsoft.EntityFrameworkCore;
using teachers_hours_be.Application.Models.Lookups;
using teachers_hours_be.Extensions.ModelConversion;
using TH.Dal;

namespace teachers_hours_be.Application.Queries.Lookups;

public static class GetSpecializationsLookups
{
    public record Query() : IRequest<IEnumerable<SpecializationModel>>;

    internal class Handler : IRequestHandler<Query, IEnumerable<SpecializationModel>>
    {
        private readonly TeachersHoursDbContext _dbContext;

        public Handler(TeachersHoursDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<SpecializationModel>> Handle(Query request, CancellationToken cancellationToken)
        {
            var specializations = await _dbContext.Specializations
                .AsNoTracking()
                .ToListAsync();

            return specializations.Select(x => x.ToSpecializationModel());
        }
    }
}
