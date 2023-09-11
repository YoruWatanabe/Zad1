using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using University.Data;
using University.Interfaces;
using University.Models;
using University.Services;
using University.ViewModels;

namespace University.Tests
{
    [TestClass]
    public class StudentsTest
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
            var students = new[]
            {
                new Student { Name = "Wieczysław", LastName = "Nowakowicz", PESEL = "PESEL1", BirthDate = new DateTime(1987, 05, 22) },
                new Student { Name = "Stanisław", LastName = "Nowakowicz", PESEL = "PESEL2", BirthDate = new DateTime(2019, 06, 25) },
                new Student { Name = "Eugenia", LastName = "Nowakowicz", PESEL = "PESEL3", BirthDate = new DateTime(2021, 06, 08) }
            };

            _context.Students.AddRange(students);
            _context.Courses.Add(new Course
            {
                CourseCode = "MAT",
                Title = "Matematyka",
                Instructor = "Michalina Beldzik",
                Schedule = "schedule1",
                Description = "des",
                Credits = 5,
                Department = "dep"
            });
            _context.SaveChanges();
        }

        [TestMethod]
        public void TestShowAllStudents()
        {
            var studentsViewModel = new StudentsViewModel(_context, _dialogService);
            Assert.IsTrue(studentsViewModel.Students.Any());
        }

        [TestMethod]
        public void TestAddStudentWithoutSubjects()
        {
            var addStudentViewModel = new AddStudentViewModel(_context, _dialogService)
            {
                Name = "Jacek",
                LastName = "Dawidson",
                PESEL = "00303136176",
                BirthDate = new DateTime(2000, 10, 31)
            };
            addStudentViewModel.Save.Execute(null);

            Assert.IsTrue(_context.Students.Any(s => s.Name == "Jacek" && s.LastName == "Dawidson" && s.PESEL == "00303136176"));
        }

        [TestMethod]
        public void TestAddStudentWithSubjects()
        {
            var random = new Random();
            var subject = _context.Courses.OrderBy(x => x.CourseCode).Skip(random.Next(0, _context.Courses.Count())).FirstOrDefault();
            subject.IsSelected = true;

            var addStudentViewModel = new AddStudentViewModel(_context, _dialogService)
            {
                Name = "Jacek",
                LastName = "Dawidson",
                PESEL = "00303136176",
                BirthDate = new DateTime(2000, 10, 30),
                AssignedCourses = new ObservableCollection<Course> { subject }
            };
            addStudentViewModel.Save.Execute(null);

            Assert.IsTrue(_context.Students.Any(s => s.Name == "Jacek" && s.LastName == "Dawidson" && s.PESEL == "00303136176" && s.Courses.Any()));
        }

        [TestMethod]
        public void TestAddStudentWithoutName()
        {
            var addStudentViewModel = new AddStudentViewModel(_context, _dialogService)
            {
                LastName = "Dawidson",
                PESEL = "00303136176",
                BirthDate = new DateTime(2000, 10, 30)
            };
            addStudentViewModel.Save.Execute(null);

            Assert.IsFalse(_context.Students.Any(s => s.LastName == "Dawidson" && s.PESEL == "00303136176"));
        }

        [TestMethod]
        public void TestAddStudentWithoutLastName()
        {
            var addStudentViewModel = new AddStudentViewModel(_context, _dialogService)
            {
                Name = "Jacek",
                PESEL = "00303136176",
                BirthDate = new DateTime(2000, 10, 30)
            };
            addStudentViewModel.Save.Execute(null);

            Assert.IsFalse(_context.Students.Any(s => s.Name == "Jacek" && s.PESEL == "00303136176"));
        }

        [TestMethod]
        public void TestAddStudentWithoutPESEL()
        {
            var addStudentViewModel = new AddStudentViewModel(_context, _dialogService)
            {
                Name = "Jacek",
                LastName = "Dawidson",
                BirthDate = new DateTime(2000, 10, 30)
            };
            addStudentViewModel.Save.Execute(null);

            Assert.IsFalse(_context.Students.Any(s => s.Name == "Jacek" && s.LastName == "Dawidson"));
        }
    }
}