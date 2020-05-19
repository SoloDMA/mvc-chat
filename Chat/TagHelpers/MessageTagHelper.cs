using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Chat.TagHelpers
{
    public class MessageTagHelper : TagHelper
    {
        public bool CurrentUserMessage { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "message");
            if (CurrentUserMessage)
            {
                output.Attributes.SetAttribute("class", "message darker");
            }
        }
    }
}
