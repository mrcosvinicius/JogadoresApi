using System.ComponentModel.DataAnnotations;

namespace JogadoresApi.Dtos
{
    public abstract class TimeBaseDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "O nome deve conter apenas letras.")]
        [MaxLength(40, ErrorMessage = "O nome deve ter no máximo 40 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O estádio é obrigatório.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "O estádio deve conter apenas letras.")]
        [MaxLength(40, ErrorMessage = "O estádio deve ter no máximo 40 caracteres.")]
        public string Estadio { get; set; } = string.Empty;

        public List<int>? LigasIds { get; set; }
    }

    public class CriarTimeDto : TimeBaseDto { }

    public class AtualizarTimeDto : TimeBaseDto { }
}
