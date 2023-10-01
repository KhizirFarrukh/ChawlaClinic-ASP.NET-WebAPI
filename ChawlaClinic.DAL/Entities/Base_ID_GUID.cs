using System.ComponentModel.DataAnnotations;

namespace ChawlaClinic.DAL.Entities
{
    public class Base_ID_GUID : Base
    {
        [Key]
        public new Guid Id { get; set; } = Guid.NewGuid();
    }
}
