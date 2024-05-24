using TH.Services.Models;

namespace TH.Services.GenerateDocxService;

public class RenderCourseworkJournalContext
{
	public RenderCourseworkJournalContext(Dictionary<string, IEnumerable<TeacherStudents>> courseworkJournal)
	{
		CourseworkJournal = courseworkJournal;
	}

	public Dictionary<string, IEnumerable<TeacherStudents>> CourseworkJournal { get; set; }
}

public interface IRenderCourseworkJournalService : IService<RenderCourseworkJournalContext, byte[]>
{
}
