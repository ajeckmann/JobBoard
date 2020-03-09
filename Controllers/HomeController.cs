using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamCSharp.Models;
using ExamCSharp.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace ExamCSharp.Controllers
{
    public class HomeController : Controller
    {
        private HomeContext dbContext;

        public HomeController (HomeContext context) 
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost ("register")]
            public IActionResult Register (User registree) {
                if (ModelState.IsValid)
                {
                    if (dbContext.Users.Any(u => u.Email == registree.Email)) 
                    {
                        ModelState.AddModelError ("Email", "Email Address Already in System. Sorry");
                        return View ("Index");
                    } 
                    else 
                    {
                        //hash the password
                        PasswordHasher<User> hash = new PasswordHasher<User> ();
                        registree.Password = hash.HashPassword (registree, registree.Password);
                        //add the registree to the db
                        dbContext.Users.Add (registree);

                        //save the datase
                        dbContext.SaveChanges ();
                        //go back to the login page and make them log in
                        return RedirectToAction ("Login");
                        //make them login again (first time)
                    }
                }
                else
                {
                    return View("Index");
                }

            }

            [HttpPost ("SignIn")]
            public IActionResult Signin (LoginUser loginner)
            //take in a loginner--a login user trying to log in from the LoginUser class
            {
                if (ModelState.IsValid) 
                {
                    User checkmatch = dbContext.Users.FirstOrDefault (u => u.Email == loginner.LoginEmail);
                    //declare a User from the db, call him checkmatch....
                    //check checkmatch (from the database )to see if he exists. Hope that's not null. If it is....
                    if (checkmatch == null)
                    {
                        ModelState.AddModelError ("LoginEmail", "Your Email or Password is invalid");
                        return View ("Login");
                        //add an error message and redirect back to login page

                    } 
                    else 
                    {
                        //check to see if passwords match
                        PasswordHasher<LoginUser> compare = new PasswordHasher<LoginUser> ();
                        var result = compare.VerifyHashedPassword (loginner, checkmatch.Password, loginner.LoginPassword);
                        if (result == 0) //this means that they don't match.
                        {
                            ModelState.AddModelError ("LoginEmail", "Your Email or Password is invalid");
                            return View ("Login");
                        } 
                        else 
                        {
                            HttpContext.Session.SetInt32 ("UserId", checkmatch.UserId);
                            //IF it works out, and the emails and psaswords match, store the checkmatch user's id in session (above);
                            return RedirectToAction ("Dashboard");
                            //this is where ya go when ya log in successfully. might need to change it
                        }

                    }

                }
                else
                {
                    return View("Login");
                }
                    
                
            }
                
                
                //query up a user with the ID "UserID" and store him in session

                [HttpGet ("login")]
                public IActionResult Login () 
                {
                    return View ();
                }

                [HttpGet("logout")]
                public IActionResult Logout()
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index");
                }
//#######END OF LOGIN###############END OF LOGIN##############END OF LOGIN##############END OF LOGIN
            private User LoggedIn() 
            {
                User LoggedIn= dbContext.Users.FirstOrDefault(u=>u.UserId==HttpContext.Session.GetInt32("UserId"));
                return LoggedIn;
            }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            User userinDb=LoggedIn();
            if (userinDb == null)
            {
                return RedirectToAction("Logout");
            }
           ViewBag.UserLoggedIn=userinDb;
           List<Funthing>FunthingList= dbContext.Funthings.Include(f=>f.Participants).ThenInclude(r=>r.Participant).Include(f=>f.Creator).ToList();
           return View(FunthingList);
           
        }
        public IActionResult CreateFunthing()
        {
           User userinDb=LoggedIn();
           if (userinDb==null)
           {
               return RedirectToAction("Logout");
           }
           //make sure the person who wants to creat a funthing is actually logged in!
           ViewBag.UserLoggedIn=userinDb;
           return View();
        }
        [HttpPost("AddFunthing")]
        public IActionResult AddFunthing(Funthing newFunthing)
        {
            User userinDb= LoggedIn();
            if (userinDb==null)
           {
               return RedirectToAction("Logout");
           }
           if(ModelState.IsValid)
           {
               dbContext.Funthings.Add(newFunthing);
               dbContext.SaveChanges();
               ViewBag.UserLoggedIn=userinDb;
               List<Funthing>FunthingList= dbContext.Funthings.Include(f=>f.Participants).ThenInclude(r=>r.Participant).Include(f=>f.Creator).ToList();
               return View("Dashboard", FunthingList);
           }
           else
           {
            ViewBag.UserLoggedIn=userinDb;
            return View("CreateFunthing");
           }
        }

        [HttpGet("response.{funthingId}/{userId}/{status}")]
        public IActionResult AddResponse(int funthingId, int userId, string status )
        {
            User userinDb=LoggedIn();
            if (userinDb==null)
            {
                return RedirectToAction("Logout");
            }
            if(status=="join")
            {
                Response newResponse= new Response();
                newResponse.FunthingId=funthingId;
                newResponse.UserId=userId;
                dbContext.Responses.Add(newResponse);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            if(status=="unjoin")
            {
                Response responsetoremove=dbContext.Responses.FirstOrDefault(f=>f.FunthingId==funthingId && f.UserId==userId);
                //find a response where both the user Id matches that of the user AND the funthing Id matches that of the funthing
                dbContext.Responses.Remove(responsetoremove);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                RedirectToAction("Logout");
            }
            return RedirectToAction("Dashboard");

        }
        [HttpGet("deletefunthing/{funthingId}")]
        public IActionResult DeleteFunthing(int funthingId)
        {
            Funthing funthingtoremove=dbContext.Funthings.FirstOrDefault(f=>f.FunthingId==funthingId);
            dbContext.Funthings.Remove(funthingtoremove);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("funthing/{funthingId}")]
        public IActionResult ViewFunthing(int funthingId)
        {
            User userinDb=LoggedIn();
            ViewBag.UserLoggedIn=userinDb;
            // ViewBag.Funthingtodisplay=dbContext.Funthings
            // .Include(f=>f.Participants)
            // .ThenInclude(r=>r.Participant)
            // .Include(f=>f.Creator)
            // .FirstOrDefault(f=>f.FunthingId==funthingId);
            Funthing Funthingtodisplay=dbContext.Funthings
            .Include(f=>f.Participants)
            .ThenInclude(r=>r.Participant)
            .Include(f=>f.Creator)
            .FirstOrDefault(f=>f.FunthingId==funthingId);
            return View(Funthingtodisplay);
           
        }

        public IActionResult Privacy()
        {
            return View();
        }

       


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
