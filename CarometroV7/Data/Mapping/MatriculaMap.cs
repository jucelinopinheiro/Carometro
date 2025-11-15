using CarometroV7.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarometroV7.Data.Mapping
{
    public class MatriculaMap : IEntityTypeConfiguration<Matricula>
    {
        public void Configure(EntityTypeBuilder<Matricula> builder)
        {
            builder.ToTable("Matriculas");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
              .ValueGeneratedOnAdd()
              .UseIdentityColumn();

            builder.Property(x => x.MatriculaSgset)
                .IsRequired()
                .HasColumnName("MatriculaSgset")
                .HasColumnType("INT");

            builder.Property(x => x.DataMatricula)
                .HasColumnName("DataMatricula")
                .HasColumnType("DATE");

            builder.Property(x => x.AlunoId)
                .IsRequired()
                .HasColumnName("AlunoId")
                .HasColumnType("INT");

            builder.Property(x => x.TurmaId)
                .IsRequired()
                .HasColumnName("TurmaId")
                .HasColumnType("INT");

            builder.Property(x => x.Status)
                .HasColumnName("Status")
                .HasColumnType("TINYINT");

            builder.Property(x => x.CreateAt)
                .IsRequired()
                .HasColumnName("CreateAt")
                .HasColumnType("DATE");

            builder.Property(x => x.UpdateAt)
             .HasColumnName("UpdateAt")
             .HasColumnType("DATE");

            builder.HasOne(x => x.Aluno)
             .WithMany(y => y.Matriculas)
             .HasForeignKey(x => x.AlunoId)
             .HasConstraintName("FK_Matriculas_Alunos");

            builder.HasOne(x => x.Turma)
             .WithMany(y => y.Matriculas)
             .HasForeignKey(x => x.TurmaId)
             .HasConstraintName("FK_Matriculas_Turmas");
        }
    }
}
