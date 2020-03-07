namespace EFCore_Demo.Repository.Implementation
{
    using System;
    using Entity;
    using Context;
    using Interface;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public class StudentRepository : IStudentRepository {
        private readonly EFCoreContext _context;

        public StudentRepository(EFCoreContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAll() {
            throw new Exception("Ooops!");
            return await _context.Student
                                 .Include(x => x.ReferenceType)
                                 .Include(x => x.Person)
                                 .Include(x => x.Attorney)
                                 .Include(x => x.Attorney.Person)
                                 .Include(x => x.Attorney.RelationshipType)
                                 .ToListAsync();
        }
    }
}
