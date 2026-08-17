namespace Identity.Application.Models;

public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Role = "Customer"
);

public record LoginRequest(
    string Email,
    string Password
);

public record AuthResponse(
    string UserId,
    string Email,
    string Token
);