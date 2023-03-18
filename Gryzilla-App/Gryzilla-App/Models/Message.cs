using System;
using System.Collections.Generic;

namespace Gryzilla_App.Models
{
    public partial class Message
    {
        public Message()
        {
            GroupUserMessages = new HashSet<GroupUserMessage>();
        }

        public int IdMessage { get; set; }
        public string MessageText { get; set; } = null!;

        public virtual ICollection<GroupUserMessage> GroupUserMessages { get; set; }
    }
}
