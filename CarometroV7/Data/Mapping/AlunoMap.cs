using CarometroV7.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarometroV7.Data.Mapping
{
    public class AlunoMap : IEntityTypeConfiguration<Aluno>
    {
        public void Configure(EntityTypeBuilder<Aluno> builder)
        {
            builder.ToTable("Alunos");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
              .ValueGeneratedOnAdd()
              .UseIdentityColumn();

            builder.Property(x => x.Nome)
               .IsRequired()
               .HasColumnName("Nome")
               .HasColumnType("VARCHAR")
               .HasMaxLength(80);

            builder.Property(x => x.CelAluno)
             .HasColumnName("CelAluno")
             .HasColumnType("VARCHAR")
             .HasMaxLength(15);

            builder.Property(x => x.EmailAluno)
             .HasColumnName("EmailAluno")
             .HasColumnType("VARCHAR")
             .HasMaxLength(80);

            builder.Property(x => x.Nascimento)
             .IsRequired()
             .HasColumnName("Nascimento")
             .HasColumnType("DATE");

            builder.Property(x => x.Rg)
            .IsRequired()
            .HasColumnName("Rg")
            .HasColumnType("VARCHAR")
            .HasMaxLength(14);

            builder.Property(x => x.Cpf)
            .IsRequired()
            .HasColumnName("Cpf")
            .HasColumnType("VARCHAR")
            .HasMaxLength(14);

            builder.Property(x => x.Pai)
            .HasColumnName("Pai")
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

            builder.Property(x => x.CelPai)
              .HasColumnName("CelPai")
              .HasColumnType("VARCHAR")
              .HasMaxLength(15);

            builder.Property(x => x.Mae)
                .HasColumnName("Mae")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder.Property(x => x.CelMae)
              .HasColumnName("CelMae")
              .HasColumnType("VARCHAR")
              .HasMaxLength(15);

            builder.Property(x => x.Foto)
              .HasColumnName("Foto")
              .HasColumnType("VARCHAR")
              .HasMaxLength(255);

            builder.Property(x => x.Pne)
             .IsRequired()
             .HasColumnName("Pne")
             .HasColumnType("BIT")
             .HasDefaultValueSql("0");

            builder.Property(x => x.ObsAluno)
             .HasColumnName("ObsAluno")
             .HasColumnType("VARCHAR")
             .HasMaxLength(255);

            builder.Property(x => x.CreateAt)
             .IsRequired()
             .HasColumnName("CreateAt")
             .HasColumnType("DATE");

            builder.Property(x => x.UpdateAt)
             .HasColumnName("UpdateAt")
             .HasColumnType("DATE");

        }
    }
}
