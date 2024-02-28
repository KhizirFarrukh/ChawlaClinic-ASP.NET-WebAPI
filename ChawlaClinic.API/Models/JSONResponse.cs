namespace ChawlaClinic.API.Models
{
    public class JSONResponse
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; } = null!;
        public object Data { get; set; }
        public string ErrorMessage { get; set; } = null!;
        public string ErrorDescription { get; set; } = null!;
    }
}
