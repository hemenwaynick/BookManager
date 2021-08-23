using Microsoft.EntityFrameworkCore;
using BookManager.Data;
using BookManager.Models;
using System.Collections.Generic;
using Xunit;

namespace BookManager.Tests
{
    public class BookManagerTests
    {
        private readonly DbContextOptions _options;

        public BookManagerTests()
        {
            _options = new DbContextOptionsBuilder<BookManagerDbContext>()
                .UseInMemoryDatabase("BookManagerDb")
                .Options;
        }

        [Fact]
        public void AddBookSucceeds()
        {
            using (var db = new BookManagerDbContext(_options))
            {
                var bookManager = new BookManager(db);

                var title = "The Hobbit";
                var author = "J. R. R. Tolkien";
                var description = "Unlikely hero takes on dragon";
                var bookId = bookManager.AddBook(
                    title,
                    author,
                    description);

                var book = bookManager.GetBookById(bookId);

                Assert.Equal(title, book.Title);
                Assert.Equal(author, book.Author);
                Assert.Equal(description, book.Description);
            }
        }

        [Fact]
        public void EditBookSucceeds()
        {
            using (var db = new BookManagerDbContext(_options))
            {
                var title = "Ulysses";
                var description = "A day in the life";

                var bookId = db.Books.Add(new Book()
                {
                    Title = title,
                    Author = "James Joyce",
                    Description = description
                })
                .Entity
                .Id;

                db.SaveChanges();

                var bookManager = new BookManager(db);

                var newAuthor = "James Augustine Aloysius Joyce";
                bookManager.EditBook(
                    bookId,
                    "",
                    newAuthor,
                    "");

                var book = bookManager.GetBookById(bookId);

                Assert.Equal(title, book.Title);
                Assert.Equal(newAuthor, book.Author);
                Assert.Equal(description, book.Description);
            }
        }

        [Fact]
        public void SearchForWordsSucceeds()
        {
            using (var db = new BookManagerDbContext(_options))
            {
                var hobbitId = db.Books.Add(new Book()
                {
                    Title = "The Hobbit",
                    Author = "J. R. R. Tolkien",
                    Description = "Unlikely hero takes on dragon"
                })
                .Entity
                .Id;
                var ulyssesId = db.Books.Add(new Book()
                {
                    Title = "Ulysses",
                    Author = "James Joyce",
                    Description = "A day in the life"
                })
                .Entity
                .Id;
                db.Books.Add(new Book()
                {
                    Title = "Decameron",
                    Author = "Boccaccio",
                    Description = "Various"
                });
                var prometheaId = db.Books.Add(new Book()
                {
                    Title = "Promethea",
                    Author = "Alan Moore",
                    Description = "Apocalypse"
                })
                .Entity
                .Id;

                db.SaveChanges();

                var bookManager = new BookManager(db);

                var books = bookManager
                    .SearchForWords(new List<string> { "the", "apocalypse" });

                var hobbit = bookManager.GetBookById(hobbitId);
                var ulysses = bookManager.GetBookById(ulyssesId);
                var promethea = bookManager.GetBookById(prometheaId);

                Assert.Contains(hobbit, books);
                Assert.Contains(ulysses, books);
                Assert.Contains(promethea, books);
            }
        }
    }
}
