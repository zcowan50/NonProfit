using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NonProfit.Models;

namespace NonProfit.Controllers
{
    public class StudentController : Controller
    {
        private NonProfitContext db;
        public StudentController(NonProfitContext context)
        {
            db = context;
        }
        private int? activeUser
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId");
            }
        }

        [HttpGet("student/new")]
        public IActionResult NewStudent()
        {
            if(activeUser == null)
            {
                return RedirectToAction("Index","Home");
            }
            return View("NewStudent");
        }

        [HttpPost("student/new")]
        public IActionResult CreateStudent(Student NewStudent)
        {
            if(ModelState.IsValid == false)
            {
                return View("NewStudent");
            }
            NewStudent.UserId = (int)activeUser;
            NewStudent.VolunteerId = 4;
            db.Students.Add(NewStudent);
            db.SaveChanges();
            return RedirectToAction("StudentDetails", new {studentId = NewStudent.StudentId});
        }
        [HttpGet("students")]
        public IActionResult Students()
        {
            List<Student> allStudents = db.Students.Include(stu=> stu.Mentor).OrderBy(vol => vol.LastName).ToList();
            return View("Students", allStudents);
        }

        [HttpGet("student/{studentId}")]
        public IActionResult StudentDetails(int studentId)
        {
            Student singleStudent = db.Students.Include(stu => stu.Mentor).Include(stu => stu.TimeWithMentor).ThenInclude(time => time.Mentor).FirstOrDefault(stu => stu.StudentId == studentId);
            ViewBag.Volunteers = db.Volunteers.Include(vol => vol.Students).ThenInclude(stu => stu.Mentor).Where(vol => vol.Students.Any(student => student.Mentor.VolunteerId == singleStudent.VolunteerId) == false).OrderBy(vol => vol.FirstName).ToList();
            if(singleStudent == null)
            {
                return RedirectToAction("Students");
            }
            return View("StudentDetails", singleStudent);
        }

        [HttpGet("student/unassigned")]
        public IActionResult UnassignedStudents()
        {
            List<Student> allStudents = db.Students.Include(stu=> stu.Mentor).OrderBy(vol => vol.LastName).ToList();
            ViewBag.Volunteers = db.Volunteers.Include(vol => vol.Students).ThenInclude(stu => stu.Mentor).Where(vol => vol.Students.Any(student => student.Mentor.VolunteerId == 4 )== false).OrderBy(vol => vol.LastName).ToList();
            return View("UnassignedStudents", allStudents);
        }

        [HttpPost("student/{studentId}")]
        public IActionResult AssignVolunteer(int studentId, int volunteerId)
        {
            Student DbSelectedStudent = db.Students.FirstOrDefault(student => student.StudentId == studentId);
            DbSelectedStudent.VolunteerId = volunteerId;
            db.Students.Update(DbSelectedStudent);
            db.SaveChanges();
            if(volunteerId == 4)
            {
            return RedirectToAction("UnassignedStudents");
            }
            return RedirectToAction("Students");
        }

        [HttpGet("student/{studentId}/edit")]
        public IActionResult Edit(int studentId)
        {
            if(activeUser == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Student Student = db.Students.FirstOrDefault(vol => vol.StudentId == studentId);
            if(Student == null)
            {
                return RedirectToAction("Volunteers");
            }
            return View("Edit", Student);
        }

        [HttpPost("/student/{studentId}/update")]
        public IActionResult Update(Student editedStudent, int studentId)
        {
            if(ModelState.IsValid == false)
            {
                // return the view to display validation errors
                // editedTruck has the Truck id in it, so we have to pass
                // it back in because of asp-route-truckId="@Model.TruckId"
                // Technically we could remove that asp-route from the form and NOT pass in the editedTruck
                // below and it will be handled automatically
                return View("Edit", editedStudent);
            }

            Student dbStudent = db.Students.FirstOrDefault(t => t.StudentId == studentId);
            if(dbStudent == null)
            {
                return RedirectToAction("Students");
            }

            dbStudent.FirstName = editedStudent.FirstName;
            dbStudent.LastName = editedStudent.LastName;
            dbStudent.Age = editedStudent.Age;
            dbStudent.PhoneNumber = editedStudent.PhoneNumber;
            dbStudent.HighSchool = editedStudent.HighSchool;
            dbStudent.Grade = editedStudent.Grade;
            dbStudent.FacebookName = editedStudent.FacebookName;
            dbStudent.InstagramName = editedStudent.InstagramName;
            dbStudent.LivingSituation = editedStudent.LivingSituation;
            dbStudent.FatherFirstName = editedStudent.FatherFirstName;
            dbStudent.FatherLastName = editedStudent.FatherLastName;
            dbStudent.FatherPhoneNumber = editedStudent.FatherPhoneNumber;
            dbStudent.MotherFirstName = editedStudent.MotherFirstName;
            dbStudent.MotherLastName = editedStudent.MotherLastName;
            dbStudent.MotherPhoneNumber = editedStudent.MotherPhoneNumber;
            dbStudent.GuardianFirstName = editedStudent.GuardianFirstName;
            dbStudent.GuardianLastName = editedStudent.GuardianLastName;
            dbStudent.GuardianPhoneNumber = editedStudent.GuardianPhoneNumber;
            dbStudent.HomeAddress = editedStudent.HomeAddress;
            dbStudent.Location = editedStudent.Location;
            dbStudent.Notes = editedStudent.Notes;
            dbStudent.UpdatedAt = DateTime.Now;

            db.Students.Update(dbStudent);
            db.SaveChanges();
            return RedirectToAction("StudentDetails", dbStudent);
        }

        [HttpPost("student/{studentId}/delete")]
        public IActionResult Delete(int studentId)
        {
            Student dbStudent = db.Students.FirstOrDefault(vol => vol.StudentId == studentId);
            
            db.Students.Remove(dbStudent);
            db.SaveChanges();
            return RedirectToAction("Students");
        }
    }
}