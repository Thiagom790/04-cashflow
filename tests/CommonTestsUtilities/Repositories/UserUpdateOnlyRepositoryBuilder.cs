using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Moq;

namespace CommonTestsUtilities.Repositories;

public static class UserUpdateOnlyRepositoryBuilder
{
    public static IUserUpdateOnlyRepository Build(User user)
    {
        var repository = new Mock<IUserUpdateOnlyRepository>();

        repository.Setup(userRepo => userRepo.GetById(user.Id)).ReturnsAsync(user);

        return repository.Object;
    }
}