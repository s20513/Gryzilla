using System;
using System.Collections.Generic;

namespace Gryzilla_App.Models
{
    public partial class GroupUserMessage
    {
        public int IdMessage { get; set; }
        public int? IdUser { get; set; }
        public int? IdGroup { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Message { get; set; } = null!;

        public virtual GroupUser? Id { get; set; }
    }
}
