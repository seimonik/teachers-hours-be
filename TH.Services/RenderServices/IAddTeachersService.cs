using TH.Services.Models;

namespace TH.Services.RenderServices;

public class AddTeachersServiceContext
{
    public AddTeachersServiceContext(Stream stream, IEnumerable<IEnumerable<TeacherStudents>> teachersFullNames)
    {
        Stream = stream;
        TeachersFullNames = teachersFullNames;
    }

    public Stream Stream { get; set; }
    public IEnumerable<IEnumerable<TeacherStudents>> TeachersFullNames { get; set; }
}

public interface IAddTeachersService : IService<AddTeachersServiceContext, byte[]>
{

}
