using System.ComponentModel.DataAnnotations;

namespace JogadoresApi.Dtos
{
    public class AtualizarLigaDto
    {
        [Required(ErrorMessage = "O nome da liga é obrigatório.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "O nome deve conter apenas letras.")]
        [MaxLength(40, ErrorMessage = "O nome deve ter no máximo 40 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        public AtualizarLigaDto() { }

        public AtualizarLigaDto(string nome)
        {
            Nome = nome;
        }
    }
}
