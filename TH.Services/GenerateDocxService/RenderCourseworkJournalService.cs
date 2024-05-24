using Xceed.Document.NET;
using Xceed.Words.NET;

namespace TH.Services.GenerateDocxService;

internal sealed class RenderCourseworkJournalService : IRenderCourseworkJournalService
{
	public async Task<byte[]> ExecuteAsync(RenderCourseworkJournalContext context, CancellationToken cancellationToken)
	{
		MemoryStream stream = new MemoryStream();
		// Create a new document
		DocX document = DocX.Create(stream);

		foreach(var course in context.CourseworkJournal)
		{
			document.InsertParagraph(course.Key).Font("Times New Roman").FontSize(16).Alignment = Alignment.center;

			int rowCount = 1;
			foreach (var teachers in course.Value)
			{
				rowCount += teachers.StudentsCount;
			}

			Table teachersTable = document.AddTable(rowCount, 3);
			teachersTable.Rows[0].Cells[0].Paragraphs[0].Append("ФИО преподавателя").FontSize(16).Font("Times New Roman");
			teachersTable.Rows[0].Cells[1].Paragraphs[0].Append("Тема курсовой работы").FontSize(16).Font("Times New Roman");
			teachersTable.Rows[0].Cells[2].Paragraphs[0].Append("ФИО студента").FontSize(16).Font("Times New Roman");

			int row = 1;
			foreach (var teachers in course.Value)
			{
				teachersTable.Rows[row].Cells[0].Paragraphs[0].Append(teachers.TeacherName).FontSize(14).Font("Times New Roman");
				int endRow = row + teachers.StudentsCount - 1;
				if (row < endRow)
					teachersTable.MergeCellsInColumn(0, row, endRow);

				row = endRow + 1;
			}
			document.InsertTable(teachersTable);
		}

		document.Save();
		
		return stream.ToArray();
	}
}
