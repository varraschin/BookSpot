using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace BookSpot.API.Models;
    [Table("Usuario")]

public class Usuario : IdentityUser
{
    [Required]
    [StringLength(50)]
    public string Nome { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DataNascimento { get; set; }

    [StringLength(300)]
    public string Foto { get; set; }
}