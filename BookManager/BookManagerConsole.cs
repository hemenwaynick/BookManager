using System;
using System.Collections.Generic;
using System.Linq;

namespace BookManager
{
    public class BookManagerConsole
    {
        private readonly string _spaces = "      ";
        private readonly BookManager _bookManager;

        public BookManagerConsole(BookManager bookManager)
        {
            _bookManager = bookManager;
        }

        public void Run()
        {
            var bookCount = _bookManager.BooksCount;
            if (bookCount > 0)
                Console.WriteLine($"Loaded {bookCount} book(s) into the library.");

            var prompt = "\n==== Book Manager ====\n\n" +
                _spaces + "1) View all books\n" +
                _spaces + "2) Add a book\n" +
                _spaces + "3) Edit a book\n" +
                _spaces + "4) Search for a book\n" +
                _spaces + "5) Save and exit\n\n" +
                "Choose [1-5]: ";

            DisplayPromptAndRespondToInput(
                prompt,
                Enumerable.Range(1, 5),
                HandleMainMenuSelection);
        }

        private void DisplayPromptAndRespondToInput(
            string prompt,
            IEnumerable<int> validSelections,
            Action<int> selectionHandler)
        {
            while (true)
            {
                Console.Write(prompt);

                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    break;

                if (!int.TryParse(input, out var selection) ||
                    !validSelections.Contains(selection))
                {
                    Console.WriteLine("Invalid selection. Please, try again.");
                }
                else
                {
                    selectionHandler(selection);
                }
            }
        }

        private void HandleMainMenuSelection(int selection)
        {
            switch (selection)
            {
                case 1:
                    ViewBooks();
                    break;
                case 2:
                    AddBook();
                    break;
                case 3:
                    EditBook();
                    break;
                case 4:
                    SearchForBook();
                    break;
                case 5:
                    SaveAndExit();
                    break;
            }
        }

        private void ViewBooks()
        {
            Console.WriteLine("\n==== View Books ====\n");
            foreach (var book in _bookManager.ListBooks())
            {
                Console.WriteLine(_spaces + $"[{book.Id}] {book.Title}");
            }

            var prompt = "\nTo view details, enter the book ID. " +
                "To return, press <Enter>.\n\nBook ID: ";

            DisplayPromptAndRespondToInput(
                prompt,
                _bookManager.BookIds,
                HandleViewBooksSelection);
        }

        private void HandleViewBooksSelection(int selection)
        {
            var book = _bookManager.GetBookById(selection);
            Console.Write($"\n" + _spaces + $"ID: {book.Id}\n" +
                _spaces + $"Title: {book.Title}\n" +
                _spaces + $"Author: {book.Author}\n" +
                _spaces + $"Description: {book.Description}\n");
        }

        private void AddBook()
        {
            Console.Write("\n==== Add a Book ====\n\n" +
                "Please enter the following information:\n\n" +
                _spaces + "Title: ");
            var title = Console.ReadLine();

            Console.Write(_spaces + "Author: ");
            var author = Console.ReadLine();

            Console.Write(_spaces + "Description: ");
            var description = Console.ReadLine();

            var bookId = _bookManager.AddBook(title, author, description);

            Console.WriteLine($"\nBook [{bookId}] saved.");
        }

        private void EditBook()
        {
            Console.WriteLine("\n==== Edit a Book ====\n");
            foreach (var book in _bookManager.ListBooks())
            {
                Console.WriteLine(_spaces + $"[{book.Id}] {book.Title}");
            }

            var prompt = "\nEnter the book ID of the book you want to edit. " +
                "To return, press <Enter>.\n\nBook ID: ";

            DisplayPromptAndRespondToInput(
                prompt,
                _bookManager.BookIds,
                HandleEditBooksSelection);
        }

        private void HandleEditBooksSelection(int selection)
        {
            var book = _bookManager.GetBookById(selection);

            Console.Write("\nInput the following information. " +
                "To leave a field unchanged, press <Enter>.\n\n" +
                _spaces + $"Title [{book.Title}]: ");
            var newTitle = Console.ReadLine();

            Console.Write(_spaces + $"Author [{book.Author}]: ");
            var newAuthor = Console.ReadLine();

            Console.Write(_spaces + $"Description [{book.Description}]: ");
            var newDescription = Console.ReadLine();

            _bookManager.EditBook(book.Id, newTitle, newAuthor, newDescription);

            Console.WriteLine("\nBook saved.");
        }

        private void SearchForBook()
        {
            Console.Write("\n==== Search ====\n\n" +
                "Type in one or more keywords to search for.\n\n" +
                _spaces + "Search: ");
            var wordsToSearchFor = Console.ReadLine()
                .Split(" ")
                .Select(word => word.ToLower());

            var bookMatches = _bookManager.SearchForWords(wordsToSearchFor);

            Console.WriteLine("\nThe following books matched your query. Enter the book ID to see more details or <Enter> to return.\n");
            foreach (var bookMatch in bookMatches)
            {
                Console.WriteLine(_spaces + $"[{bookMatch.Id}] {bookMatch.Title}");
            }

            var prompt = "\nBook ID: ";

            DisplayPromptAndRespondToInput(
                prompt,
                bookMatches.Select(book => book.Id),
                HandleSearchForBookSelection);
        }

        private void HandleSearchForBookSelection(int selection)
        {
            var book = _bookManager.GetBookById(selection);
            Console.Write("\n" + _spaces + $"ID: {book.Id}\n" +
                _spaces + $"Title: {book.Title}\n" +
                _spaces + $"Author: {book.Author}\n" +
                _spaces + $"Description: {book.Description}\n");
        }

        private void SaveAndExit()
        {
            _bookManager.SaveChanges();
            Console.WriteLine("\nLibrary saved.");
            Environment.Exit(0);
        }
    }
}
