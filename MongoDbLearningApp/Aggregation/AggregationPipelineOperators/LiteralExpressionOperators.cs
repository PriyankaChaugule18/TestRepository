using MongoDB.Driver;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Linq;
using MongoDB.Bson;
using MongoDbLearningApp.CrudOperations;
using System;

namespace MongoDbLearningApp.Aggregation.AggregationPipelineOperators
{
    class LiteralExpressionOperator: SalesCollectionMongoDb
    {
        //$literal
        [Test]
        public void Find_the_products_which_are_bags()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Item",1 },
                                {"Result", new BsonDocument
                                                   {
                                                       {
                                                           "$eq", new BsonArray
                                                           {
                                                                "$Item", new BsonDocument
                                                                {
                                                                    {
                                                                        "$literal", "bag"
                                                                    }
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
            Assert.AreEqual(result.Count(), 5);
            result.ForEach(x => Assert.AreEqual(x.Result, 
                x.Item!=null && x.Item.Equals("bag")?true:false));
        }

        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertSalesDetails(testData);
            salesCollection.InsertMany(documents);
        }
    }
}
