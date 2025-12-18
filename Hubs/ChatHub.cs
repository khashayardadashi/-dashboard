using Microsoft.AspNetCore.SignalR;

namespace persian_code.web.Hubs;

public class ChatHub :Hub
{
    public async Task SendMessage( string user,string message)
    {
       await Clients.All.SendAsync("RecieveMessage",user, message);
    }
}