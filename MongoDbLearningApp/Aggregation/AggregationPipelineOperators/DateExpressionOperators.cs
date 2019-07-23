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
        //$dateFromParts
        [Test]
        public void Find_the_fasttrack_sing_bag_and_match_the_manufactured_datatime()
        {
            PrepareDatabase();
            var match = new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                            {
                                {"Name","Fastrack"},
                            }
                    }
                };
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"ManufacturingDate",1 },
                                {"ConvertedDateTime", new BsonDocument
                                                   {
                                                       {
                                                           "$dateFromParts", new BsonDocument
                                                           {
                                                               {
                                                                   "year", 2017
                                                               },
                                                               {
                                                                   "month" , 2
                                                               },
                                                               {
                                                                   "day",8
                                                               },
                                                               {
                                                                   "hour",12
                                                               }
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
