namespace EFCore_Demo.Entity
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("document_type")]
    public class DocumentType {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Person Person { get; set; }
    }
}
