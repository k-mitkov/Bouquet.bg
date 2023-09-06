using Bouquet.CommunicationsApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bouquet.CommunicationsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IHubContext<MobileHub> _hubContext;

        public MessagesController(IHubContext<MobileHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessageToClient([FromBody] List<string> emails)
        {
            foreach(var email in emails)
            {
                try
                {
                    if (MobileHub.ConnectedClients.TryGetValue(email, out var connectionId))
                    {
                        await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", "New Order");
                    }
                }
                catch {
                    return BadRequest("Something went wrong.");
                }
            }
            return Ok("Messages sent successfully.");
        }
    }
}