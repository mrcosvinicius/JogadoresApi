namespace JogadoresApi.Dtos
{
    public class ReadLigaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public List<TimeResumoDto> Times { get; set; } = new();
    }
}
