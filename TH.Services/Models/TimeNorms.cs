namespace TH.Services.Models;

public class TimeNorms
{
    // производственная практика
    public double ProductionPractice { get; set; }
	// учебная практика (НИР) 2 курс 3 семестр
	public double EducationalPractice23 { get; set; }
	// учебная практика (НИР) 3 курс 5 семестр
	public double EducationalPractice35 { get; set; }
	// учебная практика (НИР) 2 курс 4 семестр
	public double EducationalPractice24 { get; set; }
	// учебная практика (НИР) 3 курс 6 семестр
	public double EducationalPractice36 { get; set; }
	// учебная практика (НИР) 4 курс 7 семестр
	public double EducationalPractice47 { get; set; }
	// Норма часов по курсовым работам (2 курс)
	public double Coursework2 { get; set; }
    // Норма часов по курсовым работам (3 курс)
    public double Coursework3 { get; set; }
    // Норма часов по курсовым работам для ПО (2 курс)
    public double Coursework2PO { get; set; }
    // Норма часов по курсовым работам для ПО (3 курс)
    public double Coursework3PO { get; set; }
    // ВКР (бакалавриат)
    public double FinalQualifyingWorkBachelor { get; set; }
    // ВКР (магистратура)
    public double FinalQualifyingWorkMagistracy { get; set; }
}
