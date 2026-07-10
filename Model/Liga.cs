using System.ComponentModel.DataAnnotations;

namespace JogadoresApi.Model
{
    public class Liga
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da liga é obrigatório.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "O nome deve conter apenas letras.")]
        [MaxLength(40, ErrorMessage = "O nome deve ter no máximo 40 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        public ICollection<Time> Times { get; set; } = new List<Time>();

        public Liga() { }

        public Liga(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }
    }
}
