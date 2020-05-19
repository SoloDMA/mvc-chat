using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Chat.Models;
using Chat.Models.DataBase;

namespace Chat.Components
{
    public class UserActivityViewComponent:ViewComponent
    {
        private ApplicationContext DataBase;
        public UserActivityViewComponent(ApplicationContext context)
        {
            DataBase = context;
        }

        public IViewComponentResult Invoke()
        {
            var currentUser = HttpContext.User.Claims.ToList()[0].Value;

            var userEntity = DataBase.Users.FirstOrDefault(user => user.UserName == currentUser);
            var registerDate = DateTime.Parse(userEntity.RegisterTime);

            var amountWrittedMessages = DataBase.ChatMessages.ToList().Where(message => message.SenderId == userEntity.Id).Count();

            return View("UserActivity", new UserActivityModel 
            { DaysInChat = (DateTime.Now - registerDate).Days, AmountWrittedMessages = amountWrittedMessages});
        }
    }
}
