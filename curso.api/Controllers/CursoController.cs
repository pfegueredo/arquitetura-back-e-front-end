using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using curso.api.Models.Cursos;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using curso.api.Business.Repositories;
using Microsoft.Extensions.Configuration;
using curso.api.Configurations;

namespace curso.api.Controllers
{
    [Route("api/v1/cursos")]
    [ApiController]
    [Authorize]
    
    public class CursoController : ControllerBase
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authentication;
        public CursoController(
            ICursoRepository cursoRepository,
            IConfiguration configuration,
            IAuthenticationService authentication)
        {
            _cursoRepository = cursoRepository;
            _configuration = configuration;
            _authentication = authentication;
        }

        /// <summary>
        /// Este serviço permite cadastrar cursos para o usuário autenticado
        /// </summary>
        /// <returns>Retorna status 201 e dados do curso do usuário</returns>

        [SwaggerResponse(statusCode: 200, description: "Sucesso ao cadastrar um curso")]
        [SwaggerResponse(statusCode: 401, description: "Não autorizado")]

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post(CursosViewModelInput cursoViewModelInput)
        {
            var codigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            return Created("", cursoViewModelInput);
        }


        /// <summary>
        /// Este serviço permite obter os cursos cadastrado
        /// </summary>
        /// <returns>Retorna status ok e dados do curso do usuário</returns>
        /// 
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao obter os cursos")]
        [SwaggerResponse(statusCode: 401, description: "Não autorizado")]

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get(CursosViewModelInput cursoViewModelInput)
        {
            //Cria uma lista Fake
            var cursos = new List<CursoViewModelOutput>();

            var codigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            cursos.Add(new CursoViewModelOutput()
            {
                Login = codigoUsuario.ToString(),
                Descricao = "Teste",
                Nome = "Teste Nome"

            });
            return Ok(cursos);
        }
    }
}
