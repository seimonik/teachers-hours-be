using Amazon.S3.Transfer;
using MediatR;
using Microsoft.Extensions.Options;
using teachers_hours_be.Application.Models;
using teachers_hours_be.Application.Queries;
using teachers_hours_be.Application.Queries.Lookups;
using teachers_hours_be.Extensions.ModelConversion;
using TH.Dal;
using TH.Dal.Entities;
using TH.Dal.Enums;
using TH.S3Client;
using TH.Services.Models;
using TH.Services.RenderServices;
using TH.Services.TeachersHoursService;

namespace teachers_hours_be.Application.Commands;

public static class GenerateCalculation
{
	public record Command(Guid DocumentId) : IRequest<DocumentModel>;

	internal class Handler : IRequestHandler<Command, DocumentModel>
	{
		private readonly IRenderService _renderService;
		private readonly IReportRenderService _reportRenderService;
		private readonly IMediator _mediator;
		private readonly TeachersHoursDbContext _dbContext;
		private readonly ITransferUtility _transferUtility;
		private readonly S3Options _s3options;

		public Handler(IRenderService renderService, 
					   IMediator mediator, 
					   TeachersHoursDbContext dbContext,
					   ITransferUtility transferUtility, 
					   IOptions<S3Options> options,
					   IReportRenderService reportRenderService)
		{
			_renderService = renderService;
			_mediator = mediator;
			_dbContext = dbContext;
			_transferUtility = transferUtility;
			_s3options = options.Value;
			_reportRenderService = reportRenderService;
		}

		public async Task<DocumentModel> Handle(Command request, CancellationToken cancellationToken)
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
			var file = await GetDocument(request.DocumentId, cancellationToken);

			var specializations = await _mediator.Send(new GetSpecializations.Query(), cancellationToken);

			var teacherRates = (await _mediator.Send(new GetTeachers.Query(), cancellationToken))
				.Select(x => new TeacherRateModel { FullName = x.FullName, Rate = x.Rate })
				.ToList();

			//var context = new RenderServiceContext(file, "КНиИТ", timeNorms, specializations, teacherRates);
			//var result = await _renderService.ExecuteAsync(context, cancellationToken);

			var result = await _reportRenderService.ExecuteAsync(new ReportRenderServiceContext(file, "КНиИТ", timeNorms, specializations, teacherRates), cancellationToken);

			return await AddDocument(result, request.DocumentId, cancellationToken);

			//using MemoryStream ms = new MemoryStream();
			//result.CopyTo(ms);

			//return new FileDownloadResult { FileByteArray = ms.ToArray(), FileName = "Расчет_преподавательских_часов.xlsx", MimeType = MimeTypes.Xlsx };
		}

		private async Task<Stream> GetDocument(Guid documentId, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new GetDocumentFile.Query(documentId), cancellationToken);
			return new MemoryStream(result.FileByteArray);
		}

		private async Task<DocumentModel> AddDocument(Stream fileStream, Guid parentDocumentId, CancellationToken cancellationToken)
		{
			string url = $"calculation/{Guid.NewGuid()}";
			var uploadRequest = new TransferUtilityUploadRequest
			{
				BucketName = _s3options.Bucket,
				Key = url,
				AutoCloseStream = true,
				InputStream = fileStream
			};
			await _transferUtility.UploadAsync(uploadRequest, cancellationToken);

			var document = new Document
			{
				Name = $"Расчет_часов_{DateTime.Now}.xlsx",
				Url = url,
				CreatedAt = DateTime.UtcNow,
				DocumentType = DocumentTypes.Calculation,
				ParentDocumentId = parentDocumentId
			};
			_dbContext.Documents.Add(document);
			await _dbContext.SaveChangesAsync();

			return document.ToDocumentModel();
		}
	}
}
