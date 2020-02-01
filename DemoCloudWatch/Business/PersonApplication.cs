namespace DemoCloudWatch.Business 
{
    using System;
    using Transversal;
    using System.Collections.Generic;

    public class PersonApplication : BaseClass, IPersonApplication {
        public Response<IEnumerable<string>> GetAllPerson() {
            var response = new Response<IEnumerable<string>>();
            var identifier = Guid.NewGuid();

            try {
                var test = 10;
                var test1 = 0;
                var test2 = test / test1;
            } catch (Exception ex) {
                RegisterLogFatal(ex, identifier);

                response.IsSuccess = false;
                response.IsWarning = false;
                response.Message = $"Ocurrio un error inesperado {identifier}";
            }

            return response;
        }

        public Response<object> RegisterPerson(string value) {
            var response = new Response<object>();
            var identifier = Guid.NewGuid();

            try {
                var test = 10;
                var test1 = 0;
                var test2 = test / test1;
            } catch (Exception ex) {
                RegisterLogFatal(ex, identifier);

                response.IsSuccess = false;
                response.IsWarning = false;
                response.Message = $"Ocurrio un error inesperado {identifier}";
            }

            return response;
        }
    }
}
