using System.ComponentModel.DataAnnotations;

namespace CarometroV7.ViewModel.Turma
{
    public class CreateTurmaAtaViewModel
    {
        [Required(ErrorMessage = "Consulte o Id do Aluno no sistema")]
        [Display(Name = "TurmaId")]
        public int TurmaId { get; set; }

        [Required(ErrorMessage = "Descrição da ATA")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        public IFormFile Anexo { get; set; }
    }
}
