using BookManager.Data;

namespace BookManager
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BookManagerDbContext())
            {
                new BookManagerConsole(new BookManager(db)).Run();
            }
        }
    }
}
