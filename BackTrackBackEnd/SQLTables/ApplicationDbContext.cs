using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BackTrackBackEnd.SQLTables;

public class ApplicationDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{
    /// <summary>
    /// Setting up the accounts database
    /// </summary>
    public DbSet<Account> Accounts { get; set; } 

    /// <summary>
    /// The configuration where the Connection String is stored
    /// </summary>
    private readonly IConfiguration _configuration;

    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="configuration">The configuration of the application</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, 
      IConfiguration configuration) 
      : base(options)
    {
        _configuration = configuration;
    }

    
    /// <summary>
    /// Used for setting up the database
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=sql_server;Database=BackTrackDataBase;User Id=sa;Password=ThisPassword1<; TrustServerCertificate=True;");
        }
    }
}