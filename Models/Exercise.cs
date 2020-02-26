using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BeltExam.Models
{
    public class Exercise
    {
        [Key]
        public int ExerciseId {get;set;}

        [Required(ErrorMessage="A title is required")]
        public string Title {get;set;}

        [Required(ErrorMessage="A description is required")]
        public string Desc {get;set;}

        [Required(ErrorMessage="Date is required")]
        public DateTime StartDate {get;set;}

        [Required(ErrorMessage="Start time is required")]
        public DateTime StartTime {get;set;}

        [Required(ErrorMessage="A duration is required")]
        public int Duration{get;set;}

        [Required]
        public string DurationType{get;set;}

        public User Creator{get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<Association> JoinedUsers {get;set;}
    }
}