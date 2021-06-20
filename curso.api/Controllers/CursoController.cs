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
using curso.api.Business.Entities;

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
        public async Task<IActionResult> Post(CursoViewModelInput cursoViewModelInput)
        {
            try
            {
                Curso curso = new Curso
                {
                    Nome = cursoViewModelInput.Nome,
                    Descricao = cursoViewModelInput.Descricao
                };

                var codigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                curso.CodigoUsuario = codigoUsuario;
                _cursoRepository.Adicionar(curso);
                _cursoRepository.Commit();

                //var cursoViewModelOutput = new CursoViewModelOutput();


                var cursoViewModelOutput = new CursoViewModelOutput
                {
                    Nome = curso.Nome,
                    Descricao = curso.Descricao,
                };

                return Created("", cursoViewModelOutput);
            }
        
            catch (Exception ex)
            {
              //_logger.LogError(ex.ToString());
              return new StatusCodeResult(500);
             }
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
        public async Task<IActionResult> Get(CursoViewModelInput cursoViewModelInput)
        {
            var codigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var cursos = _cursoRepository.ObterPorUsuario(codigoUsuario)
                .Select(s => new CursoViewModelOutput()
                {
                    Nome = s.Nome,
                    Descricao = s.Descricao,
                    Login = s.Usuario.Login
                });
            
            return Ok(cursos);
        }
    }
}
