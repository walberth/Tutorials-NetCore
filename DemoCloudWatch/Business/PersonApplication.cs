namespace DemoCloudWatch.Business 
{
    using Transversal;
    using System.Collections.Generic;

    public class PersonApplication : BaseClass, IPersonApplication {
        public IEnumerable<string> GetAllPerson()
        {
            RegisterLogError();
            throw new System.NotImplementedException();
        }

        public object RegisterPerson(string value)
        {
            throw new System.NotImplementedException();
        }
    }
}
