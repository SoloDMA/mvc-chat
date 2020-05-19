using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Models
{
    public class RoomModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CreatorName { get; set; }
        public string TimeOfCreation { get; set; }
    }
}
