using Microsoft.AspNetCore.SignalR;

namespace Back.Hubs
{
    public class NotificationsHub : Hub
    {
        public async Task JoinUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"notifications-{userId}");
        }

        public async Task LeaveUserGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"notifications-{userId}");
        }
    }
}
