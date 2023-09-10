namespace ChawlaClinic.DAL.Entities
{
    public class User : Base
    {
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
