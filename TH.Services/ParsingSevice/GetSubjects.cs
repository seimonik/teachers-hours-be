﻿using OfficeOpenXml;
using TH.Services.Models;

namespace TH.Services.ParsingSevice;

public class GetSubjects : IParsingService
{
	public async Task<IEnumerable<SubjectModel>> ExecuteAsync(ParsingServiceContext context, CancellationToken cancellationToken)
	{
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		using (var package = new ExcelPackage(context.Stream))
		{
			var worksheet = package.Workbook.Worksheets.First();
			var result = new List<SubjectModel>();

			// TODO: Добавить при сохранении запись в БД строк начала и конца заявки
			for (var row = 1; row <= 7; row++)
			{
				SubjectModel subject = new SubjectModel()
				{
					Name = worksheet.Cells[row, 1].Value?.ToString(),
					Specialization = worksheet.Cells[row, 2].Value.ToString(),
					Semester = int.Parse(worksheet.Cells[row, 3].Value.ToString()),
					Budget = worksheet.Cells[row, 4].Value == null ? 0 : int.Parse(worksheet.Cells[row, 4].Value.ToString()),
					Commercial = worksheet.Cells[row, 5].Value == null ? 0 : int.Parse(worksheet.Cells[row, 5].Value.ToString()),
					Groups = worksheet.Cells[row, 6].Value.ToString(),
					GroupForm = worksheet.Cells[row, 7].Value.ToString(),
					TotalHours = worksheet.Cells[row, 8].Value == null ? null : int.Parse(worksheet.Cells[row, 8].Value?.ToString()),
					Lectures = worksheet.Cells[row, 9].Value == null ? null : int.Parse(worksheet.Cells[row, 9].Value?.ToString()),
					Seminars = worksheet.Cells[row, 10].Value == null ? null : int.Parse(worksheet.Cells[row, 10].Value?.ToString()),
					Laboratory = worksheet.Cells[row, 11].Value == null ? null : int.Parse(worksheet.Cells[row, 11].Value?.ToString()),
					SelfStudy = worksheet.Cells[row, 12].Value == null ? null : int.Parse(worksheet.Cells[row, 12].Value?.ToString()),
					LoadPerWeek = worksheet.Cells[row, 13].Value == null ? null : int.Parse(worksheet.Cells[row, 13].Value?.ToString()),
					ReportingForm = worksheet.Cells[row, 14].Value?.ToString(),
					Remark = worksheet.Cells[row, 15].Value?.ToString(),
					TeacherFullName = worksheet.Cells[row, 16].Value?.ToString()
				};
				result.Add(subject);
			}

			return result;
		}
	}
}
