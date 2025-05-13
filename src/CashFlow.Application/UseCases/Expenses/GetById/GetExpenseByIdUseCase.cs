using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Execption;
using CashFlow.Execption.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.GetById;

public class GetExpenseByIdUseCase(
    IExpensesReadOnlyRepository repository,
    IMapper mapper,
    ILoggedUser loggedUser
) : IGetExpenseByIdUseCase
{
    public async Task<ResponseExpenseJson> Execute(long id)
    {
        var user = await loggedUser.Get();

        var result = await repository.GetById(user, id);

        if (result is null)
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        return mapper.Map<ResponseExpenseJson>(result);
    }
}