using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MongoDbLearningApp.Model
{
    public class AirTravel
    {
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public double TravelRating { get; set; }
        public string Phone { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Gender gender{get;set;}
        [BsonRepresentation(BsonType.String)]
        public List<FoodTypes> FoodPreferences{ get; set; }
        public List<TravelHistory> TravelHistory { get; set; }
        public List<TravelFrequency> TravelFrequency { get; set; }
        public List<string> SeatPreference { get; set; }
        public int IntegerResult { get; set; }
        public string[][] Beverages { get; set; }
        public object BsonResult { get; set; }
        public string[] ArrayResult { get; set; }
        public string Result { get; set; }
        public bool BooleanResult { get; set; }
    }

    public enum FoodTypes
    {
        Chinese,
        Indian_NonVeg,
        Indian_Veg
    }

    public enum Gender
    {
        Male, 
        Female
    }
}
