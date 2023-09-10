using System.ComponentModel.DataAnnotations;

namespace ChawlaClinic.DAL.Entities
{
    public abstract class Base
    {
        [Key]
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
