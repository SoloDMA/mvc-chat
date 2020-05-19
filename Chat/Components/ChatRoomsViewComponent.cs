using Chat.Models.DataBase;
using Chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chat.Components
{
    public class ChatRoomsViewComponent : ViewComponent
    {
        private ApplicationContext DataBase;

        public ChatRoomsViewComponent(ApplicationContext context)
        {
            DataBase = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roomEntities = DataBase.ChatRooms.ToList();
            var currentUserRooms = new List<RoomModel>();
            if (roomEntities != null)
            {

                var member = DataBase.RoomMembers.ToList();

                var rooms = from room in roomEntities
                                       join roomMember in member on room.Id equals roomMember.RoomId
                                       where roomMember.UserId == Request.Cookies["_uid"]
                                       select new RoomModel { Id = room.Id, Name = room.Name, CreatorName = room.CreatorName };

                foreach (var room in rooms)
                {
                    currentUserRooms.Add(room);
                }
            }

            return View("ChatRooms", currentUserRooms);
        }
    }
}
