using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Execption.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _repoitory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterExpenseUseCase(IExpensesWriteOnlyRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repoitory = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ReponseRegisterExpenseJson> Execute(RequestExpenseJson request)
    {
        Validate(request);

        //var entity = new Expense
        //{
        //    Amount = request.Amount,
        //    Date = request.Date,
        //    Description = request.Description,
        //    Title = request.Title,
        //    PaymentType = (Domain.Enums.PaymentType)request.PaymentType,
        //};
        var entity = _mapper.Map<Expense>(request);

        await _repoitory.Add(entity);

        await _unitOfWork.Commit();

        //return new ReponseRegisterExpenseJson { Title = request.Title };
        return _mapper.Map<ReponseRegisterExpenseJson>(entity);
    }

    private void Validate(RequestExpenseJson request)
    {
        //Example of native validation
        //var titleIsEmpty = string.IsNullOrWhiteSpace(request.Title);
        //if (titleIsEmpty)
        //{
        //    throw new ArgumentException("The title is required");
        //}

        var validator = new RegisterExpenseValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }

}
