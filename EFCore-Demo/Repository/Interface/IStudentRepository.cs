namespace EFCore_Demo.Repository.Interface 
{
    using Entity;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStudentRepository {
        Task<IEnumerable<Student>> GetAll();
    }
}
