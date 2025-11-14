using System.Text.Json.Serialization;

namespace BookSpot.UI.DTOs;

public class AuthResponseDto
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;

    [JsonPropertyName("expiration")]
    public DateTime Expiration { get; set; }

    [JsonPropertyName("user")]
    public UserDto User { get; set; } = null!;
}
