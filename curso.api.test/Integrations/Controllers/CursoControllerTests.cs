using AutoBogus;
using curso.api.Models.Cursos;
using curso.api.Models.Usuarios;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
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
        public async Task Registrar_InformandoDadosDeUmCursoValido_DeveRetornarSucesso()
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
            _output.WriteLine($"{nameof(CursoControllerTests)} : {nameof(Registrar_InformandoDadosDeUmCursoValido_DeveRetornarSucesso)} : { await httpClientRequest.Content.ReadAsStringAsync()}");
            Assert.Equal(System.Net.HttpStatusCode.Created, httpClientRequest.StatusCode);
        }

    }
}
