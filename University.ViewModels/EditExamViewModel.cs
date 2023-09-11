using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels
{
    public class EditExamViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;
        private Exam? _exam = new Exam();

        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string columnName]
        {
            get
            {
                if (string.IsNullOrEmpty(columnName))
                    return string.Empty;

                switch (columnName)
                {
                    case "ExamId":
                        if (string.IsNullOrEmpty(ExamId))
                            return "ExamId is Required";
                        break;
                    case "CourseCode":
                        if (string.IsNullOrEmpty(CourseCode))
                            return "CourseCode is Required";
                        break;
                    case "Date":
                        if (Date is null)
                            return "Date is Required";
                        break;
                    case "StartTime":
                        if (StartTime is null)
                            return "StartTime is Required";
                        break;
                    case "EndTime":
                        if (EndTime is null)
                            return "EndTime is Required";
                        break;
                    case "Location":
                        if (string.IsNullOrEmpty(Location))
                            return "Location is Required";
                        break;
                    case "Description":
                        if (string.IsNullOrEmpty(Description))
                            return "Description is Required";
                        break;
                    case "Professor":
                        if (string.IsNullOrEmpty(Professor))
                            return "Professor is Required";
                        break;
                }

                return string.Empty;
            }
        }

        private string _examId = string.Empty;
        public string ExamId
        {
            get
            {
                return _examId;
            }
            set
            {
                _examId = value;
                OnPropertyChanged(nameof(ExamId));
                LoadExamData();
            }
        }

        private string _courseCode = string.Empty;
        public string CourseCode
        {
            get
            {
                return _courseCode;
            }
            set
            {
                _courseCode = value;
                OnPropertyChanged(nameof(CourseCode));
            }
        }

        private DateTime? _date = null;
        public DateTime? Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private DateTime? _startTime = null;
        public DateTime? StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime = value;
                OnPropertyChanged(nameof(StartTime));
            }
        }

        private DateTime? _endTime = null;
        public DateTime? EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                _endTime = value;
                OnPropertyChanged(nameof(EndTime));
            }
        }

        private string _location = string.Empty;
        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private string _professor = string.Empty;
        public string Professor
        {
            get
            {
                return _professor;
            }
            set
            {
                _professor = value;
                OnPropertyChanged(nameof(Professor));
            }
        }

        private string _response = string.Empty;
        public string Response
        {
            get
            {
                return _response;
            }
            set
            {
                _response = value;
                OnPropertyChanged(nameof(Response));
            }
        }

        private ICommand _back = null;
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
                instance.ExamsSubView = new ExamsViewModel(_context, _dialogService);
            }
        }

        private ICommand _save = null;
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
            if (HasValidationErrors())
            {
                Response = "Please complete all required fields";
                return;
            }

            if (_exam is null)
            {
                return;
            }

            _exam.CourseCode = CourseCode;
            _exam.ExamId = ExamId;
            _exam.Date = Date;
            _exam.StartTime = StartTime;
            _exam.EndTime = EndTime;
            _exam.Location = Location;
            _exam.Professor = Professor;
            _exam.Description = Description;

            _context.Entry(_exam).State = EntityState.Modified;
            _context.SaveChanges();

            Response = "Data Updated";
        }

        public EditExamViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        private bool HasValidationErrors()
        {
            return !string.IsNullOrEmpty(this["ExamId"])
                || !string.IsNullOrEmpty(this["CourseCode"])
                || !string.IsNullOrEmpty(this["Date"])
                || !string.IsNullOrEmpty(this["StartTime"])
                || !string.IsNullOrEmpty(this["EndTime"])
                || !string.IsNullOrEmpty(this["Location"])
                || !string.IsNullOrEmpty(this["Description"])
                || !string.IsNullOrEmpty(this["Professor"]);
        }

        private void LoadExamData()
        {
            if (_context?.Exams is null)
            {
                return;
            }
            _exam = _context.Exams.Find(ExamId);
            if (_exam is null)
            {
                return;
            }
            this.CourseCode = _exam.CourseCode;
            this.Date = _exam.Date;
            this.StartTime = _exam.StartTime;
            this.EndTime = _exam.EndTime;
            this.Location = _exam.Location;
            this.Professor = _exam.Professor;
            this.Description = _exam.Description;
        }
    }
}