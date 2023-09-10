namespace ChawlaClinic.BL.DTOs.User
{
    public class AddUserDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
        public string Password1 { get; set; }
        public string Password2 { get; set; }
    }
}
