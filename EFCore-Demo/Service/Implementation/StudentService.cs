namespace EFCore_Demo.Service.Implementation
{
    using Model;
    using Interface;
    using AutoMapper;
    using Transversal;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using EFCore_Demo.Repository.Interface;

    public class StudentService : IStudentService {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepository, IMapper mapper) {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<StudentDto>>> GetAll() {
            var response = new Response<IEnumerable<StudentDto>>();
            var studentList = await _studentRepository.GetAll();

            response.Data = _mapper.Map<IEnumerable<StudentDto>>(studentList);

            return response;
        }
    }
}
