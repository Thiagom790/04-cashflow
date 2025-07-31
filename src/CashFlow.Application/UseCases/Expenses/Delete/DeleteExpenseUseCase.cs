using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Execption;
using CashFlow.Execption.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase(
    IExpensesReadOnlyRepository expensesReadOnlyRepository,
    IExpensesWriteOnlyRepository repository,
    IUnitOfWork unitOfWork,
    ILoggedUser user
) : IDeleteExpenseUseCase
{
    public async Task Execute(long id)
    {
        var loggedUser = await user.Get();

        var expenses = await expensesReadOnlyRepository.GetById(loggedUser, id);

        if (expenses is null)
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        await repository.Delete(id);

        await unitOfWork.Commit();
    }
}