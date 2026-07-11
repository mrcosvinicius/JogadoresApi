namespace JogadoresApi.Dtos
{
    public class ReadTimeDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Estadio { get; set; } = string.Empty;
        public List<LigaResumoDto> Ligas { get; set; } = new();
        public List<JogadorResumoDto> Elenco { get; set; } = new();
    }
}
