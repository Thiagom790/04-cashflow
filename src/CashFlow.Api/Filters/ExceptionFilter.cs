using CashFlow.Communication.Responses;
using CashFlow.Execption;
using CashFlow.Execption.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlow.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is CashFlowException)
        {
            HandleProjectException(context);
        }
        else
        {
            ThrowUnkowError(context);
        }
    }

    private void HandleProjectException(ExceptionContext context)
    {
        //var ex = (ErrorOnValidationException)context.Exception;
        //if (context.Exception is ErrorOnValidationException ex)
        //{
        //    var errorResponse = new ResponseErrorJson(ex.Errors);

        //    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        //    context.Result = new BadRequestObjectResult(errorResponse);
        //}
        //else if (context.Exception is NotFoundException notFoundException)
        //{
        //    var errorResponse = new ResponseErrorJson(notFoundException.Message);

        //    context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        //    context.Result = new NotFoundObjectResult(errorResponse);
        //}
        //else
        //{
        //    var errorResponse = new ResponseErrorJson(context.Exception.Message);

        //    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        //    context.Result = new BadRequestObjectResult(errorResponse);
        //}
        var cashFlowExeption = (CashFlowException)context.Exception;
        var errorResponse = new ResponseErrorJson(cashFlowExeption.GetErrors());

        context.HttpContext.Response.StatusCode = cashFlowExeption.StatusCode;
        context.Result = new ObjectResult(errorResponse);
    }

    private void ThrowUnkowError(ExceptionContext context)
    {
        var errorResponse = new ResponseErrorJson(ResourceErrorMessages.UNKNOWN_ERROR);

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}
