using WebCamServer.Models.Templates;

namespace WebCamServer.Models
{
  public class Seeker : HistoryModel
  {
    public int Id { get; set; }

    // todo: References
    public int UserId { get; set; }

    public virtual User User { get; set; }
    public virtual ICollection<Contacts> Contacts { get; set; }
    public virtual ICollection<Missing> Missings { get; set; }
  }
}