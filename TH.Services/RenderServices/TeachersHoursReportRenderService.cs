using OfficeOpenXml;
using System.Drawing;
using TH.Services.Models;

namespace TH.Services.RenderServices;

internal sealed class TeachersHoursReportRenderService : IRenderService
{
    public async Task<Stream> ExecuteAsync(RenderServiceContext context, CancellationToken cancellationToken)
    {
        TeachersWorkload workload = new TeachersWorkload();
        workload.Faculty = context.Faculty;
        workload.TimeNorms = context.TimeNorms;

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(context.File))
        {
            var worksheet = package.Workbook.Worksheets.First();
            int rowTotal = context.RowCount;

            // Начало парсинга с 9 строки !!! (Рассмотреть дальнейшее расширение гибкости)
            for (var row = 9; row <= rowTotal; row++)
            {
                try
                {
                    // колонка "P" предназначена для ФИО преподавателя
                    string nameTeacher = worksheet.Cells[row, 16].Value?.ToString() ?? " ";
                    Subject subject = new Subject()
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
                        Remark = worksheet.Cells[row, 15].Value?.ToString()
                    };
                    workload.Add(nameTeacher, subject);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Something went wrong"); // Вылавливание ошибки и вывод строки, в которой проихошла ошибка
                }
            }

			// Направления/специальности, их наименования
			workload.Bachelor = context.Specializations.Bachelor;
            workload.Specialty = context.Specializations.Specialty;
            workload.Magistracy = context.Specializations.Magistracy;
            workload.Postgraduate = context.Specializations.Postgraduate;

            // Ставки преподавателей
            foreach(var teacherRate in context.TeacherRates)
            {
                workload.teacherRate.Add(teacherRate.FullName, new Teacher(teacherRate.Rate));
            }
        }

        return ExportToExcel(workload);
    }

    public Stream ExportToExcel(TeachersWorkload workload)
    {
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

            workload.PrintTableHeader(worksheetP1, true, 7);
            workload.PrintToExcelP(worksheetP1, true);

            var worksheetP2 = xlPackage.Workbook.Worksheets.Add("p2");
            workload.PrintTableHeader(worksheetP2, true, 7);
            workload.PrintToExcelP(worksheetP2, false);

            var worksheetC1 = xlPackage.Workbook.Worksheets.Add("c1");
            workload.PrintToExcelC(worksheetC1, true);

            var worksheetC2 = xlPackage.Workbook.Worksheets.Add("c2");
            workload.PrintToExcelC(worksheetC2, false);

            var worksheetO1 = xlPackage.Workbook.Worksheets.Add("o1");
            workload.PrintToExcelO(worksheetO1, true);

            var worksheetO2 = xlPackage.Workbook.Worksheets.Add("o2");
            workload.PrintToExcelO(worksheetO2, false);

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
