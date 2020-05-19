using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Models.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Components
{
    public class UserViewComponent : ViewComponent
    {
        private ApplicationContext Database;
        public UserViewComponent(ApplicationContext context)
        {
            Database = context;
        }

        public IViewComponentResult Invoke()
        {
            var currentUser = HttpContext.User.Claims.ToList()[0].Value;

            var userEntity = Database.Users.FirstOrDefault(user => user.UserName == currentUser);

            return View("User", userEntity);
        }
    }
}
