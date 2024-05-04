﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using teachers_hours_be.Application.Commands;
using teachers_hours_be.Application.Models;
using teachers_hours_be.Application.Queries;
using teachers_hours_be.Constants;
using TH.Dal.Entities;
using TH.Services.Models;
using TH.Services.RenderServices;

namespace teachers_hours_be.Controllers;

[ApiController]
[ProducesResponseType(StatusCodes.Status200OK)]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IRenderService _renderService;

    public ReportsController(IMediator mediator, IRenderService renderService)
    {
        _mediator = mediator;
        _renderService = renderService;
    }

    // TODO: !!! Будет добавлен после подключени БД !!!

    //[HttpPost("download")]
    //[ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    //public async Task<ActionResult> GenerateTeachersHoursReport([FromForm] GetTeachersHoursReportRequest request, CancellationToken cancellationToken)
    //{
    //    var context = new RenderServiceContext(request);
    //    var result = await _renderService.ExecuteAsync(context, cancellationToken);

    //    return File(result, MimeTypes.Xlsx, "test_help.xlsx");
    //}

    [HttpPost("add-file")]
    public async Task<Document> AddFile([FromForm] AddFileRequest request, CancellationToken cancellationToken) =>
        await _mediator.Send(new AddDocument.Command(request.File, request.DocumentType), cancellationToken);

    [HttpGet]
    public Task<IEnumerable<DocumentModel>> GetDocuments([FromQuery] GetDocumentsModel request, CancellationToken cancellationToken) =>
        _mediator.Send(new GetDocuments.Query(request.DocumentType), cancellationToken);

    [HttpGet("{documentId}")]
    public Task<IEnumerable<SubjectModel>> GetSubjects([FromRoute] Guid documentId, CancellationToken cancellationToken) =>
        _mediator.Send(new GetSubjects.Query(documentId), cancellationToken);

    [HttpGet("{documentId}/download")]
    public async Task<IActionResult> GetDocumentFile([FromRoute] Guid documentId, CancellationToken cancellationToken)
    {
        var fileByte = await _mediator.Send(new GetDocumentFile.Query(documentId), cancellationToken);
        return File(fileByte, MimeTypes.Docx, "test.docx");
    }

	[HttpPost("{documentId}/add-teachers")]
	public async Task<IActionResult> Test([FromRoute] Guid documentId, IEnumerable<string> teachersNames, CancellationToken cancellationToken)
    {
        var fileStream = await _mediator.Send(new AddTeachersToExcelDocument.Command(documentId, teachersNames), cancellationToken);

        return File(fileStream, MimeTypes.Xlsx, "test.xlsx");
	}
}
