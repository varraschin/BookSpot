using System.Text.Json.Serialization;

namespace BookSpot.UI.DTOs;

public class ProdutoDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("categoriaId")]
    public int CategoriaId { get; set; }

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("descricao")]
    public string Descricao { get; set; } = string.Empty;

    [JsonPropertyName("qtde")]
    public int Qtde { get; set; } = 0;

    [JsonPropertyName("valorCusto")]
    public decimal ValorCusto { get; set; } = 0;

    [JsonPropertyName("valorVenda")]
    public decimal ValorVenda { get; set; } = 0;

    [JsonPropertyName("destaque")]
    public bool Destaque { get; set; } = false;

    [JsonPropertyName("foto")]
    public string Foto { get; set; } = string.Empty;

    [JsonPropertyName("categoriaNome")]
    public string CategoriaNome { get; set; } = string.Empty;
}
