using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartSchool.Core.Entities;

namespace SmartSchool.Persistence
{
  public class ApplicationDbContext : DbContext
  {
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<Measurement> Measurements { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      var builder = new ConfigurationBuilder()
        .SetBasePath(Environment.CurrentDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true,
          reloadOnChange: true)
        .AddEnvironmentVariables();

      var configuration = builder.Build();
      Debug.Write(configuration.ToString());
      string connectionString = configuration["ConnectionStrings:DefaultConnection"];
      optionsBuilder.UseSqlServer(connectionString);

    }
  }
}
