using System.ComponentModel.DataAnnotations;

namespace JogadoresApi.Model
{
    public class Jogador
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "O nome deve conter apenas letras.")]
        [MaxLength(30, ErrorMessage = "O nome deve ter no máximo 30 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A posição é obrigatória.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "A posição deve conter apenas letras.")]
        [MaxLength(30, ErrorMessage = "A posição deve ter no máximo 30 caracteres.")]
        public string Posicao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O número de gols é obrigatório.")]
        [Range(0, int.MaxValue, ErrorMessage = "O número de gols deve ser maior ou igual a 0.")]
        public int? Gols { get; set; }

        public int? TimeId { get; set; }

        public Time? Time { get; set; }

        public Jogador() { }

        public Jogador(int id, string nome, string posicao, int? gols, int? timeId, Time? time)
        {
            Id = id;
            Nome = nome;
            Posicao = posicao;
            Gols = gols;
            TimeId = timeId;
            Time = time;
        }
    }
}
