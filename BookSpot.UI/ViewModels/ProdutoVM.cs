using System.ComponentModel.DataAnnotations;

namespace BookSpot.UI.ViewModels;

public class ProdutoVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatória")]
    [Display(Name = "Categoria")]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    [Display(Name = "Nome do Produto")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(3000, ErrorMessage = "Máximo 3000 caracteres")]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A quantidade é obrigatória")]
    [Range(0, int.MaxValue, ErrorMessage = "A quantidade não pode ser negativa")]
    [Display(Name = "Quantidade em Estoque")]
    public int Qtde { get; set; } = 0;

    [Required(ErrorMessage = "O valor de custo é obrigatório")]
    [Range(0, double.MaxValue, ErrorMessage = "O valor de custo não pode ser negativo")]
    [Display(Name = "Valor de Custo")]
    public decimal ValorCusto { get; set; } = 0;

    [Required(ErrorMessage = "O valor de venda é obrigatório")]
    [Range(0, double.MaxValue, ErrorMessage = "O valor de venda não pode ser negativo")]
    [Display(Name = "Valor de Venda")]
    public decimal ValorVenda { get; set; } = 0;

    [Display(Name = "Produto em Destaque")]
    public bool Destaque { get; set; } = false;

    [Display(Name = "Foto do Produto")]
    public IFormFile Foto { get; set; }

    public string FotoUrl { get; set; }
    public string CategoriaNome { get; set; }
}
