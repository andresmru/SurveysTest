using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Surveys.Model
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<ProductExistence> ProductExistences { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductExistence>().HasKey(s => new { s.StoreId, s.ProductId});

            // Queries
            var queryMethod = typeof(ModelBuilder).GetMethod("Query", new Type[] { });

            Type type = null;
            
            type = typeof(ProductExistenceDTO);
            queryMethod.MakeGenericMethod(type).Invoke(modelBuilder, new object[] { });

            base.OnModelCreating(modelBuilder);
        }
    }
}
