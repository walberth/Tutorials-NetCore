namespace DemoCloudWatch.Controller
{
    using System;
    using Business;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/person")]
    public class PersonController : ControllerBase {
        private IPersonApplication _personApplication;

        public PersonController(IPersonApplication personApplication) {
            _personApplication = personApplication ?? throw new ArgumentNullException(nameof(personApplication));
        }

        [HttpGet]
        public ActionResult Get() {
            return Ok(_personApplication.GetAllPerson());
        }

        [HttpPost]
        public ActionResult RegisterPerson() {
            return Ok(_personApplication.RegisterPerson(""));
        }
    }
}