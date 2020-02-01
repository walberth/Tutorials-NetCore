namespace DemoCloudWatch.Business
{
    using Transversal;
    using System.Collections.Generic;

    public interface IPersonApplication {
        Response<IEnumerable<string>> GetAllPerson();
        Response<object> RegisterPerson(string value);
    }
}
