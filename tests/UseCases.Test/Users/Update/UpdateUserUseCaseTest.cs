using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Domain.Entities;
using CommonTestsUtilities.LoggedUser;
using CommonTestsUtilities.Repositories;

namespace UseCases.Test.Users.Update;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        
    }

    private UpdateUserUseCase CreateUseCase(User user, string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var updateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
        var readRepository = new UserReadOnlyRepositoryBuilder();

        if (!string.IsNullOrWhiteSpace(email))
        {
            readRepository.ExistActiveUserWithEmail(email);
        }

        return new UpdateUserUseCase(loggedUser, readRepository.Build(), updateRepository, unitOfWork);
    }
}