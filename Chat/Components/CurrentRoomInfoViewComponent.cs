using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Models.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Components
{
    public class CurrentRoomInfoViewComponent : ViewComponent
    {

        private ApplicationContext DataBase;
        public CurrentRoomInfoViewComponent(ApplicationContext context)
        {
            DataBase = context;
        }

        public IViewComponentResult Invoke()
        {
            var currentRoomId = Request.Query["currentRoomId"].ToString();
            var currentRoomEntity = DataBase.ChatRooms.FirstOrDefault(room => room.Id == currentRoomId);

            return View("CurrentRoomInfo", currentRoomEntity);
        }
    }
}
