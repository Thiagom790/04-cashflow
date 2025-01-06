using System.Net.Mime;
using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    [HttpGet("excel")]
    //[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel([FromHeader] DateOnly month,
        [FromServices] IGenerateExpensesReportExcelUseCase useCase)
    {
        //var file = new byte[1];
        var file = await useCase.Execute(month);

        if (file.Length > 0) return File(file, MediaTypeNames.Application.Octet, "report.xlsx");

        return NoContent();
    }
}