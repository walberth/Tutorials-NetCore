namespace DemoCloudWatch.Business 
{
    using System;
    using Transversal;
    using System.Collections.Generic;

    public class PersonApplication : BaseClass, IPersonApplication {
        public IEnumerable<string> GetAllPerson()
        {
            try {
                var test = 10;
                var test1 = 0;
                var test2 = test / test1;
            } catch (Exception ex) {
                RegisterLogFatal(ex);
            }

            return null;
        }

        public object RegisterPerson(string value)
        {
            throw new System.NotImplementedException();
        }
    }
}
