using System.Net;

namespace CashFlow.Execption.ExceptionBase;

public class ErrorOnValidationException : CashFlowException
{
    //public List<string> Errors { get; set; }
    private readonly List<string> _errors;

    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public ErrorOnValidationException(List<string> errorMessages) : base(string.Empty)
    {
        _errors = errorMessages;
    }

    public override List<string> GetErrors()
    {
        return _errors;
    }
}
