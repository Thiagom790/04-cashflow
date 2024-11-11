using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

// Não utilizar essa forma é como era a v1
public interface IExpensesRepository
{
    Task Add(Expense expense);
    Task<List<Expense>> GetAll();
    Task<Expense?> GetById(long id);
}
