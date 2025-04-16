using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infraestructure.DataAccess;

public class CashFlowDbContext : DbContext
{
    public CashFlowDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Expense> Expenses { get; set; }
    
    // the Users is the name of the table
    public DbSet<User> Users { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    var connectionString = "Server=localhost;Database=cashflowdb;Uid=root;Pwd=@Pasword123;";

    //    var version = new Version(8, 0, 35);
    //    var serverVersion = new MySqlServerVersion(version);

    //    optionsBuilder.UseMySql(connectionString, serverVersion);
    //}
}
