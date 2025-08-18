using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infraestructure.DataAccess;
using CommonTestsUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public ExpenseIdentityManager Expense_Admin { get; set; } = null!;

    public ExpenseIdentityManager Expense_MemberTeam { get; private set; } = null!;

    public UserIdentityManager User_Team_Member { get; private set; } = null!;

    public UserIdentityManager User_Admin { get; private set; } = null!;

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
            var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

            StartDatabase(dbContext, passwordEncrypter, accessTokenGenerator);
        });
    }


    private void StartDatabase(
        CashFlowDbContext dbContext,
        IPasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        var userTeamMember = AddUserTeamMember(dbContext, passwordEncrypter, accessTokenGenerator);
        var expenseTeamMember = AddExpenses(dbContext, userTeamMember, expenseId: 1);
        Expense_MemberTeam = new ExpenseIdentityManager(expenseTeamMember);

        var userAdmin = AddUserAdmin(dbContext, passwordEncrypter, accessTokenGenerator);
        var expenseAdmin = AddExpenses(dbContext, userAdmin, expenseId: 2);
        Expense_Admin = new ExpenseIdentityManager(expenseAdmin);

        dbContext.SaveChanges();
    }

    private User AddUserTeamMember(
        CashFlowDbContext dbContext,
        IPasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build();
        user.Id = 1;

        var password = user.Password;
        user.Password = passwordEncrypter.Encrypt(user.Password);

        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;
    }

    private User AddUserAdmin(
        CashFlowDbContext dbContext,
        IPasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        user.Id = 2;

        var password = user.Password;
        user.Password = passwordEncrypter.Encrypt(user.Password);

        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        User_Admin = new UserIdentityManager(user, password, token);

        return user;
    }

    private Expense AddExpenses(CashFlowDbContext dbContext, User user, long expenseId)
    {
        var expense = ExpenseBuilder.Build(user);
        expense.Id = expenseId;

        dbContext.Expenses.Add(expense);

        return expense;
    }
}