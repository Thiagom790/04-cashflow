using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Moq;

namespace CommonTestsUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository = new();

    public void ExistActiveUserWithEmail(string email)
    {
        // dessa forma seria para toros os emails
        // _repository.Setup(userRepo => userRepo.ExistActiveUserWithEmail(It.IsAny<string>())).ReturnsAsync(true);
        //Essa função só devolve true quando chamada com o email especifico que passei
        _repository.Setup(userRepo => userRepo.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public UserReadOnlyRepositoryBuilder GetUserByEmail(User user)
    {
        _repository.Setup(userRepo => userRepo.GetUserByEmail(user.Email)).ReturnsAsync(user);

        return this;
    }

    public IUserReadOnlyRepository Build() => _repository.Object;
}