using CashFlow.Domain.Entities;

namespace WebApi.Test.Resources;

public class ExpenseIdentityManager(Expense expense)
{
    public long GetExpenseId() => expense.Id;
}