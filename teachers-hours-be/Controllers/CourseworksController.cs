using MediatR;
using Microsoft.AspNetCore.Mvc;
using teachers_hours_be.Constants;
using TH.Services.GenerateDocxService;

namespace teachers_hours_be.Controllers;

[ApiController]
[ProducesResponseType(StatusCodes.Status200OK)]
[Route("api/courseworks")]
public class CourseworksController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly IRenderCourseworkJournalService _journalService;

	public CourseworksController(IMediator mediator, IRenderCourseworkJournalService journalService)
	{
		_mediator = mediator;
		_journalService = journalService;
	}

	[HttpPost]
	public async Task<IActionResult> GetCourseworkJournal(Dictionary<string, Dictionary<string, int>> courseworkJournal, CancellationToken cancellationToken)
	{
		var fileByte = await _journalService.ExecuteAsync(new RenderCourseworkJournalContext(courseworkJournal), cancellationToken);
		return File(fileByte, MimeTypes.Docx, "filetest.docx");
	}
}
