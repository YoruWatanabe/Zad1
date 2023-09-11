using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels
{
    public class ExamsViewModel : ViewModelBase
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;

        public bool? DialogResult { get; set; }
        public ObservableCollection<Exam> Exams { get; set; }

        public ICommand Add => new RelayCommand<object>(AddNewExam);
        public ICommand Edit => new RelayCommand<object>(EditExam);
        public ICommand Remove => new RelayCommand<object>(RemoveExam);

        public ExamsViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            _context.Database.EnsureCreated();
            _context.Exams.Load();
            Exams = _context.Exams.Local.ToObservableCollection();
        }

        private void AddNewExam(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.ExamsSubView = new AddExamViewModel(_context, _dialogService);
            }
        }

        private void EditExam(object? obj)
        {
            if (obj is not null)
            {
                string examId = (string)obj;
                var editExamViewModel = new EditExamViewModel(_context, _dialogService)
                {
                    ExamId = examId
                };
                var instance = MainWindowViewModel.Instance();
                if (instance is not null)
                {
                    instance.ExamsSubView = editExamViewModel;
                }
            }
        }

        private void RemoveExam(object? obj)
        {
            if (obj is not null)
            {
                string examId = (string)obj;
                Exam? exam = _context.Exams.Find(examId);
                if (exam is not null)
                {
                    DialogResult = _dialogService.Show(exam.CourseCode + " " + exam.Date.ToString());
                    if (DialogResult == false)
                    {
                        return;
                    }

                    _context.Exams.Remove(exam);
                    _context.SaveChanges();
                }
            }
        }
    }
}