using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public char Sex { get; set; }
        public string UserInfo { get; set; }
        public byte[] Avatar { get; set; }

        public string RegisterTime { get; set; }

    }
}
