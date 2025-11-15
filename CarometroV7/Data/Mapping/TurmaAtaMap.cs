using CarometroV7.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarometroV7.Data.Mapping
{
    public class TurmaAtaMap : IEntityTypeConfiguration<TurmaAta>
    {
        public void Configure(EntityTypeBuilder<TurmaAta> builder)
        {
            builder.ToTable("TurmaAtas");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
              .ValueGeneratedOnAdd()
              .UseIdentityColumn();

            builder.Property(x => x.UsuarioId)
                .IsRequired()
                .HasColumnName("UsuarioId")
                .HasColumnType("INT");

            builder.Property(x => x.TurmaId)
             .IsRequired()
             .HasColumnName("TurmaId")
             .HasColumnType("INT");

            builder.Property(x => x.Descricao)
              .HasColumnName("Descricao")
              .HasColumnType("VARCHAR")
              .HasMaxLength(50);

            builder.Property(x => x.UrlAnexo)
               .IsRequired()
               .HasColumnName("UrlAnexo")
               .HasColumnType("VARCHAR")
               .HasMaxLength(255);

            builder.Property(x => x.CreateAt)
               .IsRequired()
               .HasColumnName("CreateAt")
               .HasColumnType("DATE");

            builder.Property(x => x.UpdateAt)
                .HasColumnName("UpdateAt")
                .HasColumnType("DATE");

            builder.HasOne(x => x.Turma)
               .WithMany(y => y.TurmaAtas)
               .HasForeignKey(x => x.TurmaId)
               .HasConstraintName("FK_Atas_Turmas");

            builder.HasOne(x => x.Usuario)
               .WithMany(y => y.TurmaAtas)
               .HasForeignKey(x => x.UsuarioId)
               .HasConstraintName("FK_Atas_Usuarios");
        }
    }
}
