using System;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels
{
    public class AddBookViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;

        public ICommand Back { get; }
        public ICommand Save { get; }

        public AddBookViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            Back = new RelayCommand(NavigateBack);
            Save = new RelayCommand(SaveData);
        }

        private void NavigateBack()
        {
            var instance = MainWindowViewModel.Instance();
            if (instance != null)
            {
                instance.BooksSubView = new BooksViewModel(_context, _dialogService);
            }
        }

        private void SaveData()
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            var book = new Book
            {
                Title = Title,
                BookId = BookId,
                Author = Author,
                Publisher = Publisher,
                PublicationDate = PublicationDate,
                ISBN = ISBN,
                Genre = Genre,
                Description = Description
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            Response = "Data Saved";
        }

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(Title) ||
                string.IsNullOrEmpty(BookId) ||
                string.IsNullOrEmpty(Author) ||
                string.IsNullOrEmpty(Publisher) ||
                string.IsNullOrEmpty(Description) ||
                string.IsNullOrEmpty(ISBN) ||
                string.IsNullOrEmpty(Genre) ||
                PublicationDate == null)
            {
                return false;
            }

            return true;
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        private string _bookId = string.Empty;
        public string BookId
        {
            get => _bookId;
            set
            {
                if (_bookId != value)
                {
                    _bookId = value;
                    OnPropertyChanged(nameof(BookId));
                }
            }
        }

        private string _author = string.Empty;
        public string Author
        {
            get => _author;
            set
            {
                if (_author != value)
                {
                    _author = value;
                    OnPropertyChanged(nameof(Author));
                }
            }
        }

        private string _publisher = string.Empty;
        public string Publisher
        {
            get => _publisher;
            set
            {
                if (_publisher != value)
                {
                    _publisher = value;
                    OnPropertyChanged(nameof(Publisher));
                }
            }
        }

        private string _isbn = string.Empty;
        public string ISBN
        {
            get => _isbn;
            set
            {
                if (_isbn != value)
                {
                    _isbn = value;
                    OnPropertyChanged(nameof(ISBN));
                }
            }
        }

        private string _genre = string.Empty;
        public string Genre
        {
            get => _genre;
            set
            {
                if (_genre != value)
                {
                    _genre = value;
                    OnPropertyChanged(nameof(Genre));
                }
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private DateTime? _publicationDate;
        public DateTime? PublicationDate
        {
            get => _publicationDate;
            set
            {
                if (_publicationDate != value)
                {
                    _publicationDate = value;
                    OnPropertyChanged(nameof(PublicationDate));
                }
            }
        }

        private string _response = string.Empty;
        public string Response
        {
            get => _response;
            set
            {
                if (_response != value)
                {
                    _response = value;
                    OnPropertyChanged(nameof(Response));
                }
            }
        }

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                if (string.IsNullOrEmpty(columnName) || IsValid())
                {
                    return string.Empty;
                }

                return $"{columnName} is Required";
            }
        }
    }
}