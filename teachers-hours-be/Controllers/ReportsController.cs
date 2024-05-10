using MediatR;
using Microsoft.AspNetCore.Mvc;
using teachers_hours_be.Application.Commands;
using teachers_hours_be.Application.Models;
using teachers_hours_be.Application.Queries;
using TH.Dal.Entities;
using TH.Services.Models;

namespace teachers_hours_be.Controllers;

[ApiController]
[ProducesResponseType(StatusCodes.Status200OK)]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("add-file")]
    public async Task<Document> AddFile([FromForm] AddFileRequest request, CancellationToken cancellationToken) =>
        await _mediator.Send(new AddDocument.Command(request.File, request.DocumentType), cancellationToken);

    [HttpGet]
    public Task<IEnumerable<DocumentModel>> GetDocuments([FromQuery] GetDocumentsModel request, CancellationToken cancellationToken) =>
        _mediator.Send(new GetDocuments.Query(request.DocumentType), cancellationToken);

	[HttpGet("{documentId}")]
	public Task<DocumentModel> GetDocuments([FromRoute] Guid documentId, CancellationToken cancellationToken) =>
		_mediator.Send(new GetDocument.Query(documentId), cancellationToken);

	[HttpGet("{documentId}/subjects")]
    public Task<IEnumerable<SubjectModel>> GetSubjects([FromRoute] Guid documentId, CancellationToken cancellationToken) =>
        _mediator.Send(new GetSubjects.Query(documentId), cancellationToken);

    [HttpGet("{documentId}/download")]
    public async Task<IActionResult> GetDocumentFile([FromRoute] Guid documentId, CancellationToken cancellationToken)
    {
        var fileResult = await _mediator.Send(new GetDocumentFile.Query(documentId), cancellationToken);
        return File(fileResult.FileByteArray, fileResult.MimeType, fileResult.FileName);
    }

    [HttpPost("{documentId}/add-teachers")]
    public Task<DocumentModel> Test([FromRoute] Guid documentId, IEnumerable<string> teachersNames, CancellationToken cancellationToken) =>
        _mediator.Send(new AddTeachersToExcelDocument.Command(documentId, teachersNames), cancellationToken);

    [HttpPost("generate-calculation/{documentId}")]
    public Task<DocumentModel> GetCalculationFile([FromRoute] Guid documentId, CancellationToken cancellationToken) =>
        _mediator.Send(new GenerateCalculation.Command(documentId), cancellationToken);
}
