using CarometroV7.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CarometroV7.Data.Mapping
{
    public class OcorrenciaMap : IEntityTypeConfiguration<Ocorrencia>
    {
        public void Configure(EntityTypeBuilder<Ocorrencia> builder)
        {

            builder.ToTable("Ocorrencias");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
              .ValueGeneratedOnAdd()
              .UseIdentityColumn();

            builder.Property(x => x.Nome)
               .HasColumnName("Nome")
               .HasColumnType("VARCHAR")
               .HasMaxLength(20);

            builder.Property(x => x.Descricao)
              .HasColumnName("Descricao")
              .HasColumnType("TEXT");

            builder.Property(x => x.CreateAt)
                .IsRequired()
                .HasColumnName("CreateAt")
                .HasColumnType("DATE");

            builder.Property(x => x.UpdateAt)
                .HasColumnName("UpdateAt")
                .HasColumnType("DATE");

            builder.Property(x => x.UsuarioId)
                .IsRequired()
                .HasColumnName("UsuarioId")
                .HasColumnType("INT");

            builder.Property(x => x.AlunoId)
                .IsRequired()
                .HasColumnName("AlunoId")
                .HasColumnType("INT");

            builder.Property(x => x.TurmaId)
              .IsRequired()
              .HasColumnName("TurmaId")
              .HasColumnType("INT");

            builder.HasOne(x => x.Usuario)
                .WithMany(y => y.Ocorrencias)
                .HasForeignKey(x => x.UsuarioId)
                .HasConstraintName("FK_Ocorrencias_Usuarios");

            builder.HasOne(x => x.Aluno)
               .WithMany(y => y.Ocorrencias)
               .HasForeignKey(x => x.AlunoId)
               .HasConstraintName("FK_Ocorrencias_Alunos");

            builder.HasOne(x => x.Turma)
                .WithMany(y => y.Ocorrencias)
                .HasForeignKey(x => x.TurmaId)
                .HasConstraintName("FK_Ocorrencias_Turmas");

        }
    }
}
