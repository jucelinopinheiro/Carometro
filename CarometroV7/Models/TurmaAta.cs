namespace CarometroV7.Models
{
    public class TurmaAta
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int TurmaId { get; set; }
        public Turma? Turma { get; set; }
        public string? Descricao { get; set; }
        public string? UrlAnexo { get; set; }       
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

    }
}
