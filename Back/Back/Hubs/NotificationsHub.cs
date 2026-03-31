using Microsoft.AspNetCore.SignalR;

namespace Back.Hubs
{
    public class NotificationsHub : Hub
    {
        public async Task JoinUserGroup(string userId)
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"notifications-{userId}");   
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task LeaveUserGroup(string userId)
        {
            try
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"notifications-{userId}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
