using System.Text.Json.Serialization;

namespace BookSpot.UI.DTOs;

public class UserDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("dataNascimento")]
    public DateTime? DataNascimento { get; set; }

    [JsonPropertyName("foto")]
    public string Foto { get; set; } = string.Empty;

    [JsonPropertyName("perfil")]
    public string Perfil { get; set; } = string.Empty;
}
