namespace CashFlow.Execption.ExceptionBase;

abstract public class CashFlowException : SystemException
{
    protected CashFlowException(string? message) : base(message)
    {
    }

    public abstract int StatusCode { get; }

    public abstract List<string> GetErrors();
}
