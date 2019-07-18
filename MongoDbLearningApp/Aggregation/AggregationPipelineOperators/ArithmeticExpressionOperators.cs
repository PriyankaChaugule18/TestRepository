using MongoDB.Driver;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Linq;
using MongoDB.Bson;
using MongoDbLearningApp.CrudOperations;
using System;

namespace MongoDbLearningApp.Aggregation.AggregationPipelineOperators
{
    class ArithmeticExpressionOperators: SalesCollectionMongoDb
    { 
        //abs
        [Test]
        public void Find_the_absolute_value_of_ratings_compared_to_5_of_all_items()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Rating", new BsonDocument
                                                   {
                                                       {
                                                           "$abs", new BsonDocument
                                                           {
                                                                {
                                                                    "$subtract", new BsonArray
                                                                    {
                                                                        "$Rating", 5
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
            result.ForEach(x => Assert.Greater(x.Rating, 0));
        }
        
        //add
        [Test]
        public void Find_the_total_price_for_each_item()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                            {"Price", 1},
                            {"Fee", 1},
                            {"PriceValues", new BsonDocument
                                                   {
                                                       {
                                                           "$add", new BsonArray
                                                           {
                                                               "$Price", "$Fee"
                                                           }
                                                       }
                                                   }}
                            }
                    }
                };
            
            var pipeline = new[] { project};
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
            result.ForEach(x => Assert.AreEqual(x.PriceValues, x.Price+x.Fee));            
        }

        //ceil
        [Test]
        public void Find_the_round_off_value_of_rating_of_all_items()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Rating", new BsonDocument
                                                   {
                                                       {
                                                           "$ceil", new BsonArray
                                                           {
                                                               "$Rating"
                                                           }
                                                       }
                                                   }}
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
            Assert.AreEqual(result.FirstOrDefault().Rating, 3);
        }

        
        //divide
        [Test]
        public void Find_the_half_price_discount_of_5_item()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Price",1 },
                                {"PriceValues", new BsonDocument
                                                   {
                                                       {
                                                           "$divide", new BsonArray
                                                           {
                                                               "$Price",2
                                                           }
                                                       }
                                                   }}
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
            result.ForEach(x => Assert.AreEqual(x.PriceValues, x.Price / 2));
        }
        
        //exp
        [Test]
        public void Find_the_exponent_of_rating_of_all_items()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Rating",1 },
                                {"MathValues", new BsonDocument
                                                   {
                                                       {
                                                           "$subtract", new BsonArray
                                                                    {
                                                                        new BsonDocument
                                                                        { {"$exp","$Rating" } }, 2
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
            result.ForEach(x => Assert.AreEqual(x.MathValues, System.Math.Exp(x.Rating) - 2));
        }

        //floor
        [Test]
        public void Find_the_rating_of_all_passengers()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Rating", new BsonDocument
                                                   {
                                                       {
                                                           "$floor", new BsonArray
                                                           {
                                                               "$Rating"
                                                           }
                                                       }
                                                   }}
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
            Assert.AreEqual(result.FirstOrDefault().Rating, 2);
        }

        //ln(log)
        [Test]
        public void Find_the_natural_log_of_rating_of_all_items()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Rating",1 },
                                {"MathValues", new BsonDocument
                                                   {
                                                       {
                                                           "$ln", new BsonArray
                                                           {
                                                               "$Rating"
                                                           }
                                                       }
                                                   }}
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
            result.ForEach(x => Assert.AreEqual(x.MathValues, System.Math.Log(x.Rating)));
        }

        //log (log base e)
        [Test]
        public void Find_the_log_of_rating_of_all_items()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Rating",1 },
                                {"MathValues", new BsonDocument
                                                   {
                                                       {
                                                           "$log", new BsonArray
                                                           {
                                                               "$Rating",2
                                                           }
                                                       }
                                                   }}
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
            result.ForEach(x => Assert.AreEqual(x.MathValues, System.Math.Log(x.Rating,2)));
        }

        //log (log base 10)
        [Test]
        public void Find_the_log_base_10_of_rating_of_all_items()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Rating",1 },
                                {"MathValues", new BsonDocument
                                                   {
                                                       {
                                                           "$log10", new BsonArray
                                                           {
                                                               "$Rating"
                                                           }
                                                       }
                                                   }}
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
            result.ForEach(x => Assert.AreEqual(System.Math.Round(x.MathValues), System.Math.Round(System.Math.Log(x.Rating, 10))));
        }

        //mod
        [Test]
        public void Find_the_fee_mod_5_of_all_items()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Fee", new BsonDocument
                                                   {
                                                       {
                                                           "$mod", new BsonArray
                                                           {
                                                               "$Fee",5
                                                           }
                                                       }
                                                   }}
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
            result.ForEach(x => Assert.AreEqual(x.Fee, 0));
        }

        //multiply
        [Test]
        public void Find_the_double_fee_of_all_bags()
        {
            PrepareDatabase();
            var match = new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                            {
                                {"Item", "bag"}
                            }
                    }
                };
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
                                                           "$multiply", new BsonArray
                                                           {
                                                               "$Fee",2
                                                           }
                                                       }
                                                   }}
                            }
                    }
                };

            var pipeline = new[] { match, project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 1);
            result.ForEach(x => Assert.AreEqual(x.MathValues, x.Fee * 2));
        }

        //pow
        [Test]
        public void Find_the_rating_pow_of_2_for_all_items()
        {
            PrepareDatabase();           
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Rating",1 },
                                {"MathValues", new BsonDocument
                                                   {
                                                       {
                                                           "$pow", new BsonArray
                                                           {
                                                               "$Rating",2
                                                           }
                                                       }
                                                   }}
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
            result.ForEach(x => Assert.AreEqual(x.MathValues, System.Math.Pow(x.Rating, 2)));
        }

        //sqrt
        [Test]
        public void Find_the_square_root_of_price_of_all_items()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Price",1 },
                                {"MathValues", new BsonDocument
                                                   {
                                                       {
                                                           "$sqrt", new BsonArray
                                                           {
                                                               "$Price"
                                                           }
                                                       }
                                                   }}
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
            result.ForEach(x => Assert.AreEqual(x.MathValues, System.Math.Sqrt(x.Price)));
        }

        //subtract
        [Test]
        public void Find_the_discount_price_of_all_items()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Price",1 },
                                {"Fee",1 },
                                {"MathValues", new BsonDocument
                                                   {
                                                       {
                                                           "$subtract", new BsonArray
                                                           {
                                                               "$Price","$Fee"
                                                           }
                                                       }
                                                   }}
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
            result.ForEach(x => Assert.AreEqual(x.MathValues, x.Price - x.Fee));
        }

        //tranc
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
                                {"Rating",1 },
                                {"MathValues", new BsonDocument
                                                   {
                                                       {
                                                           "$trunc", new BsonArray
                                                           {
                                                               "$Rating"
                                                           }
                                                       }
                                                   }}
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
