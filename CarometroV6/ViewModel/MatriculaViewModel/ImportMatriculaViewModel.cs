using CsvHelper.Configuration;

namespace CarometroV6.ViewModel.MatriculaViewModel
{

    public class ImportMatriculaViewModelClassMap : ClassMap<ImportMatriculaViewModel>
    {
        public ImportMatriculaViewModelClassMap()
        {
            Map(r => r.Nome).Name("Nome");
            Map(r => r.CelAluno).Name("Celular");
            Map(r => r.EmailAluno).Name("e-mail");
            Map(r => r.Nascimento).Name("Data de Nascimento");
            Map(r => r.Rg).Name("RG");
            Map(r => r.Cpf).Name("CPF");
            Map(r => r.Pai).Name("Nome do Pai");
            Map(r => r.Mae).Name("Nome da Mãe");
            Map(r => r.ObsAluno).Name("Portador de Necessidades Especiais");
            Map(r => r.MatriculaSgset).Name("Nº de Matrícula");
            Map(r => r.DataMatricula).Name("Data de Matrícula");
            Map(r => r.TurmaSegset).Name("Turma");
        }
    }

    public class ImportMatriculaViewModel
    {

        public string? Nome { get; set; }
        public string? CelAluno { get; set; }
        public string? EmailAluno { get; set; }
        public string? Nascimento { get; set; }
        public string? Rg { get; set; }
        public string? Cpf { get; set; }
        public string? Pai { get; set; }
        public string? Mae { get; set; }
        public string? ObsAluno { get; set; }
        public int MatriculaSgset { get; set; }
        public string? DataMatricula { get; set; }
        public string? TurmaSegset { get; set; }
    }
}
