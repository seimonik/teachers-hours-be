﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using teachers_hours_be.Application.Commands;
using teachers_hours_be.Application.Models;
using teachers_hours_be.Constants;
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
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddFile([FromForm] AddFileRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddFileToS3.Query(request.File), cancellationToken);
        return Ok();
    }
}