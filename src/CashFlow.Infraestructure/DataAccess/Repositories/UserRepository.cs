using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infraestructure.DataAccess.Repositories;

internal class UserRepository(CashFlowDbContext context)
    : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await context.Users.AnyAsync(user => user.Email.Equals(email));
    }

    public async Task Add(User user)
    {
        await context.Users.AddAsync(user);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public async Task<User> GetById(long id)
    {
        return await context.Users.FirstAsync(user => user.Id == id);
    }

    public void Update(User user)
    {
        context.Users.Update(user);
    }
}