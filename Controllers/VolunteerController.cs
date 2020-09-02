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
    public class VolunteerController : Controller
    {
        private NonProfitContext db;

        public VolunteerController(NonProfitContext context)
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
        
        [HttpGet("volunteer/new")]
        public IActionResult NewVolunteer()
        {
            if(activeUser == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("NewVolunteer");
        }

        [HttpPost("volunteer/new")]
        public IActionResult CreateVolunteer(Volunteer NewVolunteer)
        {
            if(ModelState.IsValid == false)
            {
                return View("NewVolunteer");
            }
            NewVolunteer.UserId = (int)activeUser; 
            db.Volunteers.Add(NewVolunteer);
            db.SaveChanges();
            // return RedirectToAction("Dashboard", "Home");
            return RedirectToAction("VolunteerDetails", new {volunteerId = NewVolunteer.VolunteerId});
        }

        [HttpGet("volunteers")]
        public IActionResult Volunteers()
        {
            List<Volunteer> allVolunteers = db.Volunteers.Include(vol => vol.Students).Include(vol => vol.TimeWithStudent).OrderBy(vol => vol.LastName).ToList();
            return View("Volunteers", allVolunteers);
        }

        [HttpGet("volunteer/{volunteerId}")]
        public IActionResult VolunteerDetails(int volunteerId)
        {
            if(activeUser == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            ViewBag.SingleVolunteer = db.Volunteers.Include(vol => vol.Students).ThenInclude(student => student.TimeWithMentor).ThenInclude(time => time.Mentor).FirstOrDefault(vol => vol.VolunteerId == volunteerId);
            ViewBag.Students = db.Students.Include(vol => vol.Mentor).ThenInclude(stu => stu.Students).Where(vol => vol.Mentor.VolunteerId != volunteerId).OrderBy(vol => vol.FirstName).ToList();
            if(ViewBag.SingleVolunteer == null)
            {
                return RedirectToAction("Volunteers");
            }
            return View("VolunteerDetails");
        }
        [HttpPost("volunteer/{volunteerId}/delete")]
        public IActionResult Delete(int volunteerId)
        {

            Volunteer AssignedStudents = db.Volunteers.Include(vol => vol.Students).FirstOrDefault(vol => vol.VolunteerId == volunteerId);
            Volunteer dbVolunteer = db.Volunteers.FirstOrDefault(vol => vol.VolunteerId == volunteerId);
            if(AssignedStudents.Students.Count > 0)
            {
                foreach(Student student in AssignedStudents.Students)
                {
                    student.VolunteerId = 4;
                }
            }
            db.Volunteers.Remove(dbVolunteer);
            db.SaveChanges();
            return RedirectToAction("Volunteers");
        }

        [HttpPost("volunteer/{volunteerId}")]
        public IActionResult AssignStudent(int studentId, int volunteerId)
        {
            Student DbSelectedStudent = db.Students.FirstOrDefault(student => student.StudentId == studentId);
            DbSelectedStudent.VolunteerId = volunteerId;
            db.Students.Update(DbSelectedStudent);
            db.SaveChanges();
            return RedirectToAction("VolunteerDetails", volunteerId);
        }

        [HttpPost("volunteer/{volunteerId}/{studentId}")]
        public IActionResult AddTime(int volunteerId, int studentId, Time newTime)
        {
            if(ModelState.IsValid == false)
            {
                return View("VolunteerDetails", volunteerId);
            }
            newTime.UserId = (int)activeUser;
            newTime.VolunteerId = volunteerId;
            newTime.StudentId = studentId;
            db.Times.Add(newTime);
            db.SaveChanges();
            return RedirectToAction("VolunteerDetails", volunteerId);
        }

        [HttpGet("volunteer/{volunteerId}/edit")]
        public IActionResult Edit(int volunteerId)
        {
            if(activeUser == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Volunteer Volunteer = db.Volunteers.FirstOrDefault(vol => vol.VolunteerId == volunteerId);
            if(Volunteer == null)
            {
                return RedirectToAction("Volunteers");
            }
            return View("Edit", Volunteer );
        }

        [HttpPost("/volunteer/{volunteerId}/update")]
        public IActionResult Update(Volunteer editedVolunteer, int volunteerId)
        {
            if(ModelState.IsValid == false)
            {
                // return the view to display validation errors
                // editedTruck has the Truck id in it, so we have to pass
                // it back in because of asp-route-truckId="@Model.TruckId"
                // Technically we could remove that asp-route from the form and NOT pass in the editedTruck
                // below and it will be handled automatically
                return View("Edit", editedVolunteer);
            }

            Volunteer dbVolunteer = db.Volunteers.FirstOrDefault(t => t.VolunteerId == volunteerId);
            if(dbVolunteer == null)
            {
                return RedirectToAction("Volunteers");
            }

            dbVolunteer.FirstName = editedVolunteer.FirstName;
            dbVolunteer.LastName = editedVolunteer.LastName;
            dbVolunteer.Email = editedVolunteer.Email;
            dbVolunteer.PhoneNumber = editedVolunteer.PhoneNumber;
            dbVolunteer.Location = editedVolunteer.Location;
            dbVolunteer.Role = editedVolunteer.Role;
            dbVolunteer.MailingAddress = editedVolunteer.MailingAddress;
            dbVolunteer.UpdatedAt = DateTime.Now;

            db.Volunteers.Update(dbVolunteer);
            db.SaveChanges();
            return RedirectToAction("Volunteers");
        }


    }
}