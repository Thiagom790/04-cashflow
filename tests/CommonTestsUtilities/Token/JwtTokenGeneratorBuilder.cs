using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Moq;

namespace CommonTestsUtilities.Token;

public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();

        // essa função lambda é para dizer qual a função do access token quero mockar os dados
        //O It.IsAny significa que vai ser para qualquer parâmetro que passo aquele retorno
        mock.Setup(accessTokenGenerator => accessTokenGenerator.Generate(It.IsAny<User>())).Returns("token");

        return mock.Object;
    }
}