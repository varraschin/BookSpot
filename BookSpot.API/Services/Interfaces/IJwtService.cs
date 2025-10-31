using BookSpot.API.DTOs;

namespace BookSpot.API.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(UserDto user);
}
