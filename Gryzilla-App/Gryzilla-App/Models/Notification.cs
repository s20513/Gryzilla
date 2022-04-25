namespace Gryzilla_App.Models
{
    public partial class Notification
    {
        public int IdNotification { get; set; }
        public int IdUser { get; set; }
        public string Content { get; set; } = null!;
        public DateTime Date { get; set; }

        public virtual UserDatum IdUserNavigation { get; set; } = null!;
    }
}
