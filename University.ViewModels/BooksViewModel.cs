using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels;

public class BooksViewModel : ViewModelBase
{
    private readonly UniversityContext _context;
    private readonly IDialogService _dialogService;

    public BooksViewModel(UniversityContext context, IDialogService dialogService)
    {
        _context = context;
        _dialogService = dialogService;

        _context.Database.EnsureCreated();
        _context.Books.Load();
        Books = _context.Books.Local.ToObservableCollection();

        Add = new RelayCommand<object>(AddNewBook);
        Edit = new RelayCommand<object>(EditBook);
        Remove = new RelayCommand<object>(RemoveBook);
    }

    public ObservableCollection<Book> Books { get; set; } = new ObservableCollection<Book>();
    public bool? DialogResult { get; set; }

    public ICommand Add { get; }
    public ICommand Edit { get; }
    public ICommand Remove { get; }

    private void AddNewBook(object? obj)
    {
        var instance = MainWindowViewModel.Instance();
        if (instance is not null)
        {
            instance.BooksSubView = new AddBookViewModel(_context, _dialogService);
        }
    }

    private void EditBook(object? obj)
    {
        if (obj is not null)
        {
            string bookId = (string)obj;
            var editBookViewModel = new EditBookViewModel(_context, _dialogService) { BookId = bookId };
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.BooksSubView = editBookViewModel;
            }
        }
    }

    private void RemoveBook(object? obj)
    {
        if (obj is not null)
        {
            string bookId = (string)obj;
            var book = _context.Books.Find(bookId);
            if (book is not null)
            {
                DialogResult = _dialogService.Show(book.Title);
                if (DialogResult == false)
                {
                    return;
                }

                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }
    }
}