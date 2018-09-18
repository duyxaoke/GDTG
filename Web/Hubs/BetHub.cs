using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRAngularDemo.Hubs
{
    [HubName("bet")]
    public class BetHub : Hub { }
}