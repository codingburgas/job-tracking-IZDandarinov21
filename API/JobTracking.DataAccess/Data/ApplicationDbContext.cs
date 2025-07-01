using Microsoft.EntityFrameworkCore;
using JobTracking.Domain.Entities;
using JobTracking.Domain.Enums;
using System;

namespace JobTracking.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<JobAdvertisement> JobAdvertisements { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Username).IsUnique();
                entity.Property(u => u.Role)
                      .HasConversion<string>();
            });

            // Configure JobAdvertisement entity
            modelBuilder.Entity<JobAdvertisement>(entity =>
            {
                
                entity.Property(ja => ja.DatePosted).HasDefaultValueSql("CURRENT_TIMESTAMP");

               
            });

            
            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasOne(a => a.User)
                      .WithMany(u => u.Applications)
                      .HasForeignKey(a => a.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.JobAdvertisement)
                      .WithMany(ja => ja.Applications)
                      .HasForeignKey(ja => ja.JobAdvertisementId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(a => a.Status)
                      .HasConversion<string>(); 

                entity.HasIndex(a => new { a.UserId, a.JobAdvertisementId }).IsUnique();
            });

            // Seed data за администратор
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "System",
                    MiddleName = "",
                    LastName = "Admin",
                    Username = "admin",
                    PasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2a64c489c6", // Хеш на "password"
                    Role = UserRole.Admin
                }
            );
        }
    }
}
