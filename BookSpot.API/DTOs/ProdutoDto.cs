using System.ComponentModel.DataAnnotations;

namespace BookSpot.API.DTOs;

public class ProdutoDto
{
    public int Id { get; set; }
    public int CategoriaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Qtde { get; set; } = 0;
    public decimal ValorCusto { get; set; } = 0;
    public decimal ValorVenda { get; set; } = 0;
    public bool Destaque { get; set; } = false;
    public string Foto { get; set; }
    public string CategoriaNome { get; set; } // Para exibir o nome da categoria
}

public class ProdutoCreateDto
{
    [Required]
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(3000)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    [Range(0, int.MaxValue)]
    public int Qtde { get; set; } = 0;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal ValorCusto { get; set; } = 0;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal ValorVenda { get; set; } = 0;

    public bool Destaque { get; set; } = false;

    public IFormFile Foto { get; set; }
}

public class ProdutoUpdateDto
{
    public int Id { get; set; }

    [Required]
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(3000)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    [Range(0, int.MaxValue)]
    public int Qtde { get; set; } = 0;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal ValorCusto { get; set; } = 0;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal ValorVenda { get; set; } = 0;

    public bool Destaque { get; set; } = false;

    public IFormFile Foto { get; set; }
}
