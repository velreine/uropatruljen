namespace CommonData.Model.DTO
{
    /// <summary>
    /// Represents a record/value-object that describes what the register endpoint returns.
    /// </summary>
    /// <param name="Message">Message to client.</param>
    /// <param name="Id">Id of created user.</param>
    public class RegisterResponseDTO
    {
        public string Message { get; }

        public int Id { get; }

        public RegisterResponseDTO(string message, int id)
        {
            Message = message;
            Id = id;
        }
    }
}