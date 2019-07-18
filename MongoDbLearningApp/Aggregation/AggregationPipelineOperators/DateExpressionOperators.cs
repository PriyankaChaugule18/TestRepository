using MongoDB.Driver;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Linq;
using MongoDB.Bson;
using MongoDbLearningApp.CrudOperations;
using System;

namespace MongoDbLearningApp.Aggregation.AggregationPipelineOperators
{
    class DateExpressionOperators :SalesCollectionMongoDb
    {
        [Test]
        public void Find_the_dates_for_a_product()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"ManufacturingDate",1 },
                                {"MathValues", new BsonDocument
                                                   {
                                                       {
                                                           "$dateFromString", new BsonDocument
                                                           {
                                                               //{
                                                               //  dateString, new BsonDocument{ '$date' },
                                                               //  timezone,new BsonDocument{"America/New_York"}
                                                               //}
                                                           }
                                                       }
                                                   }
                            }
                    }
                }
                };

            var pipeline = new[] { project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
            result.ForEach(x => Assert.AreEqual(x.MathValues, Math.Truncate(x.Rating)));
        }
        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertSalesDetails(testData);
            salesCollection.InsertMany(documents);
        }
    }
}
