using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace PadLabN2_BackEnd.Controllers
{
    public class MessageHub: Hub
    {
        public async Task Notify(string messageIn)
        {
            var messageOut = new 
            {
                message = messageIn
            };

            await Clients.All.SendAsync( "notify", messageOut );
        }
        
        public async Task AddMessage(string messageIn)
        {
            var messageOut = new 
            {
                body = messageIn
            };

            await Clients.All.SendAsync( "message", messageOut );
        }
    }
}