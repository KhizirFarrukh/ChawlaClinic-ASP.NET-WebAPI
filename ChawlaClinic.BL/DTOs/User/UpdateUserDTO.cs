namespace ChawlaClinic.BL.DTOs.User
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
    }
}
