using TH.Services.Models;

namespace TH.Services.ParsingSevice;

public class ParsingServiceContext
{
	public ParsingServiceContext(Stream stream)
	{
		Stream = stream;
	}

	public Stream Stream { get; set; }
}

public interface IParsingService : IService<ParsingServiceContext, IEnumerable<SubjectModel>>
{
}
