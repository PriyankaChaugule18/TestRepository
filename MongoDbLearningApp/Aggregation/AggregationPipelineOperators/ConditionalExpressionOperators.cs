using MongoDB.Driver;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Linq;
using MongoDB.Bson;
using MongoDbLearningApp.CrudOperations;
using System;

namespace MongoDbLearningApp.Aggregation.AggregationPipelineOperators
{
    class ConditionalExpressionOperators : SalesCollectionMongoDb
    {
        //cond
        [Test]
        public void Find_the_price_greater_than_1500_project_fee_250_or_50()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Price",1 },
                                {"Fee", new BsonDocument
                                                   {
                                                       {
                                                           "$cond", new BsonArray
                                                           {
                                                                new BsonDocument
                                                                {
                                                                    {
                                                                        "$gt", new BsonArray
                                                                        {
                                                                            "$Price",1500
                                                                        }
                                                                    }
                                                                }, 250,50
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
            foreach (var res in result)
            {
                var value = res.Price > 1500 ? 250 : 50;
                Assert.AreEqual(res.Fee, value);
            }
        }

        //ifNull
        [Test]
        public void Find_the_product_with_null_item_description_project_as_unspecified()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Name",1 },
                                {"Item", new BsonDocument
                                                   {
                                                       {
                                                           "$ifNull", new BsonArray
                                                           {
                                                               "$Item", "Unspecified"
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
            Assert.AreEqual(result.ElementAt(1).Item, "Unspecified");
        }


        //Todo
        //switch

        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertSalesDetails(testData);
            salesCollection.InsertMany(documents);
        }
    }
}
