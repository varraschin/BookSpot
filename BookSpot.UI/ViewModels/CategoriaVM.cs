using System.ComponentModel.DataAnnotations;

namespace BookSpot.UI.ViewModels;

public class CategoriaVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
    [Display(Name = "Nome")]
    public string Nome { get; set; } = string.Empty;

    [Display(Name = "Foto")]
    public IFormFile Foto { get; set; }

    public string FotoUrl { get; set; }

    [StringLength(26, ErrorMessage = "Máximo 26 caracteres")]
    [Display(Name = "Cor")]
    public string Cor { get; set; } = "rgba(0,0,0,1)";
}