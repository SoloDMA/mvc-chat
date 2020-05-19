using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Models;
using Chat.Models.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Components
{
    public class RoomMessagesViewComponent : ViewComponent
    {
        private ApplicationContext DataBase;
        public RoomMessagesViewComponent(ApplicationContext context)
        {
            DataBase = context;
        }

        public IViewComponentResult Invoke()
        {
            var currentRoomId = Request.Query["currentRoomId"].ToString();
            List<UserMessageModel> messagesList = null;

            if (currentRoomId != null && DataBase.ChatRooms.Find(currentRoomId) != null)
            {
                messagesList = new List<UserMessageModel>();
                foreach (var message in  DataBase.ChatMessages.ToList())
                {
                    if (message.RoomId != currentRoomId)
                        continue;

                    var sender = DataBase.Users.Find(message.SenderId);
                    messagesList.Add(new UserMessageModel { UserMessage = message, Sender = sender });
                }
                
                messagesList.Sort((x, y) =>
                {
                    var x_Date = DateTime.Parse(x.UserMessage.Time);
                    var y_Date = DateTime.Parse(y.UserMessage.Time);
                    return DateTime.Compare(x_Date, y_Date);
                });
            }

            return View("RoomMessages", messagesList);
        }
    }
}
