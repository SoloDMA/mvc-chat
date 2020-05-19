using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Models
{
    public class UserMessageModel
    {
        public MessageModel UserMessage { get; set; }
        public UserModel Sender { get; set; }
    }
}
