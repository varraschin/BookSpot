using System.ComponentModel.DataAnnotations;

namespace BookSpot.UI.ViewModels;

public class RegistroVM
{
    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "O E-mail é Obrigatório")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido")]
    public string Email { get; set; }

    [Display(Name = "Senha")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "A Senha é Obrigatória")]
    public string Senha { get; set; }

    [StringLength(50)]
    [Display(Name = "Nome Completo")]
    [Required(ErrorMessage = "O Nome Completo é Obrigatório")]
    public string Nome { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Data de Nascimento")]
    public DateTime? DataNascimento { get; set; }

    public IFormFile Foto { get; set; }
}