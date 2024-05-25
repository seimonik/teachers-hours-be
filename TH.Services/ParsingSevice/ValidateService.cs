using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace TH.Services.ParsingSevice;

public class ValidateService : IValidateService
{
	public readonly static IEnumerable<string> GroupForms = new[] { "комм", "бюдж" };
	public readonly static IEnumerable<int> CellsWithNumbers = new[] { 3, 4, 5, 8, 9, 10, 11, 12, 13 };
	public readonly static IEnumerable<string> ReportingForms = new[] { "зачет", "экзамен" };

	public async Task<byte[]?> ExecuteAsync(ValidateServiceContext context, CancellationToken cancellationToken)
	{
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		using (var package = new ExcelPackage(context.Stream))
		{
			var worksheet = package.Workbook.Worksheets.First();
			int rowMax = 1000;
			int row = 9;
			bool success = true;

			// TODO: Добавить при сохранении запись в БД строк начала и конца заявки
			for (; row < rowMax; row++)
			{
				string nameSubject = worksheet.Cells[row, 1].Value?.ToString();
				if (nameSubject == null || nameSubject.Trim() == string.Empty)
				{
					row = rowMax;
					continue;
				}

				// numbers
				foreach (var column in CellsWithNumbers)
				{
					if (worksheet.Cells[row, column].Value != null && !IsNumber(worksheet.Cells[row, column].Value))
					{
						success = false;
						PrintError(worksheet, row, column, "\r\nНеверный формат ячейки.\nОжидается целочисленное или пустое значение.");
					}
				}

				// GroupForm
				if (worksheet.Cells[row, 7].Value != null && !GroupForms.Contains(worksheet.Cells[row, 7].Value.ToString()!.ToLower()))
				{
					success = false;
					PrintError(worksheet, row, 7, "\r\nНеверный формат ячейки.\nВозможны следующие значения \"комм\", \"бюдж\" или пустое.");
				}

				// ReportingForm
				if (worksheet.Cells[row, 14].Value != null && !ReportingForms.Contains(worksheet.Cells[row, 14].Value.ToString()!.ToLower()))
				{
					success = false;
					PrintError(worksheet, row, 14, "\r\nНеверный формат ячейки.\nВозможны следующие значения \"зачет\", \"экзамен\" или пустое значение.");
				}
			}

			if (!success)
			{
				// генерация файла с пометками валидационных ошибок
				MemoryStream updatedFileStream = new MemoryStream();
				package.SaveAs(updatedFileStream);
				updatedFileStream.Position = 0;

				return updatedFileStream.ToArray();
			}

			return null;
		}
	}

	private bool IsNumber(object value)
	{
		if (int.TryParse(value.ToString(), out _))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private void PrintError(ExcelWorksheet worksheet, int row, int column, string message)
	{
		worksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
		worksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(Color.Orange);

		var errorComment = worksheet.Comments.Add(worksheet.Cells[row, column], $"Ошибка:", "СГУ Учёт");
		errorComment.From.Column = 7;
		errorComment.From.Row = 3;
		errorComment.To.Column = 14;
		errorComment.To.Row = 7;
		errorComment.BackgroundColor = Color.White;
		errorComment.RichText.Add(message);
	}
}
