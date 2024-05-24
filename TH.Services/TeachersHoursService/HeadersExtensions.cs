using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace TH.Services.TeachersHoursService;

public static class HeadersExtensions
{
	public static ExcelWorksheet PrintTableHeader(this ExcelWorksheet worksheet, bool selfStudy, int startRow, string faculty)
	{
		// Шрифт шапки таблицы
		worksheet.Cells[$"A{startRow}:Z{startRow + 2}"].Style.Font.Name = "Garamond";
		worksheet.Cells[$"A{startRow}:Z{startRow + 2}"].Style.Font.Size = 7;

		// Выравнивание по центру
		worksheet.Cells[$"A{startRow}:Z{startRow + 2}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
		worksheet.Cells[$"A{startRow}:Z{startRow + 2}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

		// Перенос текста
		worksheet.Cells[$"A{startRow}:Z{startRow + 2}"].Style.WrapText = true;

		// ширина столбцов и высота строк заголовка
		worksheet.Row(9).Height = 51.8;
		worksheet.Column(1).Width = 28.22;
		worksheet.Column(2).Width = 17.78;
		for (int i = 3; i < 9; i++)
		{
			worksheet.Column(i).Width = 5.11;
		}
		for (int i = 9; i < 27; i++)
		{
			worksheet.Column(i).Width = 7.67;
		}

		worksheet.Cells[$"A{startRow}:A{startRow + 2}"].Merge = true;
		worksheet.Cells[$"A{startRow}"].Value = "Наименование дисциплины";

		// Поворот текста на 90
		worksheet.Cells[$"I{startRow + 1}:Y{startRow + 2}"].Style.TextRotation = 90;
		worksheet.Cells[$"B{startRow}:H{startRow + 2}"].Style.TextRotation = 90;

		// Слияние ячеек
		for (char c = 'B'; c <= 'H'; c++)
		{
			worksheet.Cells[$"{c}{startRow}:{c}{startRow + 2}"].Merge = true;
		}
		worksheet.Cells[$"B{startRow}"].Value = "Специальность или направление(код специальности или направления)";
		worksheet.Cells[$"C{startRow}"].Value = "Курс";
		worksheet.Cells[$"D{startRow}"].Value = "Семестр";
		worksheet.Cells[$"E{startRow}"].Value = "Количество студентов";
		worksheet.Cells[$"F{startRow}"].Value = "Количество потоков";
		worksheet.Cells[$"G{startRow}"].Value = "Количество групп/подгрупп";

		if (selfStudy)
		{
			worksheet.Cells[$"H{startRow}"].Value = "Самостоятельная работа по дисциплине в семестре (в часах)*";
		}

		worksheet.Cells[$"I{startRow}"].Value = "Число часов по видам учебной работы";
		worksheet.Cells[$"I{startRow}:Y{startRow}"].Merge = true;
		for (char c = 'I'; c <= 'Y'; c++)
		{
			worksheet.Cells[$"{c}{startRow + 1}:{c}{startRow + 2}"].Merge = true;
		}
		worksheet.Cells[$"I{startRow + 1}"].Value = "Лекции";
		worksheet.Cells[$"J{startRow + 1}"].Value = "Практ., семин. занятия";
		worksheet.Cells[$"K{startRow + 1}"].Value = "Лабор. занятия";
		worksheet.Cells[$"L{startRow + 1}"].Value = "Консультации по дисциплине, КСР";
		worksheet.Cells[$"M{startRow + 1}"].Value = "Консультации перед экзаменом";
		worksheet.Cells[$"N{startRow + 1}"].Value = "Экзамены";
		worksheet.Cells[$"O{startRow + 1}"].Value = "Зачеты";
		worksheet.Cells[$"P{startRow + 1}"].Value = "Руководство практикой";
		worksheet.Cells[$"Q{startRow + 1}"].Value = "Курсовые работы";
		worksheet.Cells[$"R{startRow + 1}"].Value = "Выпускные квалиф. работы";
		worksheet.Cells[$"S{startRow + 1}"].Value = "Работа в ГАК";
		worksheet.Cells[$"T{startRow + 1}"].Value = "Проверка контр. работ";
		worksheet.Cells[$"U{startRow + 1}"].Value = "Руководство аспирантами";
		worksheet.Cells[$"V{startRow + 1}"].Value = "Руководство соискателями";
		worksheet.Cells[$"W{startRow + 1}"].Value = "Руководство магис-терской программой";
		worksheet.Cells[$"X{startRow + 1}"].Value = "Факультативные занятия";

		worksheet.Cells[$"Z{startRow}:Z{startRow + 2}"].Merge = true;
		worksheet.Column(26).Width = 7.67;
		worksheet.Cells[$"Z{startRow}"].Value = "Итого (часов)";
		int col = 1;
		for (char c = 'A'; c <= 'Z'; c++)
		{
			worksheet.Cells[$"{c}{startRow + 3}"].Value = col;
			col++;
		}
		worksheet.Cells[$"A{startRow + 3}:Z{startRow + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
		worksheet.Cells[$"A{startRow + 3}:Z{startRow + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

		var FirstTableRange = worksheet.Cells[$"A{startRow}:Z{startRow + 3}"];
		FirstTableRange.Style.Border.Top.Style = ExcelBorderStyle.Medium;
		FirstTableRange.Style.Border.Left.Style = ExcelBorderStyle.Medium;
		FirstTableRange.Style.Border.Right.Style = ExcelBorderStyle.Medium;
		FirstTableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

		worksheet.Cells[$"A{startRow + 4}:Z{startRow + 4}"].Merge = true;
		worksheet.Cells[$"A{startRow + 4}:Z{startRow + 4}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
		worksheet.Cells[$"A{startRow + 4}:Z{startRow + 4}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
		worksheet.Cells[$"A{startRow + 4}"].Value = "Очная форма обучения";
		worksheet.Cells[$"A{startRow + 4}"].Value = faculty;

		return worksheet;
	}
}
