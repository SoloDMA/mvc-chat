using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Chat.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Chat.Models.DataBase;

namespace Chat.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext DataBase;
        IWebHostEnvironment hostEnviroment;

        private string DefaultMenAvatarPath = "/images/default_avatar_man.png";
        private string DefaultWomenAvatarPath = "/images/default_avatar_woman.png";

        public AccountController(ApplicationContext context, IWebHostEnvironment env)
        {
            DataBase = context;
            hostEnviroment = env;
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string userName, string password, char sex)
        {
            var user = await DataBase.Users.FirstOrDefaultAsync(user => user.UserName == userName);

            if (user != null)
            {
                return View((object)"Аккаунт с таким именем пользователя уже существует! Введите другое имя пользователя.");
            }
            
            if (sex == '\0')
            {
                return View((object)"Вы не выбрали пол!");
            }
            byte[] avatar = null;
            if (sex == 'm')
            {
                avatar = GetAvatar(DefaultMenAvatarPath);
            }
            else if(sex == 'f')
            {
                avatar = GetAvatar(DefaultWomenAvatarPath);
            }
            

            var uid = Guid.NewGuid().ToString();
            DataBase.Users.Add(new UserModel
            { Id = uid, UserName = userName, PassWord = password, Sex = sex, Avatar = avatar, UserInfo = null, RegisterTime = DateTime.Now.ToString() });
            await DataBase.SaveChangesAsync();

            await Authenticate(userName);
            HttpContext.Response.Cookies.Append("_uid", uid);

            return RedirectToAction("Index", "Home");

        }

        private byte[] GetAvatar(string path)
        {
            using var reader = new BinaryReader(System.IO.File.Open(hostEnviroment.WebRootPath+path, FileMode.Open));
            var fileLen = (int)reader.BaseStream.Length;
            return reader.ReadBytes(fileLen);
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

            var id = new ClaimsIdentity(claims, "AppCookie", ClaimsIdentity.DefaultRoleClaimType, ClaimsIdentity.DefaultNameClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string userName, string password)
        {
            var user = await DataBase.Users.FirstOrDefaultAsync(user => user.UserName == userName && user.PassWord == password);

            if (user == null)
            {
                return View((object)"Не получилось войти в чат! Возможно вы ввели неверный логин или пароль.");
            }

            await Authenticate(userName);

            HttpContext.Response.Cookies.Append("_uid", user.Id);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LogIn", "Account");
        }

        public async Task<IActionResult> ProfileInfo()
        {
            var currentUserName = User.Claims.ToArray()[0].Value;
            var userEntity = await DataBase.Users.FirstOrDefaultAsync(user => user.UserName == currentUserName);
            return View(userEntity);
        }

        [HttpPost]
        public async Task<string> ChangeAvatar(IFormFile avatar)
        {

            if (avatar == null)
                return null;

            var currentUserName = User.Claims.ToArray()[0].Value;
            var userEntity = DataBase.Users.FirstOrDefault(user => user.UserName == currentUserName);
            byte[] newAvatar = null;
            using (var reader = new BinaryReader(avatar.OpenReadStream()))
            {
                newAvatar = reader.ReadBytes((int)avatar.Length);
            }

            userEntity.Avatar = newAvatar;

            DataBase.Users.Update(userEntity);
            await DataBase.SaveChangesAsync();

            string imgTag = $"<img style='width:100px; height:100px;' src=\"data: image / jpeg; base64,{Convert.ToBase64String(newAvatar)}\" />";

            return imgTag;
            
        }

        public async Task<IActionResult> ChangeProfile(string userPass, string userInfo)
        {

            var currentUserName = User.Claims.ToArray()[0].Value;
            var userEntity = DataBase.Users.FirstOrDefault(user => user.UserName == currentUserName);

            if(userPass == null && userInfo == null)
            {
                return RedirectToAction("ProfileInfo");
            }
            else if(userPass != null && userInfo != null)
            {
                userEntity.PassWord = userPass;
                userEntity.UserInfo = userInfo;
            }
            else if(userPass != null && userInfo == null)
            {
                userEntity.PassWord = userPass;
            }
            else
            {
                userEntity.UserInfo = userInfo;
            }
            

            DataBase.Users.Update(userEntity);
            await DataBase.SaveChangesAsync();

            
            return RedirectToAction("ProfileInfo");

        }
    }
}
