using MediatR;
using Microsoft.EntityFrameworkCore;
using teachers_hours_be.Application.Models;
using teachers_hours_be.Application.Queries;
using teachers_hours_be.Constants;
using TH.Dal;
using TH.Dal.Entities;
using TH.Services.Models;
using TH.Services.RenderServices;

namespace teachers_hours_be.Application.Commands;

public static class GenerateCalculation
{
	public record Command(Guid RequestId) : IRequest<FileDownloadResult>;

	internal class Handler : IRequestHandler<Command, FileDownloadResult>
	{
		private readonly IRenderService _renderService;
		private readonly IMediator _mediator;
		private readonly TeachersHoursDbContext _dbContext;

		public Handler(IRenderService renderService, IMediator mediator, TeachersHoursDbContext dbContext)
		{
			_renderService = renderService;
			_mediator = mediator;
			_dbContext = dbContext;
		}

		public async Task<FileDownloadResult> Handle(Command request, CancellationToken cancellationToken)
		{
			var timeNormsLookups = await _mediator.Send(new GetTimeNorms.Query(), cancellationToken);
			var timeNorms = new TimeNorms
			{
				ProductionPractice = timeNormsLookups.Single(x => x.Code == "ProductionPractice").Norm,
				EducationalPractice23 = timeNormsLookups.Single(x => x.Code == "EducationalPractice23").Norm,
				EducationalPractice35 = timeNormsLookups.Single(x => x.Code == "EducationalPractice35").Norm,
				EducationalPractice24 = timeNormsLookups.Single(x => x.Code == "EducationalPractice24").Norm,
				EducationalPractice36 = timeNormsLookups.Single(x => x.Code == "EducationalPractice36").Norm,
				EducationalPractice47 = timeNormsLookups.Single(x => x.Code == "EducationalPractice47").Norm,
				Coursework2 = timeNormsLookups.Single(x => x.Code == "Coursework2").Norm,
				Coursework3 = timeNormsLookups.Single(x => x.Code == "Coursework3").Norm,
				Coursework2PO = timeNormsLookups.Single(x => x.Code == "Coursework2PO").Norm,
				Coursework3PO = timeNormsLookups.Single(x => x.Code == "Coursework3PO").Norm,
				FinalQualifyingWorkBachelor = timeNormsLookups.Single(x => x.Code == "FinalQualifyingWorkBachelor").Norm,
				FinalQualifyingWorkMagistracy = timeNormsLookups.Single(x => x.Code == "FinalQualifyingWorkMagistracy").Norm
			};
			(var file, int endRow) = await GetDocumentWithEndRow(request.RequestId, cancellationToken);

			var specializations = await _mediator.Send(new GetSpecializations.Query(), cancellationToken);

			var teacherRates = (await _mediator.Send(new GetTeachers.Query(), cancellationToken))
				.Select(x => new TeacherRateModel { FullName = x.FullName, Rate = x.Rate })
				.ToList();

			var context = new RenderServiceContext(file, "КНиИТ", timeNorms, endRow, specializations, teacherRates);
			var result = await _renderService.ExecuteAsync(context, cancellationToken);

			using MemoryStream ms = new MemoryStream();
			result.CopyTo(ms);

			return new FileDownloadResult { FileByteArray = ms.ToArray(), FileName = "Расчет_преподавательских_часов.xlsx", MimeType = MimeTypes.Xlsx };
		}

		private Task<Request> GetRequest(Guid requestId) =>
			_dbContext.Requests.AsNoTracking().SingleAsync(x => x.Id == requestId);

		private async Task<(Stream, int)> GetDocumentWithEndRow(Guid requestId, CancellationToken cancellationToken)
		{
			var request = await GetRequest(requestId);

			//var documrnt = await _dbContext.Documents.AsNoTracking().SingleAsync(x => x.Id == request.DocumentId);
			var result = await _mediator.Send(new GetDocumentFile.Query(request.DocumentId), cancellationToken);
			return (new MemoryStream(result.FileByteArray), request.EndRow);
		}
	}
}
