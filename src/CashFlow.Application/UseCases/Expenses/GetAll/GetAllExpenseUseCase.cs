using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

public class GetAllExpenseUseCase(
    IExpensesReadOnlyRepository repository,
    IMapper mapper,
    ILoggedUser loggedUser
) : IGetAllExpenseUseCase
{
    public async Task<ResponseExpensesJson> Execute()
    {
        var user = await loggedUser.Get();

        var result = await repository.GetAll(user);

        return new ResponseExpensesJson
        {
            Expenses = mapper.Map<List<ResponseShortExpenseJson>>(result)
        };
    }
}