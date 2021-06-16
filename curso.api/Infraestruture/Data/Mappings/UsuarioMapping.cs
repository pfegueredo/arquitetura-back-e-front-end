using curso.api.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace curso.api.Infraestruture.Data.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {

        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            // Relacionar Usuario à tabela do banco
            builder.ToTable("TB_USUARIO"); //Nome da tabela a ser criada 
            builder.HasKey(p => p.Codigo); //Chave primária
            builder.Property(p => p.Codigo).ValueGeneratedOnAdd(); //Gera um Identity - Autoincremento
            builder.Property(p => p.Login);
            builder.Property(p => p.Senha);
            builder.Property(p => p.Email);

        }
    }
}
