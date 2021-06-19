using curso.api.Models.Usuarios;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Xunit;
using System.Text;
using Xunit.Abstractions;
using System.Threading.Tasks;

namespace curso.api.test.Integrations.Controllers
{
    public class UsuarioControllerTests: IClassFixture<WebApplicationFactory<Startup>>
    {
        // Definindo configuração inicial
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _httpClient;

        //Instanciando o Startup do projeto da API:
        public UsuarioControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
            // Configurar como um cliente Http (tipo um mobile):
            _httpClient = _factory.CreateClient();
        }


        [Fact]
        public void Registrar_InformandoUsuarioESenha_DeveRetornarSucesso()
        {
            // Arrange
            var registroViewModelInput = new RegistroViewModelInput
            {
                Login = "Paulo Fegueredo",
                Email = "paulo.fegueredeo@gmail.com",
                Senha = "123456"
            };

            // Criação da StringContent que será usada no Post.
            StringContent content = new StringContent(JsonConvert.SerializeObject(registroViewModelInput), Encoding.UTF8, "application/json");


            //Act (Actions)
            //Rota que será testada:
            var httpClientRequest = _httpClient.PostAsync("api/v1/usuario/registrar", content).GetAwaiter().GetResult();

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, httpClientRequest.StatusCode);
        }


        [Fact]
        public async Task Logar_InformandoUsuarioESenhaExistentes_DeveRetornarSucesso()
        {
            // Arrange
            var loginViewModelInput = new LoginViewModelInput
            {
                Login = "Paulo Fegueredo",
                Senha = "123456"
            };

            // Criação da StringContent que será usada no Post.
            StringContent content = new StringContent(JsonConvert.SerializeObject(loginViewModelInput), Encoding.UTF8, "application/json");


            //Act (Actions)
            //Rota que será testada:
            var httpClientRequest = await _httpClient.PostAsync("api/v1/usuario/logar", content);

            var loginViewModelOutput = JsonConvert.DeserializeObject<LoginViewModelOutput>(await httpClientRequest.Content.ReadAsStringAsync());
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, httpClientRequest.StatusCode);
            Assert.NotNull(loginViewModelOutput.Token);
            Assert.Equal(loginViewModelInput.Login, loginViewModelOutput.Usuario.Login);
            _output.WriteLine(loginViewModelOutput.Token);
        }
    }
}
