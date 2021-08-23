using BookManager.Data;
using BookManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookManager
{
    public class BookManager
    {
        private readonly BookManagerDbContext _db;

        public BookManager(BookManagerDbContext db)
        {
            _db = db;
        }

        public int BooksCount => _db.Books.Count();

        public IQueryable<int> BookIds => _db.Books.Select(book => book.Id);

        public IOrderedQueryable<Book> ListBooks() => _db.Books.OrderBy(book => book.Id);

        public Book GetBookById(int id) => _db.Books.SingleOrDefault(book => book.Id == id) ??
            throw new ArgumentException($"Book with ID {id} not found.");

        public int AddBook(
            string title,
            string author,
            string description)
        {
            var book = new Book()
            {
                Title = title,
                Author = author,
                Description = description
            };

            _db.Books.Add(book);

            _db.SaveChanges();

            return book.Id;
        }

        public void EditBook(
            int bookId,
            string newTitle,
            string newAuthor,
            string newDescription)
        {
            var book = GetBookById(bookId);

            if (!string.IsNullOrEmpty(newTitle))
                book.Title = newTitle;

            if (!string.IsNullOrEmpty(newAuthor))
                book.Author = newAuthor;

            if (!string.IsNullOrEmpty(newDescription))
                book.Description = newDescription;

            _db.SaveChanges();
        }

        public IEnumerable<Book> SearchForWords(IEnumerable<string> words)
        {
            var bookMatches = new List<Book>();
            foreach (var book in ListBooks())
            {
                var bookWords = (book.Title + " " + book.Author + " " + book.Description)
                    .Split(" ")
                    .Select(word => word.ToLower());

                foreach (var word in words)
                {
                    if (bookWords.Contains(word))
                        bookMatches.Add(book);
                }
            }

            return bookMatches;
        }

        public void SaveChanges() => _db.SaveChanges();
    }
}
