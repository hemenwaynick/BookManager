using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using BookManager.Config;

namespace BookManager.Data
{
    public class BookManagerDbContextFactory : IDesignTimeDbContextFactory<BookManagerDbContext>
    {
        public BookManagerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BookManagerDbContext>();
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("ConnectionStrings:Postgres"));

            return new BookManagerDbContext(optionsBuilder.Options);
        }
    }
}
