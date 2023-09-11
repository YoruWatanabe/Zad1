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
    public class ExamsTest
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
            SeedTestDB();
            _dialogService = new DialogService();
        }

        private void SeedTestDB()
        {
            _context.Database.EnsureDeleted();
            _context.Exams.Add(new Exam
            {
                ExamId = "E001",
                CourseCode = "MAT",
                Date = new DateTime(2024, 10, 11),
                StartTime = new DateTime(2019, 1, 1, 10, 0, 0),
                EndTime = new DateTime(2019, 1, 1, 12, 0, 0),
                Description = "Finals",
                Location = "Auditorium B",
                Professor = "Marta Kowalska"
            });
            _context.SaveChanges();
        }

        [TestMethod]
        public void TestShowAllExams()
        {
            var examsViewModel = new ExamsViewModel(_context, _dialogService);
            Assert.IsTrue(examsViewModel.Exams.Any());
        }

        [TestMethod]
        public void TestAddExam()
        {
            var addExamViewModel = new AddExamViewModel(_context, _dialogService)
            {
                ExamId = "E002",
                CourseCode = "FIZ",
                Date = new DateTime(2024, 10, 10),
                StartTime = new DateTime(2019, 5, 2, 12, 0, 0),
                EndTime = new DateTime(2019, 5, 2, 14, 0, 0),
                Description = "Exam desc",
                Location = "Auditorium A",
                Professor = "Adam Nowaczek"
            };
            addExamViewModel.Save.Execute(null);

            var examsViewModel = new ExamsViewModel(_context, _dialogService);
            Assert.IsTrue(examsViewModel.Exams.Any(x => x.ExamId == "E002"));
        }
    }
}