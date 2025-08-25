using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Users.Profile;

public class GetUserProfileUseCase(ILoggedUser loggedUser, IMapper mapper) : IGetUserProfileUseCase
{
    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await loggedUser.Get();

        return mapper.Map<ResponseUserProfileJson>(user);
    }
}