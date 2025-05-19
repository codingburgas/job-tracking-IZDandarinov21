using JobTracking.DataAccess.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace JobTracking.DataAccess;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<JobAd> JobAds { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=J*B;Trusted_Connection=true;");
        }
    }
}