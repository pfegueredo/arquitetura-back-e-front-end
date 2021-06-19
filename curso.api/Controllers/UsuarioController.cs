using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Configurations;
using curso.api.Filters;
using curso.api.Infraestruture.Data;
using curso.api.Models;
using curso.api.Models.Usuarios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Controllers
{
    [Route("api/v1/usuario")] //personalizar o nome da rota
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authentication;
        public UsuarioController(  //Alterar para o nome da classe
            IUsuarioRepository usuarioRepository, 
            IConfiguration configuration,
            IAuthenticationService authentication)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
            _authentication = authentication;
        }

        /// <summary>
        /// Este serviço permite autenticar um usuário cadastrado e ativo
        /// </summary>
        /// <param name="loginViewModelInput">View model do login</param>
        /// <returns>Retorna status ok, dados do usuário e o token de segurança</returns>
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao autenticar", Type = typeof(LoginViewModelInput))]
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios", Type = typeof(ValidaCampoViewModelOutput))]
        [SwaggerResponse(statusCode: 500, description: "Erro interno", Type = typeof(ErroGenericoViewModel))]

        [HttpPost]
        [Route("logar")]
        [ValidacaoModelStateCustomizado] //valida formularios baseado na classe Filters/ValidacaoModel...
        public async Task<IActionResult> Logar(LoginViewModelInput loginViewModelInput)
        {
            var usuario = _usuarioRepository.ObterUsuarioAsync(loginViewModelInput.Login);

            if (usuario == null)
            {
                return BadRequest("Houve um erro ao tentar acessar. Usuário em branco.");
            }

            var usuarioViewModelOutput = new UsuarioViewModelOutput()
            {
                Codigo = 1,
                Login = "paulofegueredo",
                Email = "paulofegueredo@gmail.com"
            };

            return Ok(loginViewModelInput);
        }
        
        [HttpPost]
        [Route("registrar")]
        [ValidacaoModelStateCustomizado]
        public IActionResult Registrar(RegistroViewModelInput loginViewModelInput)
        {
            //var optionsBuilder = new DbContextOptionsBuilder<CursoDbContext>();
            //optionsBuilder.UseSqlServer("Server=192.168.0.22;Database=CURSO;user=sa;password=akarajalho");
            
            //CursoDbContext contexto = new CursoDbContext(optionsBuilder.Options); //Instanciar o BD através do Contexto

            //Verifica se tem migrações pendentes e as retoma
            //var migracoesPendentes = contexto.Database.GetPendingMigrations();
            //if (migracoesPendentes.Count() > 0)
            //{
            //    contexto.Database.Migrate();
            //}
            
            var usuario = new Usuario();
            
            usuario.Login = loginViewModelInput.Login;
            usuario.Senha = loginViewModelInput.Senha;
            usuario.Email = loginViewModelInput.Email;

            
            _usuarioRepository.Adicionar(usuario);
            _usuarioRepository.Commit();

            return Created("", loginViewModelInput);
        }
    }
}
