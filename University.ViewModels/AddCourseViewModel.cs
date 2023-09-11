using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels
{
    public class AddCourseViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;

        public AddCourseViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            Back = new RelayCommand<object>(_ => NavigateBack());
            Add = new RelayCommand<Student>(AddStudent);
            Remove = new RelayCommand<Student>(RemoveStudent);
            Save = new RelayCommand<object>(_ => SaveData());

            AvailableStudents = new ObservableCollection<Student>(LoadStudents());
            AssignedStudents = new ObservableCollection<Student>();
        }

        public ICommand Back { get; }
        public ICommand Add { get; }
        public ICommand Remove { get; }
        public ICommand Save { get; }

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

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string _courseCode = string.Empty;
        public string CourseCode
        {
            get => _courseCode;
            set
            {
                _courseCode = value;
                OnPropertyChanged(nameof(CourseCode));
            }
        }

        private string _schedule = string.Empty;
        public string Schedule
        {
            get => _schedule;
            set
            {
                _schedule = value;
                OnPropertyChanged(nameof(Schedule));
            }
        }

        private string _department = string.Empty;
        public string Department
        {
            get => _department;
            set
            {
                _department = value;
                OnPropertyChanged(nameof(Department));
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private int _credits;
        public int Credits
        {
            get => _credits;
            set
            {
                _credits = value;
                OnPropertyChanged(nameof(Credits));
            }
        }

        private string _instructor = string.Empty;
        public string Instructor
        {
            get => _instructor;
            set
            {
                _instructor = value;
                OnPropertyChanged(nameof(Instructor));
            }
        }

        private ObservableCollection<Student> _availableStudents;
        public ObservableCollection<Student> AvailableStudents
        {
            get => _availableStudents;
            set
            {
                _availableStudents = value;
                OnPropertyChanged(nameof(AvailableStudents));
            }
        }

        private ObservableCollection<Student> _assignedStudents;
        public ObservableCollection<Student> AssignedStudents
        {
            get => _assignedStudents;
            set
            {
                _assignedStudents = value;
                OnPropertyChanged(nameof(AssignedStudents));
            }
        }

        private string _response = string.Empty;
        public string Response
        {
            get => _response;
            set
            {
                _response = value;
                OnPropertyChanged(nameof(Response));
            }
        }

        private ObservableCollection<Student> LoadStudents()
        {
            _context.Database.EnsureCreated();
            _context.Students.Load();
            return _context.Students.Local.ToObservableCollection();
        }

        private bool IsValid()
        {
            return !string.IsNullOrEmpty(CourseCode) &&
                   !string.IsNullOrEmpty(Title) &&
                   !string.IsNullOrEmpty(Instructor) &&
                   !string.IsNullOrEmpty(Schedule) &&
                   !string.IsNullOrEmpty(Description) &&
                   !string.IsNullOrEmpty(Department);
        }

        private void NavigateBack()
        {
            var instance = MainWindowViewModel.Instance();
            if (instance != null)
            {
                instance.CoursesSubView = new CoursesViewModel(_context, _dialogService);
            }
        }

        private void AddStudent(Student student)
        {
            if (!AssignedStudents.Contains(student))
            {
                AssignedStudents.Add(student);
            }
        }

        private void RemoveStudent(Student student)
        {
            AssignedStudents.Remove(student);
        }

        private void SaveData()
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            var course = new Course
            {
                Title = Title,
                CourseCode = CourseCode,
                Schedule = Schedule,
                Description = Description,
                Credits = Credits,
                Department = Department,
                Instructor = Instructor,
            };

            _context.Courses.Add(course);
            _context.SaveChanges();

            Response = "Data Saved";
        }
    }
}