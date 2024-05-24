using Newtonsoft.Json;
using OfficeOpenXml;
using System.Drawing;
using TH.Services.Models;

namespace TH.Services.TeachersHoursService;

public class ReportRenderService : IReportRenderService
{
	public async Task<Stream> ExecuteAsync(ReportRenderServiceContext context, CancellationToken cancellationToken)
	{
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

		using (var package = new ExcelPackage(context.File))
		{
			var worksheet = package.Workbook.Worksheets.First();
			int rowMax = 1000;

			var subjects = new List<Subject>();

			string prevSubject = string.Empty;
			// Начало парсинга с 9 строки !!! (Рассмотреть дальнейшее расширение гибкости)
			for (var row = 9; row < rowMax; row++)
			{
				try
				{
					// колонка "P" предназначена для ФИО преподавателя
					bool practice = false;
					string nameSubject = worksheet.Cells[row, 1].Value?.ToString();

					if (nameSubject == null || nameSubject.Trim() == string.Empty)
					{
						row = rowMax;
						continue;
					}

					if (nameSubject == "--//--")
					{
						nameSubject = prevSubject;
						practice = true;
					}
					else
					{
						prevSubject = nameSubject;
					}

					Subject subject = new Subject()
					{
						Name = nameSubject,
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
						Teacher = JsonConvert.DeserializeObject<IEnumerable<TeacherStudents>>(worksheet.Cells[row, 16].Value?.ToString()),
						practice = practice
					};

					subjects.Add(subject);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Something went wrong"); // Вылавливание ошибки и вывод строки, в которой проихошла ошибка
				}
			}

			var stream = new MemoryStream();
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			using (var xlPackage = new ExcelPackage(stream))
			{
				var worksheetP1 = xlPackage.Workbook.Worksheets.Add("p1");
				var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
				namedStyle.Style.Font.UnderLine = true;
				namedStyle.Style.Font.Color.SetColor(Color.Blue);
				const int startRow = 5;
				var row = startRow;

				worksheetP1.PrintTableHeader(true, 7, context.Faculty);
				worksheetP1.PrintToExcelP(
					subjects, context.TimeNorms, context.Specializations, context.Faculty, 13, true);

				var worksheetP2 = xlPackage.Workbook.Worksheets.Add("p2");
				worksheetP2.PrintTableHeader(true, 7, context.Faculty);
				worksheetP2.PrintToExcelP(
					subjects, context.TimeNorms, context.Specializations, context.Faculty, 13, false);

				var worksheetC1 = xlPackage.Workbook.Worksheets.Add("c1");
				var teacherGenerateInfo = new List<TeacherGenerateModel>();
				var teacherSubjects = subjects.Select(x => x.ToTeacherSubject()).SelectMany(x => x);
				worksheetC1.PrintToExcelC(teacherSubjects, context.Faculty, true, teacherGenerateInfo);

				var worksheetC2 = xlPackage.Workbook.Worksheets.Add("c2");
				worksheetC2.PrintToExcelC(teacherSubjects, context.Faculty, false, teacherGenerateInfo);

				var worksheetO1 = xlPackage.Workbook.Worksheets.Add("o1");
				worksheetO1.PrintToExcelO(true, teacherGenerateInfo);

				var worksheetO2 = xlPackage.Workbook.Worksheets.Add("o2");
				worksheetO2.PrintToExcelO(false, teacherGenerateInfo);

				// set some core property values
				xlPackage.Workbook.Properties.Title = "User List";
				xlPackage.Workbook.Properties.Author = "WebSSU";
				xlPackage.Workbook.Properties.Subject = "User List";
				// save the new spreadsheet
				xlPackage.Save();
			}
			stream.Position = 0;

			return stream;
		}
	}
}
