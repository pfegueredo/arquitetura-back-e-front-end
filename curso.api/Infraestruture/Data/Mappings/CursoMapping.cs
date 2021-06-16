using curso.api.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace curso.api.Infraestruture.Data.Mappings
{
    public class CursoMapping : IEntityTypeConfiguration<Curso> //Tipar do tipo IdentityFramework
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            // Relacionar a model Curso à tabela do banco
            builder.ToTable("TB_CURSO"); //Nome da tabela
            builder.HasKey(p => p.Codigo); //Chave primária
            builder.Property(p => p.Codigo).ValueGeneratedOnAdd(); //Gera um Identity - Autoincremento
            builder.Property(p => p.Nome);
            builder.Property(p => p.Descricao);
            builder.HasOne(p => p.Usuario).WithMany().HasForeignKey(fk => fk.CodigoUsuario); //Chave estrangeira
        }
    }
}
