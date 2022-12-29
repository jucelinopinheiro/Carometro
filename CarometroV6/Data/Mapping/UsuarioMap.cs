using CarometroV6.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarometroV6.Data.Mapping
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("INT");

            builder.Property(x => x.Nome)
                .IsRequired()
                .HasColumnName("Nome")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder.Property(x => x.Email)
               .HasColumnName("Email")
               .HasColumnType("VARCHAR")
               .HasMaxLength(80);

            builder.Property(x => x.SenhaHash)
                .IsRequired()
                .HasColumnName("SenhaHash")
                .HasColumnType("VARCHAR")
                .HasMaxLength(255);

            builder.Property(x => x.Perfil)
               .IsRequired()
               .HasColumnName("Perfil")
               .HasColumnType("TINYINT");

            builder.Property(x => x.Notificar)
                .IsRequired()
                .HasColumnName("Notificar")
                .HasColumnType("BIT");

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