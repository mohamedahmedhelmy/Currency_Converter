using Core.Domain;
using Core.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AppDbContext :IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.SeedRoles(builder);
            this.SeedUserRoles(builder);
        }


        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = "fab4fac1-c546-41de-aebc-a14da6895711", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" }
                );
        }

        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId = "fab4fac1-c546-41de-aebc-a14da6895711", UserId = "8e656909-9ba2-4129-b4b6-d7d1688df258" }
                );
        }
        public DbSet<Currency> currencies { get; set; }
        public DbSet<ExchangeHistory> exchangeHistory { get; set; }

    }
}
