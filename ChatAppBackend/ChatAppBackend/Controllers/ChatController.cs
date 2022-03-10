using ChatAppBackend.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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
        public async Task<IActionResult> SendMessage([FromBody] MessageDto message)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid messageDto");

            await _hubContext.Clients.All.SendAsync("SendMessage", message);
            message.Message += " Aha!";
            return Ok(message);
        }
    }
}
