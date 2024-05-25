using TH.Services.Models;

namespace TH.Services.ParsingSevice;

public class ValidateServiceContext
{
	public ValidateServiceContext(Stream stream)
	{
		Stream = stream;
	}

	public Stream Stream { get; set; }
}

public interface IValidateService : IService<ValidateServiceContext, byte[]?>
{
}
