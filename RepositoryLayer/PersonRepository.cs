using System;

namespace RepositoryLayer
{
    using System.Collections.Generic;
    using RepositoryLayerInterface;

    public class PersonRepository : IPersonRepository
    {
        public IEnumerable<string> PersonNames() {
            var nombres = new List<string>();

            nombres.Add("Walberth");
            nombres.Add("Angela");
            nombres.Add("otro Nombre");
            nombres.Add("otro nombre más");

            return nombres;
        }
    }
}
