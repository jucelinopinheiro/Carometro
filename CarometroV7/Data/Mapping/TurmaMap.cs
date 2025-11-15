using CarometroV7.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarometroV7.Data.Mapping
{
    public class TurmaMap : IEntityTypeConfiguration<Turma>
    {
        public void Configure(EntityTypeBuilder<Turma> builder)
        {
            builder.ToTable("Turmas");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
              .ValueGeneratedOnAdd()
              .UseIdentityColumn();

            builder.Property(x => x.Descricao)
              .IsRequired()
              .HasColumnName("Descricao")
              .HasColumnType("VARCHAR")
              .HasMaxLength(80);

            builder.Property(x => x.Sigla)
             .IsRequired()
             .HasColumnName("Sigla")
             .HasColumnType("VARCHAR")
             .HasMaxLength(50);

            builder.Property(x => x.TurmaSgset)
             .IsRequired()
             .HasColumnName("TurmaSgset")
             .HasColumnType("VARCHAR")
             .HasMaxLength(20);

            builder.Property(x => x.Classroom)
            .HasColumnName("Classroom")
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

            builder.Property(x => x.DataInicio)
                .IsRequired()
                .HasColumnName("DataInicio")
                .HasColumnType("DATE");

            builder.Property(x => x.DataFim)
               .IsRequired()
               .HasColumnName("DataFim")
               .HasColumnType("DATE");

            builder.Property(x => x.Ativo)
              .IsRequired()
              .HasColumnName("Ativo")
              .HasColumnType("BIT")
              .HasDefaultValueSql("1");

            builder.Property(x => x.Bag)
             .HasColumnName("Bag")
             .HasColumnType("VARCHAR")
             .HasMaxLength(50);

            builder.Property(x => x.CreateAt)
                .IsRequired()
                .HasColumnName("CreateAt")
                .HasColumnType("DATETIME");

            builder.Property(x => x.UpdateAt)
                .HasColumnName("UpdateAt")
                .HasColumnType("DATETIME");

            builder.Property(x => x.CursoId)
                .IsRequired()
                .HasColumnName("CursoId")
                .HasColumnType("INT");

            builder.HasOne(x => x.Curso)
                .WithMany(y => y.Turmas)
                .HasForeignKey(x => x.CursoId)
                .HasConstraintName("FK_Turmas_Cursos");
        }
    }
}
