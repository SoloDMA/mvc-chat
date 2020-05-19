using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Models
{
    public class RoomMemberModel
    {
        public int Id { get; set; }
        public string RoomId { get; set; }
        public string UserId { get; set; }
    }
}
