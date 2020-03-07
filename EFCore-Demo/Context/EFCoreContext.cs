namespace EFCore_Demo.Context
{
    using Entity;
    using Microsoft.EntityFrameworkCore;

    public class EFCoreContext : DbContext {
        public EFCoreContext() {
        }

        public EFCoreContext(DbContextOptions options) : base(options) {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<Attorney> Attorney { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Direction> Direction { get; set; }
        public DbSet<District> District { get; set; }
        public DbSet<DocumentType> DocumentType { get; set; }
        public DbSet<GradeInstructionType> GradeInstructionType { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<PersonType> PersonType { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<ReferenceType> ReferenceType { get; set; }
        public DbSet<RelationshipType> RelationshipType { get; set; }
        public DbSet<Student> Student { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // PERSON
            modelBuilder.Entity<Person>()
                    .HasOne(p => p.DocumentType)
                    .WithOne(p => p.Person)
                    .HasForeignKey<Person>(p => p.IdDocumentType);

            modelBuilder.Entity<Person>()
                    .HasOne(p => p.PersonType)
                    .WithOne(p => p.Person)
                    .HasForeignKey<Person>(p => p.IdPersonType);

            // STUDENT
            modelBuilder.Entity<Student>()
                    .HasOne(p => p.Person)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(p => p.IdPerson);

            modelBuilder.Entity<Student>()
                    .HasOne(p => p.Attorney)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(p => p.IdAttorney);

            modelBuilder.Entity<Student>()
                    .HasOne(p => p.ReferenceType)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(p => p.IdReferenceType);

            // DIRECTION
            modelBuilder.Entity<Direction>()
                    .HasOne(p => p.Person)
                    .WithOne(p => p.Direction)
                    .HasForeignKey<Direction>(p => p.IdPerson);

            modelBuilder.Entity<Direction>()
                    .HasOne(p => p.Department)
                    .WithOne(p => p.Direction)
                    .HasForeignKey<Direction>(p => p.IdDepartment);

            modelBuilder.Entity<Direction>()
                    .HasOne(p => p.Province)
                    .WithOne(p => p.Direction)
                    .HasForeignKey<Direction>(p => p.IdProvince);

            modelBuilder.Entity<Direction>()
                    .HasOne(p => p.District)
                    .WithOne(p => p.Direction)
                    .HasForeignKey<Direction>(p => p.IdDistrict);

            // PROVINCE
            modelBuilder.Entity<Province>()
                    .HasOne(p => p.Department)
                    .WithOne(p => p.Province)
                    .HasForeignKey<Province>(p => p.IdDepartment);

            // DISTRICT
            modelBuilder.Entity<District>()
                    .HasOne(p => p.Province)
                    .WithOne(p => p.District)
                    .HasForeignKey<District>(p => p.IdProvince);

            // ATTORNEY
            modelBuilder.Entity<Attorney>()
                    .HasOne(p => p.Person)
                    .WithOne(p => p.Attorney)
                    .HasForeignKey<Attorney>(p => p.IdPerson);

            modelBuilder.Entity<Attorney>()
                    .HasOne(p => p.GradeInstructionType)
                    .WithOne(p => p.Attorney)
                    .HasForeignKey<Attorney>(p => p.IdGradeInstructionType);

            modelBuilder.Entity<Attorney>()
                    .HasOne(p => p.RelationshipType)
                    .WithOne(p => p.Attorney)
                    .HasForeignKey<Attorney>(p => p.IdRelationshipType);
        }
    }
}
