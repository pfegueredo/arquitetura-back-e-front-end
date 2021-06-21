using AutoBogus;
using curso.api.Models.Cursos;
using curso.api.Models.Usuarios;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace curso.api.test.Integrations.Controllers
{
    public class CursoControllerTests: UsuarioControllerTests
    {
        //private readonly WebApplicationFactory<Startup> _factory;
        //private readonly ITestOutputHelper _output;
        private readonly HttpClient _httpClient;
        //protected RegistroViewModelInput RegistroViewModelInput;

        public CursoControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output): base(factory, output)
        {
            
        }


        [Fact]
        public async Task Registrar_InformandoDadosDeUmCursoValidoEUmUsuarioAutenticado_DeveRetornarSucesso()
        {
            // Arrange
            var cursoViewModelInput = new AutoFaker<CursoViewModelInput>();
            var cursoViewModelInputFaker = cursoViewModelInput.Generate();

            // Criação da StringContent que será usada no Post.
            StringContent content = new StringContent(JsonConvert.SerializeObject(cursoViewModelInput.Generate()), Encoding.UTF8, "application/json");


            //Act (Actions)
            //Rota que será testada:
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginViewModelOutput.Token);
            var httpClientRequest = _httpClient.PostAsync("api/v1/cursos", content).GetAwaiter().GetResult();

            //Assert
            _output.WriteLine($"{nameof(CursoControllerTests)} : {nameof(Registrar_InformandoDadosDeUmCursoValidoEUmUsuarioAutenticado_DeveRetornarSucesso)} : { await httpClientRequest.Content.ReadAsStringAsync()}");
            
            Assert.Equal(System.Net.HttpStatusCode.Created, httpClientRequest.StatusCode);
        }

        [Fact]
        public async Task Registrar_InformandoDadosDeUmCursoValidoEmUsuarioNaoAutenticado_DeveRetornarSucesso()
        {
            // Arrange
            var cursoViewModelInput = new AutoFaker<CursoViewModelInput>();
            var cursoViewModelInputFaker = cursoViewModelInput.Generate();

            // Criação da StringContent que será usada no Post.
            StringContent content = new StringContent(JsonConvert.SerializeObject(cursoViewModelInput.Generate()), Encoding.UTF8, "application/json");


            //Act (Actions)
            //Rota que será testada:
            var httpClientRequest = _httpClient.PostAsync("api/v1/cursos", content).GetAwaiter().GetResult();

            //Assert
            _output.WriteLine($"{nameof(CursoControllerTests)} : {nameof(Registrar_InformandoDadosDeUmCursoValidoEmUsuarioNaoAutenticado_DeveRetornarSucesso)} : { await httpClientRequest.Content.ReadAsStringAsync()}");

            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, httpClientRequest.StatusCode);
        }


        [Fact]
        public async Task Obter_InformandoUmUsuarioAutenticado_DeveRetornarSucesso()
        {
            // Arrange
            await Registrar_InformandoDadosDeUmCursoValidoEUmUsuarioAutenticado_DeveRetornarSucesso();
            //Act (Actions)
            //Rota que será testada:
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginViewModelOutput.Token);
            var httpClientRequest = _httpClient.GetAsync("api/v1/cursos").GetAwaiter().GetResult();

            //Assert
            _output.WriteLine($"{nameof(CursoControllerTests)} : {nameof(Registrar_InformandoDadosDeUmCursoValidoEUmUsuarioAutenticado_DeveRetornarSucesso)} : { await httpClientRequest.Content.ReadAsStringAsync()}");

            var cursos = JsonConvert.DeserializeObject<IList<CursoViewModelOutput>>(await httpClientRequest.Content.ReadAsStringAsync());

            Assert.NotNull(cursos);

            Assert.Equal(System.Net.HttpStatusCode.Created, httpClientRequest.StatusCode);
        }

    }
}
