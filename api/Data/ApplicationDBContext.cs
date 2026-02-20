using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Models.StoredProcedures;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
       public ApplicationDBContext(DbContextOptions dbContextOptions)
       : base(dbContextOptions)
       {
        
       } 
       public DbSet<Stock> Stocks { get; set; }
       public DbSet<Comment> Comments { get; set; }
       public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<StockWithCommentsRow> StockWithCommentsRows { get; set; }
    public DbSet<CommentWithUserRow> CommentWithUserRows { get; set; }
    public DbSet<PortfolioStockRow> PortfolioStockRows { get; set; }
    public DbSet<ExistsRow> ExistsRows { get; set; }
       protected override void OnModelCreating(ModelBuilder builder)
       {
        base.OnModelCreating(builder);
        builder.Entity<Portfolio>(x=> x.HasKey(p=> new {p.AppUserId, p.StockId}));
        builder.Entity<Portfolio>().HasOne(p=> p.AppUser)
        .WithMany(a=> a.Portfolios)
        .HasForeignKey(p=> p.AppUserId);
        builder.Entity<Portfolio>().HasOne(p=> p.Stock)
        .WithMany(s=> s.Portfolios)
        .HasForeignKey(p=> p.StockId);
        List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole { Name = "User", NormalizedName = "USER" },
            new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" }
        };

        builder.Entity<IdentityRole>().HasData(roles);

        builder.Entity<StockWithCommentsRow>().HasNoKey();
        builder.Entity<CommentWithUserRow>().HasNoKey();
        builder.Entity<PortfolioStockRow>().HasNoKey();
        builder.Entity<ExistsRow>().HasNoKey();
        }
    }
}