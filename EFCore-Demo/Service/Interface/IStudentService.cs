namespace EFCore_Demo.Service.Interface
{
    using Model;
    using Transversal;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public interface IStudentService {
        Task<Response<IEnumerable<StudentDto>>> GetAll();
    }
}
