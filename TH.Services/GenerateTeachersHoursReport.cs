using OfficeOpenXml;
using System.Drawing;
using TH.Services.Models;

namespace TH.Services;

public class GenerateTeachersHoursReport
{
    TeachersWorkload workload = new TeachersWorkload();

    public async Task<FileResult> Run(GetTeachersHoursReportRequest request)
    {
        workload.Faculty = request.Faculty;
        workload.TimeNorms = request.TimeNorms;

        //Stream stream = new MemoryStream(request.File);
        var stream = request.File.OpenReadStream();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage(stream))
        {
            var worksheet = package.Workbook.Worksheets.First();//package.Workbook.Worksheets[0];
                                                                //var rowCount = worksheet.Dimension.Rows;
            int rowTotal = int.Parse(request.RowCount);

            for (var row = 9; row <= rowTotal; row++) // <= rowCount
            {
                try
                {
                    string nameTeacher = worksheet.Cells[row, 16].Value?.ToString() == null ? " " : worksheet.Cells[row, 16].Value.ToString();
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

            for (var row = request.SpecialtiesRow + 1; row <= request.SpecialtiesRow + 4; row++)
            {
                string[] line = worksheet.Cells[row, 1].Value.ToString().Split(":", StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, string> specializations = new Dictionary<string, string>();
                foreach (string specialization in line[1].Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] NameAndCode = specialization.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    specializations.Add(NameAndCode[0], NameAndCode[1]);
                }
                switch (row % request.SpecialtiesRow)
                {
                    case 1:
                        workload.Bachelor = specializations;
                        break;
                    case 2:
                        workload.Specialty = specializations;
                        break;
                    case 3:
                        workload.Magistracy = specializations;
                        break;
                    case 4:
                        workload.Postgraduate = specializations;
                        break;
                }
            }

            // Ставки преподавателей
            for (var row = request.RateRow + 1; worksheet.Cells[row, 1].Value != null; row++)
            {
                workload.teacherRate.Add(worksheet.Cells[row, 1].Value.ToString(),
                    new Teacher(double.Parse(worksheet.Cells[row, 2].Value.ToString())));
            }
        }
        return ExportToExcel();
    }

    public FileResult ExportToExcel()
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
            // Response.Clear();
        }
        stream.Position = 0;
        return new FileResult
        {
            File = stream,
            MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            FileName = "users.xlsx"
        };
    }
}
