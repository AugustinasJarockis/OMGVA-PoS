namespace OMGVA_PoS.Helper_modules.Utilities
{
    public class LoginDTO(bool isSuccess, string message, string? token = null)
    {
        public bool IsSuccess { get; set; } = isSuccess;
        public string Message { get; set; } = message;
        public string? Token { get; set; } = token;
    }
}
