using System.ComponentModel.DataAnnotations;

namespace CarometroV7.ViewModel.Aluno
{
    public class CreateAlunoViewModel
    {
        [Required(ErrorMessage = "Digite o nome do Aluno")]
        [StringLength(80, ErrorMessage = "Nome com no máximo 80 caracteres")]
        [Display(Name = "Nome")]
        public string? Nome { get; set; }

        [StringLength(15, ErrorMessage = "Formato Cel (11) 98899-2200")]
        [Display(Name = "Cel. Aluno")]
        public string? CelAluno { get; set; }

        [StringLength(80, ErrorMessage = "Nome com no máximo 80 caracteres")]
        [Display(Name = "E-mail")]
        public string? EmailAluno { get; set; }

        [Required(ErrorMessage = "Data de nascimento")]
        [Display(Name = "Nascimento")]
        public DateTime Nascimento { get; set; }

        [Required(ErrorMessage = "Digite o RG do Aluno")]
        [StringLength(14, ErrorMessage = "Nome com no máximo 14 caracteres")]
        [Display(Name = "RG do Aluno")]
        public string? Rg { get; set; }

        [Required(ErrorMessage = "Digite o CPF do Aluno")]
        [StringLength(14, ErrorMessage = "Nome com no máximo 14 caracteres")]
        [Display(Name = "CPF do Aluno")]
        public string? Cpf { get; set; }

        [StringLength(80, ErrorMessage = "Nome com no máximo 80 caracteres")]
        [Display(Name = "Nome Pai")]
        public string? Pai { get; set; }

        [StringLength(15, ErrorMessage = "Formato Cel (11) 98899-2200")]
        [Display(Name = "Cel. Pai")]
        public string? CelPai { get; set; }

        [StringLength(80, ErrorMessage = "Nome com no máximo 80 caracteres")]
        [Display(Name = "Nome Mãe")]
        public string? Mae { get; set; }

        [StringLength(15, ErrorMessage = "Formato Cel (11) 98899-2200")]
        [Display(Name = "Cel. Mãe")]
        public string? CelMae { get; set; }

        [Display(Name = "PNE")]
        public bool Pne { get; set; }

        [Display(Name = "Observalções do Aluno")]
        public string? ObsAluno { get; set; }
        public IFormFile? Arquivo { get; set; }
    }
}
