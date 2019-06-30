using System;

namespace ApplicationLayer
{
    using System.Collections.Generic;
    using ApplicationLayerInterface;
    using RepositoryLayerInterface;

    public class PersonApplication : IPersonApplication {
        internal IPersonRepository _personRepository;

        public PersonApplication(IPersonRepository personRepository) {
            _personRepository = personRepository;
        }

        public string GetCompleteName(string firstName, string lastName) {
            if (string.IsNullOrEmpty(firstName)) {
                return $"No se ingreso el nombre";
            }

            if (string.IsNullOrEmpty(lastName)) {
                return $"No se ingreso apelido";
            }

            return $"{firstName} {lastName}";
        }

        public IEnumerable<string> PersonNames() {
            return _personRepository.PersonNames();
        }

        public string ConcatenateTwoNames(string firsPerson, string secondPerson) {
            var firstCompleteName = _personRepository.GetPersonCompleteName(firsPerson);
            var secondCompleteName = _personRepository.GetPersonCompleteName(secondPerson);

            return $"{firstCompleteName},{secondCompleteName}";
        }
    }
}
