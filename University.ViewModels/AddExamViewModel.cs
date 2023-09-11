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
    public class AddExamViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;

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

        public AddExamViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            Back = new RelayCommand<object>(_ => NavigateBack());
            Save = new RelayCommand<object>(_ => SaveData());
        }

        public ICommand Back { get; }
        public ICommand Save { get; }

        public string ExamId { get; set; }
        public string CourseCode { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Professor { get; set; }
        public string Response { get; set; }

        private void NavigateBack()
        {
            var instance = MainWindowViewModel.Instance();
            if (instance != null)
            {
                instance.ExamsSubView = new ExamsViewModel(_context, _dialogService);
            }
        }

        private void SaveData()
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            var exam = new Exam
            {
                ExamId = ExamId,
                CourseCode = CourseCode,
                Date = Date,
                StartTime = StartTime,
                EndTime = EndTime,
                Location = Location,
                Professor = Professor,
                Description = Description
            };

            _context.Exams.Add(exam);
            _context.SaveChanges();

            Response = "Data Saved";
        }

        private bool IsValid()
        {
            return !string.IsNullOrEmpty(ExamId) &&
                   !string.IsNullOrEmpty(CourseCode) &&
                   Date.HasValue &&
                   StartTime.HasValue &&
                   EndTime.HasValue &&
                   !string.IsNullOrEmpty(Location) &&
                   !string.IsNullOrEmpty(Description) &&
                   !string.IsNullOrEmpty(Professor);
        }
    }
}