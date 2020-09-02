using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NonProfit.Models
{
    public class Student
    {
        [Key]
        public int StudentId {get; set;}
        
        [Required(ErrorMessage = "is required")]
        [MinLength(2, ErrorMessage = "must be at least 2 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "is required")]
        [MinLength(2, ErrorMessage = "must be at least 2 characters")]
        [Display(Name = "Last Name")]
        public string LastName {get; set;}

        [Display(Name = "Facebook Name")]
        public string FacebookName {get; set;}

        [Display(Name = "Instagram Name")]
        public string InstagramName {get; set;}

        [Display(Name="Contact Number")]
        public string PhoneNumber{get;set;}

        [Display(Name="Living Situation")]
        public string LivingSituation {get; set;}

        [Display(Name="Father's First Name")]
        public string FatherFirstName {get; set;}
        [Display(Name="Father's Last Name")]
        public string FatherLastName {get; set;}

        [Display(Name="Father's Phone Number")]
        public string FatherPhoneNumber {get; set;}

        [Display(Name="Mother's First Name")]
        public string MotherFirstName {get; set;}

        [Display(Name="Mother's Last Name")]
        public string MotherLastName {get; set;}

        [Display(Name="Mother's Phone Number")]
        public string MotherPhoneNumber {get; set;}

        [Display(Name="Guardian's First Name")]
        public string GuardianFirstName {get; set;}

        [Display(Name="Guardian's Last Name")]
        public string GuardianLastName {get; set;}

        [Display(Name="Guardian's Phone Number")]
        public string GuardianPhoneNumber {get; set;}

        [Display(Name= "Address")]
        public string HomeAddress {get; set;}

        [Display(Name="High School")]
        public string HighSchool {get; set;}

        public string Grade {get; set;}

        public string Notes {get; set;}

        public bool Attendance {get; set;}
        public DateTime DayOfAttendance { get; set; } = DateTime.Now;

        public string Location {get; set;}
        public int Age {get; set;}

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int VolunteerId {get; set;}

        public Volunteer Mentor {get; set;}

        public List<Time> TimeWithMentor {get; set;}

        public int UserId {get; set;}

        public User CreatedStudent {get; set;}

    }
    
}