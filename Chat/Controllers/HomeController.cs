using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Chat.Models;
using ApplContext = Chat.Models.DataBase.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace Chat.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplContext DataBase;
        public HomeController(ILogger<HomeController> logger, ApplContext context)
        {
            _logger = logger;
            DataBase = context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index(string currentRoomId = null)
        {

            return View(new IndexViewModel { CurrentRoomId = currentRoomId, ErrorDiscription = null, ErrorsExists = false });
        }

        [HttpPost]
        public async Task<IActionResult> CreateChatRoom(string roomName)
        {
            var userEntity = await DataBase.Users.FirstOrDefaultAsync(user => user.Id == HttpContext.Request.Cookies["_uid"]);
            var room = new RoomModel { Id = Guid.NewGuid().ToString(), CreatorName = userEntity.UserName, Name = roomName, TimeOfCreation = DateTime.Now.ToString() };
            var roomCreator = new RoomMemberModel { RoomId = room.Id, UserId = userEntity.Id };
            DataBase.ChatRooms.Add(room);
            DataBase.RoomMembers.Add(roomCreator);
            await DataBase.SaveChangesAsync();
            return RedirectToAction("Index", "Home", new { currentRoomId = room.Id });
        }

        [HttpPost]
        public async Task<IActionResult> AddUserInRoom(string userName, string roomId)
        {
            var userEntity = await DataBase.Users.FirstOrDefaultAsync(user => user.UserName == userName);

            if (userEntity == null)
            {
                return View("Index", new IndexViewModel { CurrentRoomId = roomId, ErrorsExists = true, ErrorDiscription = "Такого пользователя не существует!" });
            }

            var roomMemberEntity = await DataBase.RoomMembers
                .FirstOrDefaultAsync(roomMember => roomMember.UserId == userEntity.Id && roomMember.RoomId == roomId);
            if (roomMemberEntity != null)
            {
                return View("Index", new IndexViewModel { CurrentRoomId = roomId, ErrorsExists = true, ErrorDiscription = "Этот пользователь уже добавлен!" });
            }

            var roomMember = new RoomMemberModel { RoomId = roomId, UserId = userEntity.Id };
            DataBase.RoomMembers.Add(roomMember);
            await DataBase.SaveChangesAsync();

            return RedirectToAction("Index", "Home", new { currentRoomId = roomId });
        }

        [HttpPost]
        public IActionResult AddMessage(string messageText, string roomId)
        {
            if (roomId == null)
                return RedirectToAction("Index", "Home", new { currentRoomId = roomId });

            DataBase.ChatMessages.Add(new MessageModel
            { Id = Guid.NewGuid().ToString(), RoomId = roomId, SenderId = Request.Cookies["_uid"], Text = messageText, Time = DateTime.Now.ToString() });
            DataBase.SaveChanges();
            return RedirectToAction("Index", "Home", new { currentRoomId = roomId });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMessage(string messageId, string roomId)
        {
            var deletedMessage = await DataBase.ChatMessages.FindAsync(messageId);
            DataBase.ChatMessages.Remove(deletedMessage);
            DataBase.SaveChanges();

            return RedirectToAction("Index", "Home", new { currentRoomId = roomId });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRoom(string currentRoomId)
        {
            var deletedRoom = await DataBase.ChatRooms.FindAsync(currentRoomId);
            var currentRoomMembers = DataBase.RoomMembers.ToListAsync().Result.Where(roomMember => roomMember.RoomId == currentRoomId);
            DataBase.RoomMembers.RemoveRange(currentRoomMembers);
            DataBase.ChatRooms.Remove(deletedRoom);
            DataBase.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> LogOutFromRoom()
        {
            var currentUserEntity = await DataBase.Users.FirstOrDefaultAsync(user => user.UserName == User.Claims.ToList()[0].Value);
            var deletedRoomMember = await DataBase.RoomMembers.FirstOrDefaultAsync(roomMember => roomMember.UserId == currentUserEntity.Id);
            DataBase.RoomMembers.Remove(deletedRoomMember);
            await DataBase.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult MessagesSearch(string searchKey, string currentRoomId)
        {
            IEnumerable<MessageModel> foundMessages;
            if (searchKey != null)
            {
                 foundMessages = DataBase.ChatMessages.ToList()
                .Where(message => message.Text.ToLower().Contains(searchKey.ToLower()) && message.RoomId == currentRoomId);
            }
            else
            {
                foundMessages = DataBase.ChatMessages.ToList()
                .Where(message => message.RoomId == currentRoomId);
            }

            var messagesList = new List<UserMessageModel>();

            foreach (var message in foundMessages)
            {
                var sender = DataBase.Users.Find(message.SenderId);
                messagesList.Add(new UserMessageModel { UserMessage = message, Sender = sender });
            }

            messagesList.Sort((x, y) =>
            {
                var x_Date = DateTime.Parse(x.UserMessage.Time);
                var y_Date = DateTime.Parse(y.UserMessage.Time);
                return DateTime.Compare(x_Date, y_Date);
            });


            return View("MessagesSearch", messagesList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
