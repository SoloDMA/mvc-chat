using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Chat.TagHelpers
{
    public class AvatarTagHelper : TagHelper
    {
        //public string Src { get; set; }
        public bool Sender { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            output.TagMode = TagMode.StartTagOnly;
            output.Attributes.SetAttribute("alt", "Аватар пользователя");
            output.Attributes.SetAttribute("style", "width:100%;");
            if (Sender)
            {
                output.Attributes.SetAttribute("class", "right");
            }
        }
    }
}
