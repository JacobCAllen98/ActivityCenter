using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BeltExam.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Required(ErrorMessage="Name is required")]
        [MinLength(2, ErrorMessage="A Name must be atleast two characters")]
        public string Name {get;set;}

        [EmailAddress]
        [Required(ErrorMessage="An Email is required")]
        public string Email {get;set;}

        [DataType(DataType.Password)]
        [Required(ErrorMessage="Password is required")]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        [RegularExpression(@"^.*(?=.{8,})(?=.*[\d])(?=.*[\W]).*$", ErrorMessage="Must contain one letter, one number, and one special character")]
        public string Password {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        
        [NotMapped]
        [Compare("Password", ErrorMessage="Must match Password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}

        public List<Association> JoinedExercises{get;set;}

    }
    public class LoggedUser
    {
        public string LogEmail {get;set;}
        public string LogPassword {get;set;}
    } 
}