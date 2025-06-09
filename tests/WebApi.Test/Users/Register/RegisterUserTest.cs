using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CashFlow.Execption;
using CommonTestsUtilities.Requests;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Register;

// O IClassFixture indica que essa é uma classe voltada para testes de integração
// O WebApplicationFactory é como se eu estivesse criando um servidor
// Por default a api executa em ambiente de desenvolvimento então ela consome os dados do appsettings development
public class RegisterUserTest(
    CustomWebApplicationFactory webApplicationFactory
    // WebApplicationFactory<Program> webApplicationFactory // versão default porém use o app.settings development
// ) : IClassFixture<WebApplicationFactory<Program>>
// ) : IClassFixture<CustomWebApplicationFactory>
) : CashFlowClassFixture(webApplicationFactory)
{
    private const string METHOD = "api/user";

    // private readonly HttpClient _httpClient = webApplicationFactory.CreateClient();

    [Fact]
    public async Task Success()
    {
        // var httpClient = new HttpClient();
        // httpClient.PostAsJsonAsync(METHOD, request);

        var request = RequestRegisterUserJsonBuilder.Build();

        // var result = await _httpClient.PostAsJsonAsync(METHOD, request);
        var result = await DoPost(requestUri: METHOD, request: request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        // Aqui o recomendado é utilizar o json document
        // porque caso eu fasso o parse da classe igual acontece na controller não é legal
        // A ideia aqui é simular um cliente, é agir como se fosse um aplicativo ou o postman
        // a validação é sobre as propriedades que existe na classe não sobre as funções de desearilizar
        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Theory]
    // [InlineData("pt-BR")]
    // [InlineData("pt-PT")]
    // [InlineData("en")]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string cultureInfo)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        // _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(cultureInfo));

        // var result = await _httpClient.PostAsJsonAsync(METHOD, request);
        var result = await DoPost(requestUri: METHOD, request: request, culture: cultureInfo);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(cultureInfo));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}