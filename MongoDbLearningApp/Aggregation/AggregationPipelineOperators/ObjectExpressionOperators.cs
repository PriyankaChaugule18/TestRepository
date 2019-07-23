using Mongo2Go;
using MongoDB.Driver;
using MongoDbLearningApp.CrudOperations;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Text;
using System.Linq;
using WonderTools.JsonSectionReader;
using MongoDB.Bson;

namespace MongoDbLearningApp.Aggregation.AggregationPipelineOperators
{
    class ObjectExpressionOperators : AirTravelCollectionMongoDb
    {
        //todo 
        //$mergeObjects

        //todo
        //objectToArray
        [Test]
        public void Find_the_comparision_of_fees_with_standard_fees_250()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Result", new BsonDocument
                                                   {
                                                       {
                                                           "$objectToArray","$travelFrequency"
                                                       },
                                                   }

                                }
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = travelCollection.Aggregate<AirTravel>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count(), 6);            

        }
        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertTravelDetails(testData);
            travelCollection.InsertMany(documents);
        }
    }
}
