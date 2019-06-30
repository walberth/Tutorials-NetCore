using System;

namespace ApplicationLayerInterface
{
    using System.Collections.Generic;

    public interface IPersonApplication {
        string GetCompleteName(string firstName, string lastName);
        IEnumerable<string> PersonNames();
        string ConcatenateTwoNames(string firsPerson, string secondPerson);
    }
}
