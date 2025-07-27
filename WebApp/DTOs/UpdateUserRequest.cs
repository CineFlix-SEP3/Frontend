namespace Frontend.DTOs;

public class UpdateUserRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public string UserRole { get; set; }
}