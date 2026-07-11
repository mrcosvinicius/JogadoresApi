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
        public DbSet<Time> Times { get; set; }
        public DbSet<Liga> Ligas { get; set; }
    }
}
