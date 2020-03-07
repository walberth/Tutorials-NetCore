namespace EFCore_Demo.Controller 
{
    using Service.Interface;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("student")]
    public class StudentController : ControllerBase {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService) {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            return Ok(await _studentService.GetAll());
        }
    }
}
