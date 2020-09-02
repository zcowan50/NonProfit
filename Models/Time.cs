using System;
using System.ComponentModel.DataAnnotations;

namespace NonProfit.Models
{
    public class Time
    {
        [Key]
        public int TimeId {get; set;}

        [Required(ErrorMessage = "is required")]
        [Range(1, 999, ErrorMessage="Must be between 1 to 999 hrs")]
        public int TimeSpent {get; set;}

        [Required(ErrorMessage = "is required")]
        [DataType(DataType.DateTime)]
        [Range(typeof(DateTime), "01/01/2020", "01/01/2099")]
        public DateTime TimeOnDate { get; set; }

        public string Notes {get; set;}

        public int VolunteerId {get; set;}
        public int StudentId {get; set;}

        public Student Student {get; set;}

        public Volunteer Mentor {get; set;}

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int UserId {get; set;}
        public User TimeEnteredBy {get; set;}

        
    }
}