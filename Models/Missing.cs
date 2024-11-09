using System.ComponentModel.DataAnnotations.Schema;
using WebCamServer.Models.Templates;

namespace WebCamServer.Models
{
  [Table("missing")]
  public class Missing : HistoryModel
  {
    public int Id { get; set; }
    public DateTime BirthDate { get; set; }
    public string FullName { get; set; }
    public double Size { get; set; }
    public string Description { get; set; }
    public string  LastSeenMap { get; set; }
    public bool Found { get; set; }
    public int SeekerId { get; set; }
    
    // PHOTOS
    public bool PhotosFront { get; set; }
    public bool PhotosLeft { get; set; }
    public bool PhotosRigth { get; set; }

    // todo: References
    public virtual Seeker Seeker { get; set; }
    public virtual ICollection<FoundVideo> FoundVideos { get; set; }
  }
}