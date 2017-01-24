using Microsoft.AspNetCore.Mvc;
using user.Factory;
using user.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using message.Factory;
using comment.Factory;

namespace dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserFactory UserFactory;
        private readonly MessageFactory MessageFactory;
        private readonly CommentFactory CommentFactory;

        public HomeController(UserFactory user, MessageFactory message, CommentFactory comment)
        {
            UserFactory = user;
            MessageFactory = message;
            CommentFactory = comment;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("signin")]
        public IActionResult SigninPage()

        {
            HttpContext.Session.Clear();
            return View("Signin");
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string email, string password)
        {
            HttpContext.Session.Clear();
            User thisuserattempt = UserFactory.Login(email);
            if (thisuserattempt != null && password != null)
            { //need to setup some sort of errors, for now just going to redirect to index
                var Hasher = new PasswordHasher<User>();
                if (0 != Hasher.VerifyHashedPassword(thisuserattempt, thisuserattempt.password, password))
                {
                    HttpContext.Session.SetInt32("user", thisuserattempt.id);
                    HttpContext.Session.SetInt32("admin", thisuserattempt.admin);
                    return RedirectToAction("Dashboard", thisuserattempt);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("register")]
        public IActionResult RegisterPage()
        {
            return View("Register");
        }
        [HttpPost]
        [Route("register")]
        public IActionResult RegisterPage(User user)
        {
            UserFactory.Add(user);
            User thisuser = UserFactory.Login(user.email);
            HttpContext.Session.SetInt32("user", thisuser.id);
            HttpContext.Session.SetInt32("admin", thisuser.admin);
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard(User thisuserattempt)
        {
            if (HttpContext.Session.GetInt32("user") != null)
            {
                ViewBag.allusers = UserFactory.AllUsers();
                ViewBag.user = UserFactory.FindUser(HttpContext.Session.GetInt32("user").Value);
                return View("Dashboard");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        [Route("/users/new")]
        public IActionResult AdminNewUser()
        {
            return View("AddUser");
        }
        [HttpPost]
        [RouteAttribute("add_user")]
        public IActionResult AdminNewUser(User user)
        {
            TryValidateModel(User);
            if (!ModelState.IsValid)
            {
                ViewBag.errors = ModelState.Values;
                return View("Register");
            }
            UserFactory.Add(user);
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [Route("/users/profile/{id}")]
        public IActionResult EditProfile(int id)
        {
            @ViewBag.thisuser = UserFactory.FindUser(id);
            return View("EditProfile");
        }
        [HttpPost]
        [Route("users/profile/update/{id}")]
        public IActionResult UpdateProfile(User user, int id)
        {
            UserFactory.UpdateUser(user, id);
            return RedirectToAction("Dashboard");
        }
        [HttpPost]
        [Route("users/edit/update/{id}")]
        public IActionResult EditProfile(User user, int id)
        {
            UserFactory.EditUser(user, id);
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [Route("/users/show/{id}")]
        public IActionResult ShowProfile(int id)
        {
            ViewBag.loggedin = HttpContext.Session.GetInt32("user").Value;
            ViewBag.allusers = UserFactory.AllUsers();
            ViewBag.comments = CommentFactory.AllComments();
            ViewBag.user = UserFactory.FindUser(id);
            ViewBag.users_wall = MessageFactory.Messages(id);
            return View("UserInformation");
        }
        [HttpGet]
        [Route("/users/edit/{id}")]
        public IActionResult EditUser(int id)
        {
            User thisuser = UserFactory.FindUser(id);
            ViewBag.thisuser = thisuser;
            return View("EditUser");
        }
        [HttpGet]
        [Route("/delete/{id}")]
        public IActionResult RemoveUser(int id)
        {
            UserFactory.RemoveUser(id);
            return RedirectToAction("Dashboard");
        }
    }
}
