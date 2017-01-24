using Microsoft.AspNetCore.Mvc;
using message.Models;
using message.Factory;
namespace dashboard.Controllers{
    public class MessageController : Controller {
        private readonly MessageFactory MessageFactory;
        public MessageController(MessageFactory message) {
            MessageFactory = message;
        }
        [HttpPost]
        [Route("submit_message")]
        public IActionResult AddMessage(Messages item) {
            MessageFactory.Add(item);
            return RedirectToAction("Dashboard", "Home");
        }
    }
}