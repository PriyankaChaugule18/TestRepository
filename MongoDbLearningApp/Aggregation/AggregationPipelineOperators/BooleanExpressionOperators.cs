using MongoDB.Driver;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Linq;
using MongoDB.Bson;
using MongoDbLearningApp.CrudOperations;
using System;


namespace MongoDbLearningApp.Aggregation.AggregationPipelineOperators
{
    class BooleanExpressionOperators: SalesCollectionMongoDb
    {
        //$and
        [Test]
        public void Find_the_item_with_price_greater_than_1000_and_less_than_5000()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Price",1 },
                                {"Result", new BsonDocument
                                                   {
                                                       {
                                                           "$and", new BsonArray
                                                           {
                                                               new BsonDocument
                                                               {
                                                                   {
                                                                       "$gt", new BsonArray{"$Price",1000}
                                                                   }
                                                                   
                                                               }
                                                                   
                                                              ,
                                                               new BsonDocument
                                                               {
                                                                   {
                                                                       "$lt", new BsonArray{"$Price",5000}
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
            result.ForEach(x => Assert.AreEqual(x.Result,(x.Price>1000 && x.Price<5000)));
        }

        //$not
        [Test]
        public void Find_the_item_with_price_not_greater_than_2000()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Price", 1},
                                {"Result", new BsonDocument
                                                   {
                                    {
                                        "$not", new BsonArray
                                                           {
                                                               new BsonDocument
                                                               {
                                                                   {
                                                                       "$gt", new BsonArray{"$Price",2000}
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
            result.ForEach(x => Assert.AreEqual(x.Result, !(x.Price > 2000)));
        }

        //$or
        [Test]
        public void Find_the_item_with_price_greater_than_1500_or_less_than_1000()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Price", 1},
                                {"Result", new BsonDocument
                                                   {
                                                       {
                                                           "$or", new BsonArray
                                                           {
                                                               new BsonDocument
                                                               {
                                                                   {
                                                                       "$gt", new BsonArray{"$Price",1500}
                                                                   }

                                                               }

                                                              ,
                                                               new BsonDocument
                                                               {
                                                                   {
                                                                       "$lt", new BsonArray{"$Price",2000}
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
            result.ForEach(x => Assert.AreEqual(x.Result, (x.Price > 1500 || x.Price < 2000)));
        }


        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertSalesDetails(testData);
            salesCollection.InsertMany(documents);
        }
    }
}
