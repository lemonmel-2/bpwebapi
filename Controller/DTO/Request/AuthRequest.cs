namespace webapi.Controller.DTO.Request
{
    public class AuthRequest
    {
        public required string UserId { get; init; }

        public required string Password { get; init; }
    }
}