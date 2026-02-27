using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestsUtilities.Requests;

public static class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(user => user.Name, f => f.Person.FirstName)
            .RuleFor(email => email.Email, (f, user) => f.Internet.Email(user.Name))
            .Generate();
    }
}