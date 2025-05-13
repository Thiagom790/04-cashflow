using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infraestructure.DataAccess.Repositories;

internal class ExpensesRepository(CashFlowDbContext dbContext) :
    IExpensesReadOnlyRepository,
    IExpensesWriteOnlyRepository,
    IExpensesUpdateOnlyRepository
{
    public async Task<List<Expense>> GetAll(User user)
    {
        return await dbContext.Expenses.AsNoTracking().Where(expense => expense.UserId == user.Id).ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(User user, long id)
    {
        return await dbContext.Expenses
            .AsNoTracking()
            .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
    }

    public Task<List<Expense>> FilterByMonth(DateOnly date)
    {
        var startDate = new DateTime(date.Year, date.Month, 1).Date;

        var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
        var endDate = new DateTime(date.Year, date.Month, daysInMonth, 23, 59, 59);

        return dbContext
            .Expenses
            .AsNoTracking()
            .Where(expense => expense.Date >= startDate && expense.Date <= endDate)
            .OrderBy(expense => expense.Date)
            .ThenBy(expense => expense.Title)
            .ToListAsync();
    }

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(User user, long id)
    {
        return await dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
    }

    public void Update(Expense expense)
    {
        dbContext.Expenses.Update(expense);
    }

    public async Task Add(Expense expense)
    {
        //var dbContext = new CashFlowDbContext();

        //dbContext.Expenses.Add(expense);
        //dbContext.SaveChanges();
        //_dbContext.Expenses.Add(expense);
        //_dbContext.SaveChanges(); // removemos daqui pois o ideal é que só no final que aconteça o save changes
        await dbContext.Expenses.AddAsync(expense);
    }

    public async Task Delete(long id)
    {
        var result = await dbContext.Expenses.FindAsync(id);

        dbContext.Expenses.Remove(result!);
        // var result = await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);

        // if (result is null) return false;
        //
        // _dbContext.Expenses.Remove(result);
        //
        // return true;
    }
}