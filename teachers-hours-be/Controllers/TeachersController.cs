using MediatR;
using Microsoft.AspNetCore.Mvc;
using teachers_hours_be.Application.Commands;
using teachers_hours_be.Application.Models;
using teachers_hours_be.Application.Queries;
using TH.Dal.Entities;

namespace teachers_hours_be.Controller;

[ApiController]
[ProducesResponseType(StatusCodes.Status200OK)]
[Route("api/teachers")]
public class TeachersController : ControllerBase
{
	private readonly IMediator _mediator;

	public TeachersController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost]
	public Task<Teacher> AddTeacher(AddOrUpdateTeacherModel teacher, CancellationToken cancellationToken) =>
		_mediator.Send(new AddTeacher.Command(teacher), cancellationToken);

	[HttpGet]
	public Task<IEnumerable<GetTeacherModel>> GetTeachers(CancellationToken cancellationToken) =>
		_mediator.Send(new GetTeachers.Query(), cancellationToken);
}
