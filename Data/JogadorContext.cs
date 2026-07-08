using Microsoft.EntityFrameworkCore;
using JogadoresApi.Model;

namespace JogadoresApi.Data
{
    public class JogadorContext : DbContext
    {
        public JogadorContext(DbContextOptions<JogadorContext> opcoes) : base(opcoes)
        {

        }
        public DbSet<Jogador> Jogadores { get; set; }
    }
}
