using Microsoft.AspNetCore.Mvc;
using comment.Factory;
using comment.Models;

namespace dashboard.Controllers{
    public class CommentController : Controller {
        private readonly CommentFactory CommentFactory;
        public CommentController(CommentFactory comment) {
            CommentFactory = comment;
        }
        [HttpPost]
        [Route("submit_comment")]
        public IActionResult AddComment(Comment item) {
            CommentFactory.Add(item);
            return RedirectToAction("Dashboard", "Home");
        }
    }
}