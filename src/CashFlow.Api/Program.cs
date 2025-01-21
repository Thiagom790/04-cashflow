using CashFlow.Api.Filters;
using CashFlow.Api.Middleware;
using CashFlow.Application;
using CashFlow.Infraestructure;
using CashFlow.Infraestructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

// Existem 3 tipos de injeção
// Singleton - Single instance shared across the application.
// Transient - New instance created each time it’s requested.
// scoped - Single instance shared within a specific scope (e.g., an HTTP request).

//DependencyInjectionExtension.AddInfraestructure(builder.Services);
//var connectionString = builder.Configuration.GetConnectionString("Connection");

builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddAplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await MigrateDatabase();

app.Run();

// dessa forma criamos migration de forma automática quando a aplicação inicia
async Task MigrateDatabase()
{
    await using var scope = app.Services.CreateAsyncScope();
    await DataBaseMigration.MigrateDatabase(scope.ServiceProvider);
}