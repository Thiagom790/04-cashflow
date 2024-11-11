using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace CommonTestsUtilities.Requests;

public class RequestRegisterExpenseJsonBuilder
{
    static public RequestExpenseJson Build()
    {
        //Posso fazer desta forma
        //var faker = new Faker();
        //var request = new RequestRegisterExpenseJson()
        //{
        //    Title = faker.Commerce.ProductName(),
        //};

        // Ou desta forma
        return new Faker<RequestExpenseJson>()
             .RuleFor(r => r.Title, faker => faker.Commerce.ProductName())
             .RuleFor(r => r.Description, faker => faker.Commerce.ProductDescription())
             .RuleFor(r => r.Date, faker => faker.Date.Past())
             .RuleFor(r => r.PaymentType, faker => faker.PickRandom<PaymentType>())
             .RuleFor(r => r.Amount, faker => faker.Random.Decimal(min: 1, max: 1000));
    }
}
