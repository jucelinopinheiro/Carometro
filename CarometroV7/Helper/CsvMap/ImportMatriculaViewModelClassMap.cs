using CarometroV7.ViewModel.Matricula;
using CsvHelper.Configuration;

namespace CarometroV7.Helper.CsvMap
{
    public class ImportMatriculaViewModelClassMap:ClassMap<ImportMatriculaViewModel>
    {
        public ImportMatriculaViewModelClassMap()
        {  
            Map(r => r.MatriculaSgset).Name("Nº de Matrícula");
            Map(r => r.Nome).Name("Nome");
            Map(r => r.Rg).Name("RG");
            Map(r => r.Nascimento).Name("Data de Nascimento");
            Map(r => r.Pai).Name("Nome do Pai");
            Map(r => r.Mae).Name("Nome da Mãe");
            Map(r => r.DataMatricula).Name("Data de Matrícula");
            Map(r => r.CelAluno).Name("Celular");
            Map(r => r.EmailAluno).Name("e-mail");
            Map(r => r.Cpf).Name("CPF");
            Map(r => r.ObsAluno).Name("Portador de Necessidades Especiais");
            Map(r => r.TurmaSegset).Name("Turma");
        }
    }
}
