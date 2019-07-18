using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDbLearningApp.Model
{
    public class Sales
    {
            [BsonRepresentation(BsonType.String)]
            public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
            public string Name { get; set; }
            public string Item { get; set; }
            public int Price { get; set; }
            public double Fee { get; set; }
            public double Rating { get; set; }
            public string ManufacturingDate { get; set; }    
        
        //TODO : Merge and keep one double value 
            public double PriceValues { get; set; }
            public double MathValues { get; set; }
            public bool Result { get; set; }
            public int IndexOfBytes { get; set; }
            public string[] ArrayValues { get; set; }
    }
}
