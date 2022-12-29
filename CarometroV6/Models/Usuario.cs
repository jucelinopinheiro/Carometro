namespace CarometroV6.Models
{
    public class Usuario
    {
        public Usuario()
        {
            Ocorrencias = new List<Ocorrencia>();
        }
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? SenhaHash { get; set; }
        public byte Perfil { get; set; }
        public bool Notificar { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public List<Ocorrencia> Ocorrencias { get; set; }

    }
}