
namespace Entities
{
    public class MBaseEntity
    {
        // [BsonId] // point out "Id" is key in table
        // [BsonRepresentation(BsonType.ObjectId)] // "Id" datatype is "ObjectId"
        public string? Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
