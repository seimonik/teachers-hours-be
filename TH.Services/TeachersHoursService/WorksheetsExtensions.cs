using OfficeOpenXml;
using OfficeOpenXml.Style;
using TH.Services.Constants;
using TH.Services.Models;
using Xceed.Document.NET;

namespace TH.Services.TeachersHoursService;

public static class WorksheetsExtensions
{
	private static readonly string[] ClockCells =
		new string[] { "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
	private static readonly string[] CalculationCellsInCWorksheet =
		new string[] { "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T" };

	public static ExcelWorksheet PrintToExcelP(this ExcelWorksheet worksheet, 
		IEnumerable<Subject> subjects, TimeNorms timeNorms, SpecializationsModel specializations, 
		string faculty, int startRow, bool budget)
	{
		int row = startRow;
		TotalHours totalHours = new TotalHours();

		var subjectsByName = subjects.GroupBy(s => s.Name);
		foreach (var subjectGroup in subjectsByName)
		{
			foreach (var subject in subjectGroup)
			{
				var studentNumber = budget ? subject.Budget : subject.Commercial;

				if (studentNumber == null || studentNumber == 0)
					continue;

				worksheet.Cells[$"A{row}"].Value = subject.Name;
				worksheet.Cells[$"B{row}"].Value = subject.Specialization;
				// курс
				worksheet.Cells[$"C{row}"].Value = (subject.Semester % 2) + 1;
				worksheet.Cells[$"D{row}"].Value = subject.Semester;
				worksheet.Cells[$"E{row}"].Value = studentNumber;
				// количество потоков
				int countFlow = subject.Specialization.Split(",", StringSplitOptions.RemoveEmptyEntries).Length;
				worksheet.Cells[$"F{row}"].Value = countFlow;
				// количество групп
				int countGroups = subject.Groups.Split(",", StringSplitOptions.RemoveEmptyEntries).Length;
				worksheet.Cells[$"G{row}"].Value = countGroups;
				worksheet.Cells[$"H{row}"].Value = subject.SelfStudy;

				// РУКОВОДСТВО ПРАКТИКОЙ
				string nameLower = subject.Name.ToLower();
				if (nameLower.Contains("практика"))
				{
					if (PracticesNames.ProductionPractice.Any(x => nameLower.Contains(x)))
					{
						worksheet.Cells[$"P{row}"].Formula = $"={timeNorms.ProductionPractice} * F{row}";
					}
					else if (nameLower.Contains("учебная") || nameLower.Contains("НИР"))
					{
						var educationalPractice = subject.Semester switch
						{
							3 => timeNorms.EducationalPractice23,
							5 => timeNorms.EducationalPractice35,
							4 => timeNorms.EducationalPractice24,
							6 => timeNorms.EducationalPractice36,
							7 => timeNorms.EducationalPractice47,
						};
						worksheet.Cells[$"P{row}"].Formula = $"{educationalPractice} * F{row}";
					}
					else if (nameLower.Contains("педагогическая") || nameLower.Contains("научно"))
					{
						worksheet.Cells[$"P{row}"].Formula = $"8 * E{row}";
					}
				}
				else if (nameLower.Contains("консультация"))
				{
					worksheet.Cells[$"P{row}"].Formula = $"10 * E{row}";
				}
				// Работы в ГАК
				else if (nameLower.Contains("ГЭК"))
				{
					if (subject.ReportingForm.Contains("секретарь"))
					{
						worksheet.Cells[$"S{row}"].Formula = $"=3*ROUND(E{row}/2,1)";
					}
					else
					{
						worksheet.Cells[$"S{row}"].Formula = $"=3*ROUND((3*E{row})/4,1)";
					}
				}
				// ВКР
				else if (nameLower.Contains("ВКР"))
				{
					string[] specialization = subject.Specialization.Split(",", StringSplitOptions.RemoveEmptyEntries);
					if (specializations.Magistracy.ContainsKey(specialization[0]))
					{
						// магистратура
						worksheet.Cells[$"R{row}"].Formula = $"{timeNorms.FinalQualifyingWorkMagistracy} * E{row}";
					}
					else if (specializations.Bachelor.ContainsKey(specialization[0]))
					{
						// бакалавриат
						worksheet.Cells[$"R{row}"].Formula = $"{timeNorms.FinalQualifyingWorkBachelor} * E{row}";
					}
				}
				// курсовая работа
				else if (nameLower.Contains("курсовая работа"))
				{
					int year = ((subject.Semester ?? 0) % 2) + 1;
					if (specializations.Bachelor.ContainsKey(subject.Specialization)
						&& specializations.Bachelor[subject.Specialization] == "ПО" && year == 2)
					{
						worksheet.Cells[$"Q{row}"].Formula = $"{timeNorms.Coursework2PO} * E{row}";
					}
					else if (specializations.Bachelor.ContainsKey(subject.Specialization)
						&& specializations.Bachelor[subject.Specialization] == "ПО" && year == 3)
					{
						worksheet.Cells[$"Q{row}"].Formula = $"{timeNorms.Coursework3PO} * E{row}";
					}
					else if (year == 2)
					{
						worksheet.Cells[$"Q{row}"].Formula = $"{timeNorms.Coursework2} * E{row}";
					}
					else if (year == 3)
					{
						worksheet.Cells[$"Q{row}"].Formula = $"{timeNorms.Coursework3} * E{row}";
					}
				}
				else if (subject.practice)
				{
					worksheet.Cells[$"J{row}"].Value = subject.Seminars;
					worksheet.Cells[$"K{row}"].Value = subject.Laboratory;
				}
				// лекция ??
				else
				{
					worksheet.Cells[$"I{row}"].Value = subject.Lectures;
					worksheet.Cells[$"J{row}"].Value = subject.Seminars;
					worksheet.Cells[$"K{row}"].Value = subject.Laboratory;
					worksheet.Cells[$"L{row}"].Formula = $"=IF(ROUND(F{row}*H{row}*0.05,1)>2*F{row},2*F{row},ROUND(F{row}*H{row}*0.05,1))";

					if (subject.ReportingForm == "зачет")
					{
						worksheet.Cells[$"O{row}"].Formula = $"=ROUND(E{row}/3,1)";
					}
					else if (subject.ReportingForm == "экзамен")
					{
						worksheet.Cells[$"M{row}"].Formula = $"2 * F{row}";
						worksheet.Cells[$"N{row}"].Formula = $"=ROUND(E{row}/2,1)";
					}

					// Проверка контрольных работ
					worksheet.Cells[$"T{row}"].Formula = $"=ROUND(E{row}/4,1)";
				}

				worksheet.Cells[$"Z{row}"].Formula = $"=SUM(I{row}:Y{row})";
				row++;
			}
		}
		worksheet.Cells[$"A{startRow}:A{row}"].Style.WrapText = true;
		worksheet.Cells[$"A{startRow}:A{row}"].Style.Font.Name = "Times New Roman";
		worksheet.Cells[$"A{startRow}:A{row}"].Style.Font.Size = 10;
		worksheet.Cells[$"A{row}"].Value = "Итого по " + faculty;

		foreach (var cell in ClockCells)
		{
			worksheet.Cells[$"{cell}{row}"].Formula = $"=SUM({cell}{startRow}:{cell}{row - 1})";
		}

		return worksheet;
	}

	public static ExcelWorksheet PrintToExcelC(this ExcelWorksheet worksheet, 
		IEnumerable<TeacherSubject> subjects, string faculty, bool budget, List<TeacherGenerateModel> teacherGenerateInfo)
	{
		int row = 1;

		var subjectsByTeacher = subjects.GroupBy(s => s.TeacherName);
		foreach (var teacherSubjects in subjectsByTeacher)
		{
			worksheet.Cells[$"A{row}"].Value = "Карточка учебных поручений на 2023/2024 учебный год";
			worksheet.Cells[$"A{row}:I{row}"].Merge = true;
			worksheet.Cells[$"A{++row}"].Value = "Фамилия, имя, отчество преподавателя " + teacherSubjects.Key;
			worksheet.Cells[$"A{row}:I{row}"].Merge = true;
			worksheet.Cells[$"A{++row}"].Value = "Ученая степень, ученое звание ____________________________________";
			worksheet.Cells[$"A{row}:I{row}"].Merge = true;
			worksheet.Cells[$"N{row}"].Value = "Форма нагрузки бюджетная";
			worksheet.Cells[$"N{row}:S{row}"].Merge = true;
			worksheet.Cells[$"A{++row}"].Value = "Должность, ставка";
			worksheet.Cells[$"A{row}:I{row}"].Merge = true;
			worksheet.Cells[$"N{row}"].Value = "Кафедра  информатики и программирования";
			worksheet.Cells[$"N{row}:S{row}"].Merge = true;
			worksheet.Cells[$"A{++row}"].Value = "Основная, внутреннее совмещение, внешнее совмещение, почасовая оплата";
			worksheet.Cells[$"A{row}:I{row}"].Merge = true;
			worksheet.Cells[$"N{row}"].Value = "Факультет  КНиИТ";
			worksheet.Cells[$"N{row}:S{row}"].Merge = true;
			worksheet.Cells[$"A{++row}"].Value = "(нужное подчеркнуть)";
			worksheet.Cells[$"A{row}:I{row}"].Merge = true;

			// 1 семестр
			worksheet.Cells[$"A{++row}"].Value = "1 семестр";
			worksheet.Cells[$"A{row}:Z{row}"].Merge = true;
			worksheet.PrintTableHeader(false, ++row, faculty);

			row += 5;

			worksheet.Cells[$"A{++row}"].Value = faculty;

			int startRow = row;

			var firstSemesterSubjects = teacherSubjects.Select(x => x).Where(s => s.Semester % 2 == 1);
			worksheet.PrintC(budget, firstSemesterSubjects, ref row, faculty);
			
			worksheet.Cells[$"A{startRow}:A{row}"].Style.WrapText = true;
			worksheet.Cells[$"A{startRow}:A{row}"].Style.Font.Name = "Times New Roman";
			worksheet.Cells[$"A{startRow}:A{row}"].Style.Font.Size = 10;

			var teacherInfo = teacherGenerateInfo.FirstOrDefault(x => x.FullName == teacherSubjects.Key);
			if (teacherInfo == null)
			{
				teacherInfo = new TeacherGenerateModel { FullName = teacherSubjects.Key };
				teacherGenerateInfo.Add(teacherInfo);
			}
			if (budget)
				teacherInfo.FirstSemesterBudgetRow = row;
			else
				teacherInfo.FirstSemesterCommercialRow = row;

			// 2 семестр
			worksheet.Cells[$"A{++row}"].Value = "2 семестр";
			worksheet.Cells[$"A{row}:Z{row}"].Merge = true;
			worksheet.PrintTableHeader(false, ++row, faculty);

			row += 5;

			worksheet.Cells[$"A{++row}"].Value = faculty;

			startRow = row;

			var secondSemesterSubjects = teacherSubjects.Select(x => x).Where(s => s.Semester % 2 == 0);
			worksheet.PrintC(budget, secondSemesterSubjects, ref row, faculty);

			worksheet.Cells[$"A{startRow}:A{row}"].Style.WrapText = true;
			worksheet.Cells[$"A{startRow}:A{row}"].Style.Font.Name = "Times New Roman";
			worksheet.Cells[$"A{startRow}:A{row}"].Style.Font.Size = 10;

			if (budget)
				teacherInfo.SecondSemesterBudgetRow = row;
			else
				teacherInfo.SecondSemesterCommercialRow = row;

			row += 4;
		}
		return worksheet;
	}

	public static void PrintC(this ExcelWorksheet worksheet, bool budget, 
		IEnumerable<TeacherSubject> subjects, ref int row, string faculty)
	{
		int startRow = row;
		foreach (TeacherSubject subject in subjects)
		{
			var studentNumber = budget ? subject.Budget : subject.Commercial;

			if (studentNumber == null || studentNumber == 0)
				continue;

			if (subject.Specialization == null || subject.Semester == null || subject.Budget == null
			|| subject.Commercial == null || subject.Groups == null)
			{
				worksheet.Cells[$"A{row}"].Value = subject.Name;
				worksheet.Cells[$"Z{row}"].Value = subject.TotalHours;
			}
			double Total = 0;

			worksheet.Cells[$"A{row}"].Value = subject.Name;
			worksheet.Cells[$"B{row}"].Value = subject.Specialization;
			worksheet.Cells[$"C{row}"].Value = (subject.Semester % 2) + 1;
			worksheet.Cells[$"D{row}"].Value = subject.Semester;
			worksheet.Cells[$"E{row}"].Value = studentNumber;
			// количество потоков
			int countFlow = subject.Specialization.Split(",", StringSplitOptions.RemoveEmptyEntries).Length;
			worksheet.Cells[$"F{row}"].Value = countFlow;
			// количество групп
			int countGroups = subject.Groups.Split(",", StringSplitOptions.RemoveEmptyEntries).Length;
			worksheet.Cells[$"G{row}"].Value = countGroups;
			worksheet.Cells[$"H{row}"].Value = subject.SelfStudy;

			// РУКОВОДСТВО ПРАКТИКОЙ
			string nameLower = subject.Name.ToLower();
			if (nameLower.Contains("практика"))
			{
				if (nameLower.Contains("базовая") || nameLower.Contains("производственная"))
				{
					worksheet.Cells[$"P{row}"].Formula = $"2 * E{row}";
				}
				else if (nameLower.Contains("технологическая") || nameLower.Contains("вычислительная"))
				{
					worksheet.Cells[$"P{row}"].Formula = $"48 * G{row}";
				}
				else if (nameLower.Contains("педагогическая") || nameLower.Contains("научно"))
				{
					worksheet.Cells[$"P{row}"].Formula = $"8 * E{row}";
				}
			}
			else if (nameLower.Contains("консультация"))
			{
				worksheet.Cells[$"P{row}"].Formula = $"10 * E{row}";
			}

			else if (subject.Name.Contains("ВКР"))
			{
				// ВКР
				string[] groups = subject.Groups.Split(",", StringSplitOptions.RemoveEmptyEntries);
				if ((int.Parse(groups[0]) / 10) % 10 == 7)
				{
					// магистратура
					worksheet.Cells[$"R{row}"].Formula = $"34 * E{row}";
				}
				else if ((int.Parse(groups[0]) / 10) % 10 == 3)
				{
					// специалитет
					worksheet.Cells[$"R{row}"].Formula = $"30 * E{row}";
				}
				else
				{
					worksheet.Cells[$"R{row}"].Formula = $"24 * E{row}";
				}
			}
			else if (subject.Name.ToLower().Contains("курсовая работа"))
			{
				// КУРСОВЫЕ РАБОТЫ
				string[] note = subject.Remark.Split(" ", StringSplitOptions.RemoveEmptyEntries);
				worksheet.Cells[$"Q{row}"].Formula = $"{int.Parse(note[0])} * E{row}";
			}
			else if (subject.practice)
			{
				worksheet.Cells[$"J{row}"].Value = subject.Seminars;
				worksheet.Cells[$"K{row}"].Value = subject.Laboratory;
			}
			else
			{
				worksheet.Cells[$"I{row}"].Value = subject.Lectures;
				worksheet.Cells[$"J{row}"].Value = subject.Seminars;
				worksheet.Cells[$"K{row}"].Value = subject.Laboratory;

				if (subject.SelfStudy != null)
				{
					worksheet.Cells[$"L{row}"].Formula = $"=IF(ROUND(F{row}*H{row}*0.05,1)>2*F{row},2*F{row},ROUND(F{row}*H{row}*0.05,1))";
				}

				if (subject.ReportingForm == "зачет")
				{
					worksheet.Cells[$"O{row}"].Formula = $"=ROUND(E{row}/3,1)";
				}
				else if (subject.ReportingForm == "экзамен")
				{
					worksheet.Cells[$"M{row}"].Formula = $"2 * F{row}";
					worksheet.Cells[$"N{row}"].Formula = $"=ROUND(E{row}/2,1)";
				}

				// Проверка контрольных работ
				worksheet.Cells[$"T{row}"].Formula = $"=ROUND(E{row}/4,1)";

				// TODO: руководство аспирантами

			}
			worksheet.Cells[$"Z{row}"].Formula = $"=SUM(I{row}:Y{row})";
			row++;
		}
		worksheet.Cells[$"A{row}"].Value = "Итого по " + faculty;

		foreach (var cell in ClockCells)
		{
			worksheet.Cells[$"{cell}{row}"].Formula = $"=SUM({cell}{startRow}:{cell}{row - 1})";
		}

		// TODO: счетчик часов преподавателя для учета превышения нормы

		//totalHours.PrintToExcel(worksheet, row);
		//if (budget)
		//{
		//	teacherRate[teacherName].AmountHoursBudget.Add(totalHours);
		//}
		//else
		//{
		//	teacherRate[teacherName].AmountHoursCommercial.Add(totalHours);
		//}
	}

	public static ExcelWorksheet PrintToExcelO(this ExcelWorksheet worksheet,
		bool budget, List<TeacherGenerateModel> teacherGenerateInfo)
	{
		int row = 8;
		PrintTableHeaderO(worksheet, row);
		row += 5;
		int startRow = row;

		foreach(var teacherInfo in teacherGenerateInfo)
		{
			worksheet.Cells[$"A{row}"].Value = teacherInfo.FullName;
			worksheet.Cells[$"B{row}"].Value = "очн";

			for (int i = 0; i < 18; i++)
			{
				var firstSemester = budget ? teacherInfo.FirstSemesterBudgetRow : teacherInfo.FirstSemesterCommercialRow;
				var secondSemester = budget ? teacherInfo.SecondSemesterBudgetRow : teacherInfo.SecondSemesterCommercialRow;
				var worksheetName = budget ? "c1" : "c2";
				worksheet.Cells[$"{CalculationCellsInCWorksheet[i]}{row}"].Formula =
					$"={worksheetName}!{ClockCells[i]}{firstSemester}+{worksheetName}!{ClockCells[i]}{secondSemester}";
				worksheet.Cells[$"T{row}"].Formula = $"=SUM(C{row}:S{row})";
			}
			row++;
		}

		worksheet.Cells[$"A{row}"].Value = "Итого по кафедре";

		foreach (var cell in CalculationCellsInCWorksheet)
		{
			worksheet.Cells[$"{cell}{row}"].Formula = $"=SUM({cell}{startRow}:{cell}{row - 1})";
		}

		return worksheet;
	}

	public static ExcelWorksheet PrintTableHeaderO(this ExcelWorksheet worksheet, int startRow)
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
		worksheet.Cells[$"A{startRow}"].Value = "Фамилия, инициалы преподавателя, его должность";

		// Поворот текста на 90
		worksheet.Cells[$"C{startRow + 1}:S{startRow + 2}"].Style.TextRotation = 90;
		worksheet.Cells[$"B{startRow}"].Style.TextRotation = 90;

		// Слияние ячеек
		worksheet.Cells[$"B{startRow}:B{startRow + 2}"].Merge = true;

		worksheet.Cells[$"B{startRow}"].Value = "Форма обучения (очная, очно-заочная, заочная)";

		worksheet.Cells[$"C{startRow}"].Value = "Число часов по видам учебной работы";
		worksheet.Cells[$"C{startRow}:S{startRow}"].Merge = true;
		for (char c = 'C'; c <= 'Y'; c++)
		{
			worksheet.Cells[$"{c}{startRow + 1}:{c}{startRow + 2}"].Merge = true;
		}
		worksheet.Cells[$"C{startRow + 1}"].Value = "Лекции";
		worksheet.Cells[$"D{startRow + 1}"].Value = "Практ., семин. занятия";
		worksheet.Cells[$"E{startRow + 1}"].Value = "Лабор. занятия";
		worksheet.Cells[$"F{startRow + 1}"].Value = "Консультации по дисциплине, КСР";
		worksheet.Cells[$"G{startRow + 1}"].Value = "Консультации перед экзаменом";
		worksheet.Cells[$"H{startRow + 1}"].Value = "Экзамены";
		worksheet.Cells[$"I{startRow + 1}"].Value = "Зачеты";
		worksheet.Cells[$"J{startRow + 1}"].Value = "Руководство практикой";
		worksheet.Cells[$"K{startRow + 1}"].Value = "Курсовые работы";
		worksheet.Cells[$"L{startRow + 1}"].Value = "Выпускные квалиф. работы";
		worksheet.Cells[$"M{startRow + 1}"].Value = "Работа в ГАК";
		worksheet.Cells[$"N{startRow + 1}"].Value = "Проверка контр. работ";
		worksheet.Cells[$"O{startRow + 1}"].Value = "Руководство аспирантами";
		worksheet.Cells[$"P{startRow + 1}"].Value = "Руководство соискателями";
		worksheet.Cells[$"Q{startRow + 1}"].Value = "Руководство магис-терской программой";
		worksheet.Cells[$"R{startRow + 1}"].Value = "Факультативные занятия";

		worksheet.Cells[$"T{startRow}:T{startRow + 2}"].Merge = true;
		worksheet.Column(26).Width = 7.67;
		worksheet.Cells[$"T{startRow}"].Value = "Итого (часов)";
		int col = 1;
		for (char c = 'A'; c <= 'T'; c++)
		{
			worksheet.Cells[$"{c}{startRow + 3}"].Value = col;
			col++;
		}
		worksheet.Cells[$"A{startRow + 3}:T{startRow + 3}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
		worksheet.Cells[$"A{startRow + 3}:T{startRow + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

		var FirstTableRange = worksheet.Cells[$"A{startRow}:T{startRow + 3}"];
		FirstTableRange.Style.Border.Top.Style = ExcelBorderStyle.Medium;
		FirstTableRange.Style.Border.Left.Style = ExcelBorderStyle.Medium;
		FirstTableRange.Style.Border.Right.Style = ExcelBorderStyle.Medium;
		FirstTableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

		worksheet.Cells[$"A{startRow + 4}:T{startRow + 4}"].Merge = true;
		worksheet.Cells[$"A{startRow + 4}:T{startRow + 4}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
		worksheet.Cells[$"A{startRow + 4}:T{startRow + 4}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

		return worksheet;
	}
}
