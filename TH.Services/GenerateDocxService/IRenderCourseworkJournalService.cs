namespace TH.Services.GenerateDocxService;

public class RenderCourseworkJournalContext
{
	public RenderCourseworkJournalContext(Dictionary<string, Dictionary<string, int>> courseworkJournal)
	{
		CourseworkJournal = courseworkJournal;
	}

	public Dictionary<string, Dictionary<string, int>> CourseworkJournal { get; set; }
}

public interface IRenderCourseworkJournalService : IService<RenderCourseworkJournalContext, byte[]>
{
}
