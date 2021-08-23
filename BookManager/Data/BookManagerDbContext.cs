using Microsoft.EntityFrameworkCore;
using BookManager.Config;
using BookManager.Models;

namespace BookManager.Data
{
    public class BookManagerDbContext : DbContext
    {
        private readonly bool _useNpgsql;

        public BookManagerDbContext(DbContextOptions options)
            : base(options)
        {
            _useNpgsql = false;
        }

        public BookManagerDbContext()
        {
            _useNpgsql = true;
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Book>().HasKey(b => b.Id);
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (_useNpgsql)
            {
                var connectionString = Configuration.GetConnectionString("ConnectionStrings:Postgres");
                builder.UseNpgsql(connectionString);
            }
        }
    }
}
