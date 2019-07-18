using MongoDB.Driver;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Linq;
using MongoDB.Bson;
using MongoDbLearningApp.CrudOperations;
using System;

namespace MongoDbLearningApp.Aggregation.AggregationPipelineOperators
{
    class ComparisonExpressionOperators : SalesCollectionMongoDb
    {
        //cmp
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
                                {"Fee",1 },
                                {"MathValues", new BsonDocument
                                                   {
                                                       {
                                                           "$cmp", new BsonArray
                                                           {
                                                                "$Fee",250
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
            foreach(var res in result)
            {
                var value = res.Fee > 250 ? 1 : -1;
                Assert.AreEqual(res.MathValues, value);
            }
           
        }

        //eq
        [Test]
        public void Find_the_fees_equal_to_25()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Fee",1 },
                                {"Result", new BsonDocument
                                                   {
                                                       {
                                                           "$eq", new BsonArray
                                                           {
                                                                "$Fee",25
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
                var value = res.Fee == 25 ? true : false;
                Assert.AreEqual(res.Result, value);
            }

       }

        //gt
        [Test]
        public void Find_the_fees_greater_than_1000()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Fee",1 },
                                {"Result", new BsonDocument
                                                   {
                                                       {
                                                           "$gt", new BsonArray
                                                           {
                                                                "$Fee",1000
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
            result.ForEach(x => Assert.AreEqual(x.Result, x.Fee > 1000));

        }

        //gte
        [Test]
        public void Find_the_fees_greater_than_eq_1000()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Fee",1 },
                                {"Result", new BsonDocument
                                                   {
                                                       {
                                                           "$gte", new BsonArray
                                                           {
                                                                "$Fee",1000
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
            result.ForEach(x => Assert.AreEqual(x.Result, x.Fee >= 1000));

        }

        //lt
        [Test]
        public void Find_the_price_less_than_5000()
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
                                                           "$lt", new BsonArray
                                                           {
                                                                "$Price",5000
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
            result.ForEach(x => Assert.AreEqual(x.Result, x.Price < 5000));
        }

        //lte
        [Test]
        public void Find_the_price_less_than_equal_to_1500()
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
                                                           "$lte", new BsonArray
                                                           {
                                                                "$Price",1500
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
            result.ForEach(x => Assert.AreEqual(x.Result, x.Price <= 1500));
        }

        //ne
        [Test]
        public void Find_the_price_not_equal_to_1500()
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
                                                           "$ne", new BsonArray
                                                           {
                                                                "$Price",130000
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
            result.ForEach(x => Assert.AreEqual(x.Result, x.Price != 130000));
        }

        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertSalesDetails(testData);
            salesCollection.InsertMany(documents);
        }
    }
}
