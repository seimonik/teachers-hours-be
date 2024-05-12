using TH.Services.Models;

namespace TH.Services.ParsingSevice;

public class ParsingServiceContext
{
	public ParsingServiceContext(Stream stream, int endRow)
	{
		Stream = stream;
		EndRow = endRow;
	}

	public Stream Stream { get; set; }
	public int EndRow { get; set; }
}

public interface IParsingService : IService<ParsingServiceContext, IEnumerable<SubjectModel>>
{
}
