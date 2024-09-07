using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WebCamServer.Models.Templates;

namespace WebCamServer.Models
{
  public class User : HistoryModel
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string DadFirstName { get; set; }
    public string MomFirstName { get; set; }
    public string DadLastName { get; set; }
    public string MomLastName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string City { get; set; }
    public string Password { get; set; } 
    public Guid PasswordSalt { get; set; } 
  }
}