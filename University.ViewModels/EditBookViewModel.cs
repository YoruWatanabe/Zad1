using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels;

public class EditBookViewModel : ViewModelBase
{
    private readonly UniversityContext _context;
    private readonly IDialogService _dialogService;
    private Book _book = new Book();


    public EditBookViewModel(UniversityContext context, IDialogService dialogService)
    {
        _context = context;
        _dialogService = dialogService;
        _context.Books.Load();
        Save = new RelayCommand(SaveData, () => IsValid());
        Back = new RelayCommand<object>(NavigateBack);
    }

    public string BookId
    {
        get => _book.BookId;
        set
        {
            _book.BookId = value;
            OnPropertyChanged(nameof(BookId));
            LoadBookData();
        }
    }

    public string Title
    {
        get => _book.Title;
        set
        {
            _book.Title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    public string Author
    {
        get => _book.Author;
        set
        {
            _book.Author = value;
            OnPropertyChanged(nameof(Author));
        }
    }

    public string Publisher
    {
        get => _book.Publisher;
        set
        {
            _book.Publisher = value;
            OnPropertyChanged(nameof(Publisher));
        }
    }

    public string ISBN
    {
        get => _book.ISBN;
        set
        {
            _book.ISBN = value;
            OnPropertyChanged(nameof(ISBN));
        }
    }

    public string Genre
    {
        get => _book.Genre;
        set
        {
            _book.Genre = value;
            OnPropertyChanged(nameof(Genre));
        }
    }

    public string Description
    {
        get => _book.Description;
        set
        {
            _book.Description = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    public DateTime? PublicationDate
    {
        get => _book.PublicationDate;
        set
        {
            _book.PublicationDate = value;
            OnPropertyChanged(nameof(PublicationDate));
        }
    }

    public string Response { get; set; } = string.Empty;
    public ICommand Save { get; }
    public ICommand Back { get; }

    private void NavigateBack(object? obj)
    {
        var instance = MainWindowViewModel.Instance();
        if (instance != null)
        {
            instance.BooksSubView = new BooksViewModel(_context, _dialogService);
        }
    }

    private void SaveData()
    {
        if (_book == null)
        {
            return;
        }

        _context.Entry(_book).State = EntityState.Modified;
        _context.SaveChanges();

        Response = "Data Updated";
    }

    private bool IsValid()
    {
        if (string.IsNullOrEmpty(BookId)) return false;
        if (string.IsNullOrEmpty(Title)) return false;
        if (string.IsNullOrEmpty(Author)) return false;
        if (string.IsNullOrEmpty(Publisher)) return false;
        if (string.IsNullOrEmpty(Description)) return false;
        if (PublicationDate is null) return false;
        if (string.IsNullOrEmpty(ISBN)) return false;
        if (string.IsNullOrEmpty(Genre)) return false;

        return true;
    }

    private void LoadBookData()
    {
        if (_context?.Books == null)
        {
            return;
        }

        _book = _context.Books.Find(BookId);

        if (_book != null)
        {
            this.Title = _book.Title;
            this.Author = _book.Author;
            this.Publisher = _book.Publisher;
            this.PublicationDate = _book.PublicationDate;
            this.ISBN = _book.ISBN;
            this.Genre = _book.Genre;
            this.Description = _book.Description;
        }
    }
}