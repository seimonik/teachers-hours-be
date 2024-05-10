using teachers_hours_be.Application.Models;
using TH.Dal.Entities;

namespace teachers_hours_be.Extensions.ModelConversion;

public static class DocumentConversionExtensions
{
	public static DocumentModel ToDocumentModel(this Document document) =>
		new DocumentModel
		{
			Id = document.Id,
			Name = document.Name,
			CreatedAt = document.CreatedAt,
			DocumentType = document.DocumentType,
			ChildDocuments = document.ChildDocuments?.Select(x => x.ToDocumentModel()) 
			?? Enumerable.Empty<DocumentModel>()
		};
}
