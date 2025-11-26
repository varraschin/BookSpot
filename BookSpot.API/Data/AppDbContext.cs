using BookSpot.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookSpot.API.Data;

public class AppDbContext : IdentityDbContext<Usuario>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        SeedUsuarioPadrao(builder);
        SeedCategoriaPadrao(builder);
        SeedProdutoPadrao(builder);
    }

    private static void SeedUsuarioPadrao(ModelBuilder builder)
    {
        #region Populate Roles - Perfis de Usuário
        List<IdentityRole> roles =
        [
            new IdentityRole() {
               Id = "0b44ca04-f6b0-4a8f-a953-1f2330d30894",
               Name = "Administrador",
               NormalizedName = "ADMINISTRADOR"
            },
            new IdentityRole() {
               Id = "ddf093a6-6cb5-4ff7-9a64-83da34aee005",
               Name = "Cliente",
               NormalizedName = "CLIENTE"
            },
        ];
        builder.Entity<IdentityRole>().HasData(roles);
        #endregion

        #region Populate Usuário
        List<Usuario> usuarios = [
            new Usuario(){
                Id = "ddf093a6-6cb5-4ff7-9a64-83da34aee005",
                Email = "gallojunior@gmail.com",
                NormalizedEmail = "GALLOJUNIOR@GMAIL.COM",
                UserName = "gallouunior@gmail.com",
                NormalizedUserName = "GALLOJUNIOR@GMAIL.COM",
                LockoutEnabled = true,
                EmailConfirmed = true,
                Nome = "José Antonio Gallo Junior",
                DataNascimento = DateTime.Parse("05/08/1981"),
                Foto = "/img/usuarios/avatar.png"
            }
        ];
        foreach (var user in usuarios)
        {
            PasswordHasher<Usuario> pass = new();
            user.PasswordHash = pass.HashPassword(user, "123456");
        }
        builder.Entity<Usuario>().HasData(usuarios);
        #endregion

        #region Populate UserRole - Usuário com Perfil
        List<IdentityUserRole<string>> userRoles =
        [
            new IdentityUserRole<string>() {
                UserId = usuarios[0].Id,
                RoleId = roles[0].Id
            }
        ];
        builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        #endregion
    }

    private static void SeedCategoriaPadrao(ModelBuilder builder)
    {
        List<Categoria> categorias = new()
        {
            new Categoria { Id = 1, Nome = "Clássicos" },
            new Categoria { Id = 2, Nome = "Contos" },
            new Categoria { Id = 3, Nome = "Fantasia" },
            new Categoria { Id = 4, Nome = "Romance" },
            new Categoria { Id = 5, Nome = "Scifi" },
            new Categoria { Id = 6, Nome = "Terror" }
        };
        builder.Entity<Categoria>().HasData(categorias);
    }

    private static void SeedProdutoPadrao(ModelBuilder builder)
{
    List<Produto> produtos = new()
    {
        // Categoria: Clássicos (Id = 1)
        new Produto { Id = 1, CategoriaId = 1, Nome = "Capitães da Areia", Descricao = "Jorge Amado", ValorCusto = 28.00m, ValorVenda = 49.90m, Qtde = 45, Destaque = true, Foto = "/img/produtos/1.jpg" },
        new Produto { Id = 2, CategoriaId = 1, Nome = "1984", Descricao = "George Orwell", ValorCusto = 30.00m, ValorVenda = 59.90m, Qtde = 40, Destaque = true, Foto = "/img/produtos/2.jpg" },
        new Produto { Id = 3, CategoriaId = 1, Nome = "Os Miseráveis", Descricao = "Victor Hugo", ValorCusto = 45.00m, ValorVenda = 89.90m, Qtde = 30, Foto = "/img/produtos/3.jpg" },
        new Produto { Id = 4, CategoriaId = 1, Nome = "Memórias Póstumas de Brás Cubas", Descricao = "Machado de Assis", ValorCusto = 25.00m, ValorVenda = 49.90m, Qtde = 50, Foto = "/img/produtos/4.jpg" },
        new Produto { Id = 5, CategoriaId = 1, Nome = "A Metamorfose", Descricao = "Franz Kafka", ValorCusto = 20.00m, ValorVenda = 39.90m, Qtde = 60, Foto = "/img/produtos/5.jpg" },

        // Categoria: Contos (Id = 2) 
        new Produto { Id = 6, CategoriaId = 2, Nome = "Chapeuzinho Vermelho", Descricao = "Conto de fadas clássico", ValorCusto = 15.00m, ValorVenda = 29.90m, Qtde = 120, Destaque = true, Foto = "/img/produtos/6.jpg" },
        new Produto { Id = 7, CategoriaId = 2, Nome = "Três Porquinhos", Descricao = "Conto de fadas clássico", ValorCusto = 15.00m, ValorVenda = 29.90m, Qtde = 100, Foto = "/img/produtos/7.png" },
        new Produto { Id = 8, CategoriaId = 2, Nome = "João e o Pé de Feijão", Descricao = "Conto de fadas clássico", ValorCusto = 18.00m, ValorVenda = 34.90m, Qtde = 80, Foto = "/img/produtos/8.jpg" },
        new Produto { Id = 9, CategoriaId = 2, Nome = "João e Maria", Descricao = "Conto de fadas clássico", ValorCusto = 18.00m, ValorVenda = 34.90m, Qtde = 90, Foto = "/img/produtos/9.jpg" },
        new Produto { Id = 10, CategoriaId = 2, Nome = "O Patinho Feio", Descricao = "Hans Christian Andersen", ValorCusto = 15.00m, ValorVenda = 29.90m, Qtde = 110, Foto = "/img/produtos/10.png" },

        // Categoria: Fantasia (Id = 3)
        new Produto { Id = 11, CategoriaId = 3, Nome = "Harry Potter e a Pedra Filosofal", Descricao = "J.K. Rowling", ValorCusto = 35.00m, ValorVenda = 69.90m, Qtde = 55, Destaque = true, Foto = "/img/produtos/11.jpg" },
        new Produto { Id = 12, CategoriaId = 3, Nome = "O Feiticeiro de Terramar", Descricao = "Ursula K. Le Guin", ValorCusto = 40.00m, ValorVenda = 79.90m, Qtde = 35, Foto = "/img/produtos/12.jpg" },
        new Produto { Id = 13, CategoriaId = 3, Nome = "A Sociedade do Anel", Descricao = "J.R.R. Tolkien", ValorCusto = 45.00m, ValorVenda = 89.90m, Qtde = 30, Foto = "/img/produtos/13.jpg" },
        new Produto { Id = 14, CategoriaId = 3, Nome = "A Guerra dos Tronos", Descricao = "George R.R. Martin", ValorCusto = 50.00m, ValorVenda = 99.90m, Qtde = 25, Foto = "/img/produtos/14.jpg" },
        new Produto { Id = 15, CategoriaId = 3, Nome = "Alice no País das Maravilhas", Descricao = "Lewis Carroll", ValorCusto = 32.00m, ValorVenda = 64.90m, Qtde = 40, Foto = "/img/produtos/15.jpg" },

        // Categoria: Romance (Id = 4) 
        new Produto { Id = 16, CategoriaId = 4, Nome = "Agora e Para Sempre, Lara Jean", Descricao = "Jenny Han", ValorCusto = 28.00m, ValorVenda = 54.90m, Qtde = 50, Destaque = true, Foto = "/img/produtos/16.jpg" },
        new Produto { Id = 17, CategoriaId = 4, Nome = "A Culpa é das Estrelas", Descricao = "John Green", ValorCusto = 25.00m, ValorVenda = 49.90m, Qtde = 60, Foto = "/img/produtos/17.png" },
        new Produto { Id = 18, CategoriaId = 4, Nome = "Quem É Você, Alasca?", Descricao = "John Green", ValorCusto = 26.00m, ValorVenda = 51.90m, Qtde = 55, Foto = "/img/produtos/18.png" },
        new Produto { Id = 19, CategoriaId = 4, Nome = "O Morro dos Ventos Uivantes", Descricao = "Emily Brontë", ValorCusto = 29.00m, ValorVenda = 55.90m, Qtde = 45, Foto = "/img/produtos/19.png" },
        new Produto { Id = 20, CategoriaId = 4, Nome = "Romeu e Julieta", Descricao = "William Shakespeare", ValorCusto = 30.00m, ValorVenda = 59.90m, Qtde = 40, Foto = "/img/produtos/20.png" },

        // Categoria: Scifi (Ficção Científica) (Id = 5) 
        new Produto { Id = 21, CategoriaId = 5, Nome = "Duna", Descricao = "Frank Herbert", ValorCusto = 55.00m, ValorVenda = 109.90m, Qtde = 28, Destaque = true, Foto = "/img/produtos/21.jpg" },
        new Produto { Id = 22, CategoriaId = 5, Nome = "Fúria Vermelha", Descricao = "Pierce Brown", ValorCusto = 40.00m, ValorVenda = 79.90m, Qtde = 32, Foto = "/img/produtos/22.jpg" },
        new Produto { Id = 23, CategoriaId = 5, Nome = "Eu, Robô", Descricao = "Isaac Asimov", ValorCusto = 35.00m, ValorVenda = 69.90m, Qtde = 38, Foto = "/img/produtos/23.jpg" },
        new Produto { Id = 24, CategoriaId = 5, Nome = "Blade Runner", Descricao = "Philip K. Dick", ValorCusto = 38.00m, ValorVenda = 75.90m, Qtde = 30, Foto = "/img/produtos/24.jpg" },
        new Produto { Id = 25, CategoriaId = 5, Nome = "Fundação", Descricao = "Isaac Asimov", ValorCusto = 48.00m, ValorVenda = 95.90m, Qtde = 25, Foto = "/img/produtos/25.png" },

        // Categoria: Terror (Id = 6) 
        new Produto { Id = 26, CategoriaId = 6, Nome = "It: A Coisa", Descricao = "Stephen King", ValorCusto = 60.00m, ValorVenda = 119.90m, Qtde = 20, Destaque = true, Foto = "/img/produtos/26.jpg" },
        new Produto { Id = 27, CategoriaId = 6, Nome = "O Chamado de Cthulhu e Outros Contos", Descricao = "H.P. Lovecraft", ValorCusto = 35.00m, ValorVenda = 69.90m, Qtde = 35, Foto = "/img/produtos/27.jpg" },
        new Produto { Id = 28, CategoriaId = 6, Nome = "Drácula", Descricao = "Bram Stoker", ValorCusto = 32.00m, ValorVenda = 64.90m, Qtde = 40, Foto = "/img/produtos/28.jpg" },
        new Produto { Id = 29, CategoriaId = 6, Nome = "Frankenstein", Descricao = "Mary Shelley", ValorCusto = 38.00m, ValorVenda = 75.90m, Qtde = 30, Foto = "/img/produtos/29.png" },
        new Produto { Id = 30, CategoriaId = 6, Nome = "O Gato Preto e Outros Contos de Terror", Descricao = "Edgar Allan Poe", ValorCusto = 25.00m, ValorVenda = 49.90m, Qtde = 45, Foto = "/img/produtos/30.jpg" }
    };
    builder.Entity<Produto>().HasData(produtos);
}


}
