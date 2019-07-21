using System;

namespace RepositoryLayerInterface
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IPersonRepository {
        IEnumerable<string> PersonNames();
        string GetPersonCompleteName(string name);
    }
}
