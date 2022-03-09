using CashGen.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace CashGen.DBContexts
{
    public class CashGenContext : DbContext
    {
        public CashGenContext(DbContextOptions<CashGenContext> options)
          : base((DbContextOptions)options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Feature> Features { get; set; }

        public DbSet<Collection> Collections { get; set; }

        public DbSet<LineItem> LineItems { get; set; }

        public DbSet<Filter> Filters { get; set; }

        public DbSet<FilterOption> FilterOptions { get; set; }

        public DbSet<FilterCollection> FilterCollections { get; set; }

        public DbSet<ProductFilter> ProductFilters { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<ShopifyCollect> ShopifyCollects { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<StoreUser> StoreUsers { get; set; }

        public DbSet<EventLog> EventLogs { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Chat> Chats { get; set; }

        protected virtual void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (!(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"))
                return;
            modelBuilder.Entity<Store>().HasData(new Store[1]
            {
        new Store()
        {
          Id = Guid.Parse("ff71fc86-5bb1-4cb6-a56f-efe88d113733"),
          Email = "craig@day32.co.uk",
          Title = "Test Store"
        }
            });
            modelBuilder.Entity<Product>().HasData(new Product[1]
            {
        new Product()
        {
          Id = Guid.Parse("d173e20d-159e-4127-9ce9-b0ac2564ad97"),
          StoreId = Guid.Parse("ff71fc86-5bb1-4cb6-a56f-efe88d113733"),
          Email = "craig@day32.co.uk",
          Title = "Sony PS4",
          Barcode = "ABCD1234",
          Price = Convert.ToDecimal(179.99)
        }
            });
            base.OnModelCreating(modelBuilder);
        }

        public class CashGenContextFactory : IDesignTimeDbContextFactory<CashGenContext>
        {
            public CashGenContext CreateDbContext(string[] args)
            {
                DbContextOptionsBuilder<CashGenContext> contextOptionsBuilder = new DbContextOptionsBuilder<CashGenContext>();
                SqlServerDbContextOptionsExtensions.UseSqlServer<CashGenContext>(contextOptionsBuilder, "Server=tcp:cashgen.database.windows.net,1433;Initial Catalog=cashgen;Persist Security Info=False;User ID=craigmanx;Password=$Plokij32;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", (Action<SqlServerDbContextOptionsBuilder>)null);
                return new CashGenContext(contextOptionsBuilder.Options);
            }
        }
    }
}
