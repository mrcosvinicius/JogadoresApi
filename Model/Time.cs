using System.ComponentModel.DataAnnotations;

namespace JogadoresApi.Model
{
    public class Time
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "O nome deve conter apenas letras.")]
        [MaxLength(40, ErrorMessage = "O nome deve ter no máximo 40 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O estádio é obrigatório.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "O estádio deve conter apenas letras.")]
        [MaxLength(40, ErrorMessage = "O estádio deve ter no máximo 40 caracteres.")]
        public string Estadio { get; set; } = string.Empty;

        public ICollection<Liga> Ligas { get; set; } = new List<Liga>();

        public ICollection<Jogador> Elenco { get; set; } = new List<Jogador>();

        public Time() { }

        public Time(int id, string nome, string estadio)
        {
            Id = id;
            Nome = nome;
            Estadio = estadio;
        }
    }
}
