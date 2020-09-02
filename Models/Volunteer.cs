using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NonProfit.Models
{
    public class Volunteer
    {
        [Key]
        public int VolunteerId {get; set;}

        [Required(ErrorMessage = "is required")]
        [MinLength(2, ErrorMessage = "must be at least 2 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "is required")]
        [MinLength(2, ErrorMessage = "must be at least 2 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "is required.")]
        [EmailAddress]
        public string Email {get; set;}

        [Display(Name="Contact Number")]
        public string PhoneNumber{get;set;}

        public string Location {get; set;}

        [Required(ErrorMessage = "is required.")]
        public string Role {get; set;}

        [Display(Name= "Address")]
        public string MailingAddress {get; set;}

        // public string ProfileImg {get; set;}
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<Student> Students {get; set;}

        public List<Time> TimeWithStudent {get; set;}

        public int UserId {get; set;}
        public User CreatedVolunteer {get; set;}

    }
}