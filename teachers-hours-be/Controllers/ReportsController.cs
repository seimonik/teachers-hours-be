using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using teachers_hours_be.Application.Commands;
using teachers_hours_be.Application.Models;
using teachers_hours_be.Application.Queries;
using teachers_hours_be.Constants;
using TH.Dal.Entities;
using TH.Services.Models;
using TH.Services.ParsingSevice;
using GetSubjects = teachers_hours_be.Application.Queries.GetSubjects;

namespace teachers_hours_be.Controllers;

[ApiController]
[ProducesResponseType(StatusCodes.Status200OK)]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidateService _validateService;

	public ReportsController(IMediator mediator, IValidateService validateService)
    {
        _mediator = mediator;
        _validateService = validateService;
    }

    [HttpPost("add-file")]
    public async Task<Document> AddFile([FromForm] AddFileRequest request, CancellationToken cancellationToken) =>
        await _mediator.Send(new AddDocument.Command(
            request.File, request.DocumentType), cancellationToken);

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
    public Task<DocumentModel> AddTeachersToExcelDocument([FromRoute] Guid documentId, [FromBody] IEnumerable<IEnumerable<TeacherStudents>> teachersNames, CancellationToken cancellationToken) =>
        _mediator.Send(new AddTeachersToExcelDocument.Command(documentId, teachersNames), cancellationToken);

    [HttpPost("generate-calculation/{documentId}")]
    public Task<DocumentModel> GetCalculationFile([FromRoute] Guid documentId, CancellationToken cancellationToken) =>
        _mediator.Send(new GenerateCalculation.Command(documentId), cancellationToken);

    [HttpDelete("{documentId}")]
    public async Task<IActionResult> DeleteDocument([FromRoute] Guid documentId, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new DeleteDocument.Command(documentId), cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }

    [HttpPost("validate")]
    public async Task<IActionResult> ValidateDocument([FromForm] FileValidateModel request, CancellationToken cancellationToken)
    {
        var fileStream = request.File.OpenReadStream();
        var result = await _validateService.ExecuteAsync(new ValidateServiceContext(fileStream), cancellationToken);

        if (result == null)
        {
            return Ok();
        }

        return File(result, MimeTypes.Xlsx, "валидационные ошибки.xlsx");
    }
}
