namespace CommonData.Model.DTO
{
    /// <summary>
    /// Represents a record/value-object that describes what the login endpoint returns.
    /// </summary>
    /// <param name="Message">Message to client.</param>
    /// <param name="Token">The Json Web Token the client should use for subsequent requests.</param>
    public class LoginResponseDTO
    {

        public string Message { get; }
        public string Token { get; }

        public LoginResponseDTO(string message, string token)
        {
            Message = message;
            Token = token;
        }

    }
}