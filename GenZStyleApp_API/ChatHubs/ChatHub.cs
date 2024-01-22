using GenZStyleApp.DAL.DBContext;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Users;
using Microsoft.AspNetCore.SignalR;

namespace GenZStyleApp_API.ChatHubs
{
    public class ChatHub : Hub
    {
        //#region Properties
        ///// <summary>
        ///// List of online users
        ///// </summary>
        //public readonly static List<GetUserResponse> _Connections = new List<GetUserResponse>();

        ///// <summary>
        ///// List of all users
        ///// </summary>
        //public readonly static List<GetUserResponse> _Users = new List<GetUserResponse>();

        ///// <summary>
        ///// List of available chat rooms
        ///// </summary>
        ////private readonly static List<RoomViewModel> _Rooms = new List<RoomViewModel>();

        ///// <summary>
        ///// Mapping SignalR connections to application users.
        ///// (We don't want to share connectionId)
        ///// </summary>
        //private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();
        //#endregion
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userid"].ToString();
            if (userId is not null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string sender, string receiver, string message)
        {
            try
            {
                var context = new GenZStyleDbContext();
                var time = DateTime.UtcNow.AddHours(7);
                context.Messages.Add(new Message
                {
                    Sender = Int32.Parse(sender),
                    Receiver = Int32.Parse(receiver),
                    Content = message,
                    CreateAt = time
                });

                await context.SaveChangesAsync();
                await Clients.Groups(sender, receiver).SendAsync("ReceiveMessage", sender, receiver, message, time);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
