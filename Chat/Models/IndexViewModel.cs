using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Models
{
    public class IndexViewModel
    {
        public string CurrentRoomId { get; set; }
        public bool ErrorsExists { get; set; }
        public string ErrorDiscription { get; set; }
    }
}
