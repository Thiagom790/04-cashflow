using CashFlow.Domain.Entities;

namespace WebApi.Test.Resources;

public class UserIdentityManager(User user, string password, string token)
{
    public string GetEmail() => user.Email;

    public string GetName() => user.Name;

    public string GetPassword() => password;

    public string GetToken() => token;
}