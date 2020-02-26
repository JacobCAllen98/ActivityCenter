using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace BeltExam.Models
{
    public class Association
    {
        [Key]
        public int AssociationId{get;set;}
        public int UserId{get;set;}
        public int ExerciseId{get;set;}
        public User User{get;set;}
        public Exercise Exercise{get;set;}
    }
}