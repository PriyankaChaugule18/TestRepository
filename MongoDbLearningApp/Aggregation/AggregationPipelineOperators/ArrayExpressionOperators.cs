using MongoDB.Driver;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Linq;
using MongoDB.Bson;
using MongoDbLearningApp.CrudOperations;

namespace MongoDbLearningApp.Aggregation.AggregationPipelineOperators
{
    class ArrayExpressionOperators : AirTravelCollectionMongoDb
    {
        //
        [Test]
        public void Find_the_truncated_rating_of_all_items()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"FoodPreferences",1 },
                                {"Value", new BsonDocument
                                                   {
                                                       {
                                                           "$arrayElemAt", new BsonArray
                                                           {
                                                               "$FoodTypes",0
                                                           }
                                                       }
                                                   }}
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = travelCollection.Aggregate<AirTravel>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
        }

        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertTravelDetails(testData);
            travelCollection.InsertMany(documents);
        }
    }
}
