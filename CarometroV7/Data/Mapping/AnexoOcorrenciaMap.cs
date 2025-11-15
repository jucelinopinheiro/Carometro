using CarometroV7.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CarometroV7.Data.Mapping
{
    public class AnexoOcorrenciaMap : IEntityTypeConfiguration<AnexoOcorrencia>
    {
        public void Configure(EntityTypeBuilder<AnexoOcorrencia> builder)
        {
            builder.ToTable("Anexos");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
              .ValueGeneratedOnAdd()
              .UseIdentityColumn();

            builder.Property(x => x.Descricao)
               .HasColumnName("Descricao")
               .HasColumnType("VARCHAR")
               .HasMaxLength(20);

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

            builder.Property(x => x.OcorrenciaId)
                .IsRequired()
                .HasColumnName("OcorrenciaId")
                .HasColumnType("INT");

            builder.HasOne(x => x.Ocorrencia)
                .WithMany(y => y.Anexos)
                .HasForeignKey(x => x.OcorrenciaId)
                .HasConstraintName("FK_Anexos_Ocorrencias");
        }
    }
}
