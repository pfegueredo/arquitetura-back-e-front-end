using curso.api.Models.Usuarios;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Xunit;
using System.Text;
using Xunit.Abstractions;
using System.Threading.Tasks;
using AutoBogus;

namespace curso.api.test.Integrations.Controllers
{
    public class UsuarioControllerTests: IClassFixture<WebApplicationFactory<Startup>>, IAsyncLifetime
    {
        // Definindo configuração inicial
        private readonly WebApplicationFactory<Startup> _factory;
        protected readonly ITestOutputHelper _output;
        protected readonly HttpClient _httpClient;
        protected RegistroViewModelInput RegistroViewModelInput;
        protected LoginViewModelOutput LoginViewModelOutput;

        //Instanciando o Startup do projeto da API:
        public UsuarioControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
            // Configurar como um cliente Http (tipo um mobile):
            _httpClient = _factory.CreateClient();
        }


        [Fact]
        public async Task Registrar_InformandoUsuarioESenha_DeveRetornarSucesso()
        {
            // Arrange
            RegistroViewModelInput = new AutoFaker<RegistroViewModelInput>()
                                            .RuleFor(p => p.Email, faker => faker.Person.Email);
            
            
            //RegistroViewModelInput = new RegistroViewModelInput
            //{
            //    Login = "Paulo Fegueredo",
            //    Email = "paulo.fegueredeo@gmail.com",
            //    Senha = "123456"
            //};

            // Criação da StringContent que será usada no Post.
            StringContent content = new StringContent(JsonConvert.SerializeObject(RegistroViewModelInput), Encoding.UTF8, "application/json");


            //Act (Actions)
            //Rota que será testada:
            var httpClientRequest = _httpClient.PostAsync("api/v1/usuario/registrar", content).GetAwaiter().GetResult();

            //Assert
            _output.WriteLine(await httpClientRequest.Content.ReadAsStringAsync());
            Assert.Equal(System.Net.HttpStatusCode.Created, httpClientRequest.StatusCode);
        }


        [Fact]
        public async Task Logar_InformandoUsuarioESenhaExistentes_DeveRetornarSucesso()
        {
            // Arrange
            var loginViewModelInput = new LoginViewModelInput
            {
                Login = RegistroViewModelInput.Login,
                Senha = RegistroViewModelInput.Senha
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

        public async Task InitializeAsync()
        {
            await Registrar_InformandoUsuarioESenha_DeveRetornarSucesso();
            await Logar_InformandoUsuarioESenhaExistentes_DeveRetornarSucesso();
        }

        public async Task DisposeAsync()
        {
            _httpClient.Dispose();
        }
    }
}
