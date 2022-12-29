namespace CarometroV6.Models
{
    public class AnexoOcorrencia
    {
        public int Id { get; set; }
        public int OcorrenciaId { get; set; }
        public Ocorrencia? Ocorrencia { get; set; }
        public string? Descricao { get; set; }
        public string? UrlAnexo { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}