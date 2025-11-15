using CarometroV7.Enum;

namespace CarometroV7.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? SenhaHash { get; set; }
        public byte Perfil { get; set; }

        public string NomePerfil
        {
            get
            {
                EPerfil perfil = (EPerfil)Perfil;
                return perfil.ToString();
            }
        }   
       
        public bool Notificar { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public virtual List<Ocorrencia>? Ocorrencias { get; set;}
        public virtual List<TurmaAta>? TurmaAtas { get; set; }
    }

   
}
