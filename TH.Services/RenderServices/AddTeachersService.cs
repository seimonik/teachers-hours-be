using OfficeOpenXml;

namespace TH.Services.RenderServices;

internal sealed class AddTeachersService : IAddTeachersService
{
	public async Task<byte[]> ExecuteAsync(AddTeachersServiceContext context, CancellationToken cancellationToken)
	{
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		using (var package = new ExcelPackage(context.Stream))
		{
			var worksheet = package.Workbook.Worksheets[0];

			int row = 9;
			foreach (var teacherName in context.TeachersFullNames)
			{
				worksheet.Cells[$"P{row}"].Value = teacherName;
				row++;
			}

			MemoryStream updatedFileStream = new MemoryStream();
			package.SaveAs(updatedFileStream);
			updatedFileStream.Position = 0;

			return updatedFileStream.ToArray();
		}
	}
}
