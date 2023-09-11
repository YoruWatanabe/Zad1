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
    public class EditCourseViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;
        private Course? _course = new Course();

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Title" && string.IsNullOrEmpty(Title))
                    return "Title is Required";

                if (columnName == "CourseCode" && string.IsNullOrEmpty(CourseCode))
                    return "Course Code is Required";

                if (columnName == "Instructor" && string.IsNullOrEmpty(Instructor))
                    return "Instructor is Required";

                if (columnName == "Schedule" && string.IsNullOrEmpty(Schedule))
                    return "Schedule is Required";

                if (columnName == "Description" && string.IsNullOrEmpty(Description))
                    return "Description is Required";

                if (columnName == "Department" && string.IsNullOrEmpty(Department))
                    return "Department is Required";

                if (columnName == "Credits" && Credits < 0)
                    return "Credits is Required";

                return string.Empty;
            }
        }
        private bool IsValid()
        {
            if (string.IsNullOrEmpty(CourseCode) ||
                string.IsNullOrEmpty(Title) ||
                string.IsNullOrEmpty(Instructor) ||
                string.IsNullOrEmpty(Schedule) ||
                string.IsNullOrEmpty(Description) ||
                string.IsNullOrEmpty(Department) ||
                Credits < 0)
            {
                return false;
            }
            return true;
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        private string _courseCode = string.Empty;
        public string CourseCode
        {
            get => _courseCode;
            set { _courseCode = value; OnPropertyChanged(nameof(CourseCode)); LoadCourseData(); }
        }

        private string _schedule = string.Empty;
        public string Schedule
        {
            get => _schedule;
            set { _schedule = value; OnPropertyChanged(nameof(Schedule)); }
        }

        private string _department = string.Empty;
        public string Department
        {
            get => _department;
            set { _department = value; OnPropertyChanged(nameof(Department)); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        private int _credits;
        public int Credits
        {
            get => _credits;
            set { _credits = value; OnPropertyChanged(nameof(Credits)); }
        }

        private string _instructor = string.Empty;
        public string Instructor
        {
            get => _instructor;
            set { _instructor = value; OnPropertyChanged(nameof(Instructor)); }
        }

        private string _response = string.Empty;
        public string Response
        {
            get => _response;
            set { _response = value; OnPropertyChanged(nameof(Response)); }
        }

        private ObservableCollection<Student>? _availableStudents;
        public ObservableCollection<Student> AvailableStudents
        {
            get
            {
                if (_availableStudents is null)
                {
                    _availableStudents = LoadStudents();
                    return _availableStudents;
                }
                return _availableStudents;
            }
            set { _availableStudents = value; OnPropertyChanged(nameof(AvailableStudents)); }
        }

        private ObservableCollection<Student>? _assignedStudents;
        public ObservableCollection<Student>? AssignedStudents
        {
            get
            {
                if (_assignedStudents is null)
                {
                    _assignedStudents = new ObservableCollection<Student>();
                    return _assignedStudents;
                }
                return _assignedStudents;
            }
            set { _assignedStudents = value; OnPropertyChanged(nameof(AssignedStudents)); }
        }

        private ICommand? _back;
        public ICommand Back
        {
            get
            {
                if (_back is null)
                {
                    _back = new RelayCommand<object>(NavigateBack);
                }
                return _back;
            }
        }

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.CoursesSubView = new CoursesViewModel(_context, _dialogService);
            }
        }

        private ICommand? _add;
        public ICommand Add
        {
            get
            {
                if (_add is null)
                {
                    _add = new RelayCommand<object>(AddStudent);
                }
                return _add;
            }
        }

        private void AddStudent(object? obj)
        {
            if (obj is Student student)
            {
                if (AssignedStudents is not null && !AssignedStudents.Contains(student))
                {
                    AssignedStudents.Add(student);
                }
            }
        }

        private ICommand? _remove;
        public ICommand? Remove
        {
            get
            {
                if (_remove is null)
                {
                    _remove = new RelayCommand<object>(RemoveStudent);
                }
                return _remove;
            }
        }

        private void RemoveStudent(object? obj)
        {
            if (obj is Student student)
            {
                if (AssignedStudents is not null)
                {
                    AssignedStudents.Remove(student);
                }
            }
        }

        private ICommand? _save;
        public ICommand Save
        {
            get
            {
                if (_save is null)
                {
                    _save = new RelayCommand<object>(SaveData);
                }
                return _save;
            }
        }

        private void SaveData(object? obj)
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            if (_course is null)
            {
                return;
            }

            _course.CourseCode = CourseCode;
            _course.Title = Title;
            _course.Instructor = Instructor;
            _course.Schedule = Schedule;
            _course.Description = Description;
            _course.Department = Department;
            _course.Credits = Credits;

            _context.Entry(_course).State = EntityState.Modified;
            _context.SaveChanges();

            Response = "Data Saved";
        }

        public EditCourseViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        private ObservableCollection<Student> LoadStudents()
        {
            _context.Database.EnsureCreated();
            _context.Students.Load();
            return _context.Students.Local.ToObservableCollection();
        }

        private void LoadCourseData()
        {
            if (_context?.Courses is null)
            {
                return;
            }
            _course = _context.Courses.Find(CourseCode);
            if (_course is null)
            {
                return;
            }

            Title = _course.Title;
            Schedule = _course.Schedule;
            Description = _course.Description;
            Credits = _course.Credits;
            Department = _course.Department;
            Instructor = _course.Instructor;
        }
    }
}