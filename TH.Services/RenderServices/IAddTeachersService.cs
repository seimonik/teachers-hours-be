namespace TH.Services.RenderServices;

public class AddTeachersServiceContext
{
    public AddTeachersServiceContext(Stream stream, IEnumerable<string> teachersFullNames)
    {
        Stream = stream;
        TeachersFullNames = teachersFullNames;
    }

    public Stream Stream { get; set; }
    public IEnumerable<string> TeachersFullNames { get; set; }
}

public interface IAddTeachersService : IService<AddTeachersServiceContext, byte[]>
{

}
