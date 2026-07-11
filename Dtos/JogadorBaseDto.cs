using System.ComponentModel.DataAnnotations;

namespace JogadoresApi.Dtos
{
    public abstract class JogadorBaseDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "O nome deve conter apenas letras.")]
        [StringLength(30, ErrorMessage = "O nome deve ter no máximo 30 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A posição é obrigatória.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "A posição deve conter apenas letras.")]
        [StringLength(30, ErrorMessage = "A posição deve ter no máximo 30 caracteres.")]
        public string Posicao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O número de gols é obrigatório.")]
        [Range(0, int.MaxValue, ErrorMessage = "O número de gols deve ser maior ou igual a 0.")]
        public int? Gols { get; set; }

        [Required(ErrorMessage = "O time é obrigatório.")]
        public int? TimeId { get; set; }

        public string? Time { get; set; }
    }

    public class CriarJogadorDto : JogadorBaseDto { }

    public class AtualizarJogadorDto : JogadorBaseDto { }
}
