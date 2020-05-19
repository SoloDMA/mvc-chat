using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Models
{
    public class MessageModel
    {
        public string Id { get; set; }
        public string RoomId { get; set; }
        public string SenderId { get; set; }
        public string Text { get; set; }
        public string Time { get; set; }
    }
}
