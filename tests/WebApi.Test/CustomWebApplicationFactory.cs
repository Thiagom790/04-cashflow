using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infraestructure.DataAccess;
using CommonTestsUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private Expense _expense;
    private User _user;
    private string _password;
    private string _token;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // para isso eu preciso criar um app settings para teste
        builder.UseEnvironment("Test").ConfigureServices(services =>
        {
            var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            //Nessa linha eu adiciono o dbcontext
            services.AddDbContext<CashFlowDbContext>(config =>
            {
                config.UseInMemoryDatabase("InMemoryDbForTesting");
                config.UseInternalServiceProvider(provider);
            });

            //similar ao que fizemos com as migrations
            var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
            var passwordEncrypter = scope.ServiceProvider.GetRequiredService<IPasswordEncrypter>();

            StartDatabase(dbContext, passwordEncrypter);

            var tokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();
            _token = tokenGenerator.Generate(_user);
        });
    }

    public string GetEmail() => _user.Email;

    public string GetName() => _user.Name;

    public string GetPassword() => _password;

    public string GetToken() => _token;

    public long GetExpenseId() => _expense.Id;

    private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncrypter passwordEncrypter)
    {
        AddUsers(dbContext, passwordEncrypter);
        AddExpenses(dbContext, _user);

        dbContext.SaveChanges();
    }

    private void AddUsers(CashFlowDbContext dbContext, IPasswordEncrypter passwordEncrypter)
    {
        _user = UserBuilder.Build();
        _password = _user.Password;
        _user.Password = passwordEncrypter.Encrypt(_user.Password);

        dbContext.Users.Add(_user);
    }

    private void AddExpenses(CashFlowDbContext dbContext, User user)
    {
        _expense = ExpenseBuilder.Build(user);

        dbContext.Expenses.Add(_expense);
    }
}