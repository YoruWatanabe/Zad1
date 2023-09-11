using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using University.Data;
using University.Interfaces;
using University.Models;
using University.Services;
using University.ViewModels;

namespace University.Tests
{
    [TestClass]
    public class BooksTest
    {
        private UniversityContext _context;
        private IDialogService _dialogService;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase("UniversityTestDB")
                .Options;

            _context = new UniversityContext(options);
            SeedTestDatabase();
            _dialogService = new DialogService();
        }

        private void SeedTestDatabase()
        {
            _context.Database.EnsureDeleted();
            _context.Books.Add(new Book
            {
                BookId = "B0001",
                Title = "It",
                Author = "Stephen King",
                ISBN = "978-1501142970",
                Publisher = "Scribner Book Company",
                PublicationDate = new DateTime(2016, 1, 6),
                Description = "Des...",
                Genre = "Novel"
            });
            _context.SaveChanges();
        }

        [TestMethod]
        public void TestShowAllBooks()
        {
            var booksViewModel = new BooksViewModel(_context, _dialogService);
            var hasData = booksViewModel.Books.Any();
            Assert.IsTrue(hasData);
        }

        [TestMethod]
        public void TestAddBook()
        {
            var addBookViewModel = new AddBookViewModel(_context, _dialogService)
            {
                BookId = "B0002",
                Title = "Pet Sematary",
                Author = "Stephen King",
                ISBN = "978-1982112394",
                Publisher = "Random House US",
                PublicationDate = new DateTime(2018, 12, 4),
                Description = "Des...",
                Genre = "Novel"
            };
            addBookViewModel.Save.Execute(null);

            var booksViewModel = new BooksViewModel(_context, _dialogService);
            var hasData = booksViewModel.Books.Any(x => x.ISBN == "978-1982112394");
            Assert.IsTrue(hasData);
        }
    }
}