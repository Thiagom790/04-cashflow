using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Execption;
using CashFlow.Execption.ExceptionBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Update;

public class UpdateUserUseCase(
    ILoggedUser loggedUser,
    IUserReadOnlyRepository userReadOnlyRepository,
    IUserUpdateOnlyRepository userUpdateOnlyRepository,
    IUnitOfWork unitOfWork
) : IUpdateUserUseCase
{
    public async Task Execute(RequestUpdateUserJson request)
    {
        var userLogged = await loggedUser.Get();

        await Validate(request, userLogged.Email);

        var user = await userUpdateOnlyRepository.GetById(userLogged.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        userUpdateOnlyRepository.Update(user);

        await unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var validator = new UpdateUserValidator();

        var result = await validator.ValidateAsync(request);

        if (currentEmail != request.Email)
        {
            var userExist = await userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (userExist)
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREDY_REGISTERED));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}