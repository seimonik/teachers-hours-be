using MediatR;
using Microsoft.AspNetCore.Mvc;
using teachers_hours_be.Application.Commands.Lookups;
using teachers_hours_be.Application.Models.Lookups;
using teachers_hours_be.Application.Queries.Lookups;

namespace teachers_hours_be.Controllers;

[ApiController]
[ProducesResponseType(StatusCodes.Status200OK)]
[Route("api/lookups")]
public class LookupsController : ControllerBase
{
	private readonly IMediator _mediator;

	public LookupsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("specializations")]
	public Task<IEnumerable<SpecializationModel>> GetSpecializations(CancellationToken cancellationToken) =>
		_mediator.Send(new GetSpecializationsLookups.Query(), cancellationToken);

	[HttpPost("specializations")]
	public Task<SpecializationModel> AddOrUpdateSpecialization(SpecializationModel specialization, CancellationToken cancellationToken) =>
		_mediator.Send(new AddOrUpdateSpecializations.Command(specialization), cancellationToken);

	[HttpDelete("specializations/{code}")]
	public Task<Unit> DeleteSpecialisation([FromRoute] string code, CancellationToken cancellationToken) =>
		_mediator.Send(new DeleteSpecialisation.Command(code), cancellationToken);

	[HttpGet("time-norms")]
	public Task<IEnumerable<TimeNormModel>> GetTimeNorms(CancellationToken cancellationToken) =>
		_mediator.Send(new GetTimeNorms.Query(), cancellationToken);
}
