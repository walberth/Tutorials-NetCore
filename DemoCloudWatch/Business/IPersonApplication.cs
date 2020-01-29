namespace DemoCloudWatch.Business
{
    using System.Collections.Generic;

    public interface IPersonApplication {
        IEnumerable<string> GetAllPerson();
        object RegisterPerson(string value);
    }
}
