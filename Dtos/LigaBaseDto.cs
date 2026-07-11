using System.ComponentModel.DataAnnotations;

namespace JogadoresApi.Dtos
{
    public abstract class LigaBaseDto
    {
        [Required(ErrorMessage = "O nome da liga é obrigatório.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "O nome deve conter apenas letras.")]
        [MaxLength(40, ErrorMessage = "O nome deve ter no máximo 40 caracteres.")]
        public string Nome { get; set; } = string.Empty;
    }

    public class CriarLigaDto : LigaBaseDto { }

    public class AtualizarLigaDto : LigaBaseDto { }
}
