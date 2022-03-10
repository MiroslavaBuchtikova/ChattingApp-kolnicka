using ChatAppBackend.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private IHubContext<ChatHub> _hubContext;

        public ChatController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("[action]")]
        public IActionResult Test()
        {
            return Ok("Funguje");
        }

        [HttpPost]
        public IActionResult SendMessage(ChatMessage message)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid message");

            _hubContext.Clients.All.SendAsync("SendMessage", message);
            message.Message += " Aha!";
            return Ok(message);
        }
    }
}
