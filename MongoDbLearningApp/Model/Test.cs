using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbLearningApp.Model
{
    public class Test
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }

        public int Age { get; set; }

        public double Height { get; set; }
    }
}
