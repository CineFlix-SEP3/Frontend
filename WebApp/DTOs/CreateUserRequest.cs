public class CreateUserRequest
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string HashedPassword { get; set; }
    public required string UserRole { get; set; }
}