using CarometroV7.Data.Mapping;
using CarometroV7.Models;
using Microsoft.EntityFrameworkCore;

namespace CarometroV7.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options) { }
        public DbSet<Usuario>? Usuarios { get; set; }
        public DbSet<Curso>? Cursos { get; set; }
        public DbSet<Turma>? Turmas { get; set; }
        public DbSet<Aluno>? Alunos { get; set; }
        public DbSet<Matricula>? Matriculas { get; set; }
        public DbSet<Ocorrencia>? Ocorrencias { get; set; }
        public DbSet<AnexoOcorrencia>? AnexoOcorrencias { get; set; }
        public DbSet<TurmaAta>? TurmaAtas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new CursoMap());
            modelBuilder.ApplyConfiguration(new TurmaMap());
            modelBuilder.ApplyConfiguration(new AlunoMap());
            modelBuilder.ApplyConfiguration(new MatriculaMap());
            modelBuilder.ApplyConfiguration(new OcorrenciaMap());
            modelBuilder.ApplyConfiguration(new AnexoOcorrenciaMap());
            modelBuilder.ApplyConfiguration(new TurmaAtaMap());
        }

    }
}
