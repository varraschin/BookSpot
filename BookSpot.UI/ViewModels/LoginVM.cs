using System.ComponentModel.DataAnnotations;

namespace BookSpot.UI.ViewModels;

public class LoginVM
{
    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "O E-mail é Obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail válido")]
    public string Email { get; set; }

    [Display(Name = "Senha")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "A Senha é Obrigatória")]
    public string Senha { get; set; }
}

