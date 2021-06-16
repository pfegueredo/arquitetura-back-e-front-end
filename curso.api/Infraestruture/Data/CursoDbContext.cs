using curso.api.Business.Entities;
using curso.api.Infraestruture.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Infraestruture.Data
{
    public class CursoDbContext: DbContext //Herdando do DBContext
    {
        public CursoDbContext(DbContextOptions<CursoDbContext> options) : base(options) //Instanciar do tipo DbContextOptions
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder = modelBuilder.ApplyConfiguration(new CursoMapping()); //Informar qual a classe de mapeamento
            modelBuilder = modelBuilder.ApplyConfiguration(new UsuarioMapping()); //Informar qual a classe de mapeamento
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Curso> Curso { get; set; }

    }
}
