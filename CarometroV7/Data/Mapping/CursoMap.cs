using CarometroV7.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarometroV7.Data.Mapping
{
    public class CursoMap : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.ToTable("Cursos");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
              .ValueGeneratedOnAdd()
              .UseIdentityColumn();

            builder.Property(x => x.Descricao)
              .IsRequired()
              .HasColumnName("Descricao")
              .HasColumnType("VARCHAR")
              .HasMaxLength(50);

            builder.Property(x => x.Tipo)
              .IsRequired()
              .HasColumnName("Tipo")
              .HasColumnType("TINYINT");

            builder.Property(x => x.Cor)
              .HasColumnName("Cor")
              .HasColumnType("VARCHAR")
              .HasMaxLength(7);

            builder.Property(x => x.Ativo)
              .IsRequired()
              .HasColumnName("Ativo")
              .HasColumnType("BIT")
              .HasDefaultValueSql("1");

            builder.Property(x => x.CreateAt)
                .IsRequired()
                .HasColumnName("CreateAt")
                .HasColumnType("DATETIME");

            builder.Property(x => x.UpdateAt)
                .HasColumnName("UpdateAt")
                .HasColumnType("DATETIME");
        }
    }
}
