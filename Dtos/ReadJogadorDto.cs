namespace JogadoresApi.Dtos
{
    public class ReadJogadorDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Posicao { get; set; } = string.Empty;
        public int? Gols { get; set; }
        public int? TimeId { get; set; }
        public string? Time { get; set; }
    }
}
