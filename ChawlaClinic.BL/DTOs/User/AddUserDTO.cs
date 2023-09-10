namespace ChawlaClinic.BL.DTOs.User
{
    public class AddUserDTO
    {
        public string Name { get; set; }
        public string? Email { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
        public string Password { get; set; }
    }
}
