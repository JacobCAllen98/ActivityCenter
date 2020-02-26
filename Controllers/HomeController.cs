using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeltExam.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BeltExam.Controllers
{
    public class HomeController : Controller
    {

        private MyContext context;
        public HomeController(MyContext dbContext)
        {
            context = dbContext;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            int? check = HttpContext.Session.GetInt32("CurrId");
            if(check == null)
            {
                return View();
            }
            return RedirectToAction("Dashboard");
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(context.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser,newUser.Password);
                context.Add(newUser);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        [HttpPost("check")]
        public IActionResult Check(LoggedUser Logged)
        {
            if(ModelState.IsValid)
            {
                var acc = context.Users.FirstOrDefault(a => a.Email == Logged.LogEmail);
                if(acc == null)
                {
                    ModelState.AddModelError("LogEmail", "Invalid Email/Password");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoggedUser>();
                var result = hasher.VerifyHashedPassword(Logged, acc.Password, Logged.LogPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("LogEmail", "Invalid Email/Password");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("CurrId", acc.UserId);
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            int? check = HttpContext.Session.GetInt32("CurrId");
            if(check == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.LogUser = context.Users.FirstOrDefault(a => a.UserId == check);
            ViewBag.BigList = context.Exercises.Include(d => d.Creator).Include(b => b.JoinedUsers).ThenInclude(c => c.User).OrderBy(g => g.StartDate).ThenBy(h => h.StartTime);
            return View();
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("exercise/logout")]
        public IActionResult LogoutOther()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("new")]
        public IActionResult NewExercise()
        {
            int? check = HttpContext.Session.GetInt32("CurrId");
            if(check == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.LogUser = context.Users.FirstOrDefault(a => a.UserId == check);
            return View();
        }

        [HttpGet("exercise/deleteExercise/{ExerId}")]
        public IActionResult DeleteExerciseOther(int ExerId)
        {
            int? check = HttpContext.Session.GetInt32("CurrId");
            if(check == null)
            {
                return RedirectToAction("Index");
            }
            User LogUser = context.Users.FirstOrDefault(a => a.UserId == check);
            Exercise toDeleteExer = context.Exercises.FirstOrDefault(a => a.ExerciseId == ExerId);
            if(toDeleteExer.Creator == LogUser){
                context.Remove(toDeleteExer);
                context.SaveChanges(); 
            }
            return RedirectToAction("Dashboard");
        }
        [HttpPost("createExercise")]
        public IActionResult createExercise(Exercise newExercise)
        {
            int? check = HttpContext.Session.GetInt32("CurrId");
            if(check == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.LogUser = context.Users.FirstOrDefault(a => a.UserId == check);
            int result = DateTime.Compare(newExercise.StartDate, DateTime.Today);
            if(ModelState.IsValid)
            {
                if(result < 0)
                {
                    ModelState.AddModelError("StartDate", "Invalid starting date");
                    return View("NewExercise");
                }
                else if(result == 0)
                {
                    if(newExercise.StartTime < DateTime.Now)
                    {
                        ModelState.AddModelError("StartTime", "Invalid starting time");
                        return View("NewExercise");
                    }
                }
                newExercise.Creator = context.Users.FirstOrDefault(a => a.UserId == check);
                context.Add(newExercise);
                context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("NewExercise");
        }

        [HttpGet("exercise/dashboard")]
        public IActionResult ugh()
        {
            return RedirectToAction("Dashboard");
        }

        [HttpGet("exercise/leave/{ExerId}")]
        public IActionResult LeaveExerciseOther(int ExerId)
        {
            int? check = HttpContext.Session.GetInt32("CurrId");
            if(check == null)
            {
                return RedirectToAction("Index");
            }
            User LogUser = context.Users.FirstOrDefault(a => a.UserId == check);
            Association breakAssociation = context.Associations.FirstOrDefault(b => b.ExerciseId == ExerId && b.UserId == LogUser.UserId);
            context.Remove(breakAssociation);
            context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("exercise/joinExercise/{ExerId}")]
        public IActionResult JoinExerciseOther(int ExerId)
        {
            int? check = HttpContext.Session.GetInt32("CurrId");
            if(check == null)
            {
                return RedirectToAction("Index");
            }
            User LogUser = context.Users.FirstOrDefault(a => a.UserId == check);
            Association newAssociation = new Association(){ UserId = LogUser.UserId, ExerciseId = ExerId};
            context.Add(newAssociation);
            context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("exercise/{ExerId}")]
        public IActionResult ViewExer(int ExerId)
        {
            int? check = HttpContext.Session.GetInt32("CurrId");
            if(check == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.LogUser = context.Users.FirstOrDefault(a => a.UserId == check);
            ViewBag.ViewExer = context.Exercises.Include(q => q.Creator).Include(c => c.JoinedUsers).ThenInclude(d => d.User).FirstOrDefault(b => b.ExerciseId == ExerId);
            return View();
        }

        [HttpGet("joinExercise/{ExerId}")]
        public IActionResult JoinExercise(int ExerId)
        {
            int? check = HttpContext.Session.GetInt32("CurrId");
            if(check == null)
            {
                return RedirectToAction("Index");
            }
            User LogUser = context.Users.FirstOrDefault(a => a.UserId == check);
            Association newAssociation = new Association(){ UserId = LogUser.UserId, ExerciseId = ExerId};
            context.Add(newAssociation);
            context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("leave/{ExerId}")]
        public IActionResult LeaveExercise(int ExerId)
        {
            int? check = HttpContext.Session.GetInt32("CurrId");
            if(check == null)
            {
                return RedirectToAction("Index");
            }
            User LogUser = context.Users.FirstOrDefault(a => a.UserId == check);
            Association breakAssociation = context.Associations.FirstOrDefault(b => b.ExerciseId == ExerId && b.UserId == LogUser.UserId);
            context.Remove(breakAssociation);
            context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        

        [HttpGet("deleteExercise/{ExerId}")]
        public IActionResult DeleteExercise(int ExerId)
        {
            int? check = HttpContext.Session.GetInt32("CurrId");
            if(check == null)
            {
                return RedirectToAction("Index");
            }
            User LogUser = context.Users.FirstOrDefault(a => a.UserId == check);
            Exercise toDeleteExer = context.Exercises.FirstOrDefault(a => a.ExerciseId == ExerId);
            if(toDeleteExer.Creator == LogUser){
                context.Remove(toDeleteExer);
                context.SaveChanges(); 
            }
            return RedirectToAction("Dashboard");
        }
    }
}