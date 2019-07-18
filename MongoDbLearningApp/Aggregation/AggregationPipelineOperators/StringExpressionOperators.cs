using MongoDB.Driver;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Linq;
using MongoDB.Bson;
using MongoDbLearningApp.CrudOperations;
using System;

namespace MongoDbLearningApp.Aggregation.AggregationPipelineOperators
{
    class StringExpressionOperators : SalesCollectionMongoDb
    {
        //concat
        [Test]
        public void Find_the_name_and_description_of_all_items()
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
                                                           "$concat", new BsonArray
                                                           {
                                                               "$Name", "-", "$Item"
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
            Assert.IsTrue(result.FirstOrDefault().Item.Contains(result.FirstOrDefault().Name + "-"));
        }

        //todo
        //$dateFromString

        //todo
        //$dateToString

        //$indexOfBytes
        [Test]
        public void Find_the_byteLocation_of_all_bags()
        {
            PrepareDatabase();
            var match = new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                            {
                                {"Item","sling bag  "},
                            }
                    }
                };

            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {                                
                                {"IndexOfBytes", new BsonDocument
                                                   {
                                                       {
                                                           "$indexOfBytes", new BsonArray
                                                           {
                                                               "$Item", "a"
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
            Assert.AreEqual(result.FirstOrDefault().IndexOfBytes, 7);
        }

        //$indexOfCP
        [Test]
        public void Find_the_code_point_location_starting_of_all_bags()
        {
            PrepareDatabase();
            var match = new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                            {
                                {"Item","sling bag  "},
                            }
                    }
                };

            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"IndexOfBytes", new BsonDocument
                                                   {
                                                       {
                                                           "$indexOfBytes", new BsonArray
                                                           {
                                                               "$Item", "a"
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
            result.ForEach(x => Assert.AreEqual(x.IndexOfBytes, 7));
        }

        //$ltrim
        [Test]
        public void Find_bag_items_and_remove_leading_whitespace()
        {
            PrepareDatabase();

            var match = new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                            {
                                {"Item","  bag"},
                            }
                    }
                };

            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Item", new BsonDocument
                                {
                                    {
                                    "$ltrim", new BsonDocument
                                    {
                                       {
                                           "input","$Item"
                                        }

                                    }
                                    }
                            }
                        }
                            }
                }
                };

            var pipeline = new[] {match, project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 1);
            Assert.IsTrue(!result.FirstOrDefault().Item.Any(x => char.IsWhiteSpace(x)));
        }

        //$rtrim
        [Test]
        public void Find_all_item_names_and_remove_trailing_whitespace()
        {
            PrepareDatabase();
            var match = new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                            {
                                {"Item","sling bag  "},
                            }
                    }
                };

            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Item",1 },
                                {"Name", new BsonDocument
                                {
                                    {
                                    "$rtrim", new BsonDocument
                                    {
                                        {
                                             "input","$Item"
                                        }

                                    }
                                    }
                            }
                        }
                            }
                }
                };

            var pipeline = new[] {match, project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result.FirstOrDefault().Name, result.FirstOrDefault().Item.TrimEnd(' '));
        }

        //$split
        [Test]
        public void Find_all_items_and_split_item_with_space()
        {
            PrepareDatabase();
            var match = new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                            {
                                {"Name","Steve Madden"},
                            }
                    }
                };

            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"ArrayValues", new BsonDocument
                                {
                                    {
                                    "$split", new BsonArray
                                    {
                                        "$Name"," "
                                    }
                                    }
                            }
                        }
                            }
                }
                };

            var pipeline = new[] {match, project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result.FirstOrDefault().ArrayValues.Count(), 2);
            Assert.AreEqual(result.FirstOrDefault().ArrayValues.ElementAt(0), "Steve");
            Assert.AreEqual(result.FirstOrDefault().ArrayValues.ElementAt(1), "Madden");
        }

        //$strLenBytes
        [Test]
        public void Find_the_length_of_the_name_of_items()
        {
            PrepareDatabase();   

            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Name",1 },
                                {"IndexOfBytes", new BsonDocument
                                                   {
                                                       {
                                                           "$strLenBytes","$Name"
                                                       }
                                                   }}
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
            result.ForEach(x => Assert.AreEqual(x.IndexOfBytes, x.Name.Length));
        }

        //$strLenBytes
        [Test]
        public void Find_the_length_of_the_item()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Name",1 },
                                {"IndexOfBytes", new BsonDocument
                                                   {
                                                       {
                                                           "$strLenCP","$Name"
                                                       }
                                                   }}
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
            result.ForEach(x => Assert.AreEqual(x.IndexOfBytes, x.Name.Length));
        }

        //$strcasecmp
        [Test]
        public void Find_and_compare_the_case_sensitive_items()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Name",1 },
                                {"IndexOfBytes", new BsonDocument
                                                   {
                                                       {
                                                           "$strcasecmp",
                                        new BsonArray
                                        {
                                            "$Name","StEvE Madden"
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
            Assert.AreEqual(result.ElementAt(1).IndexOfBytes, 0);
        }

        //$substr
        [Test]
        public void Find_the_substring_of_an_item()
        {
            PrepareDatabase();
            var match = new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                            {
                                {"Name","Steve Madden"},
                            }
                    }
                };
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {                                
                                {"Name", new BsonDocument
                                                   {
                                                       {
                                                           "$substr",
                                        new BsonArray
                                        {
                                            "$Name",0,5
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
            Assert.AreEqual(result.ElementAt(0).Name,"Steve");
        }

        //$substrCP
        [Test]
        public void Find_the_substring_of_zero_based_index_all_item_for_length_3()
        {
            PrepareDatabase();            
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Item", new BsonDocument
                                                   {
                                                       {
                                                           "$substrCP",
                                        new BsonArray
                                        {
                                            "$Item",0,3
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
            Assert.AreEqual(result.FirstOrDefault().Item.Length, 3);
        }

        //$substrBytes
        [Test]
        public void Find_the_substring_of_all_item_for_length_3()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Name", new BsonDocument
                                                   {
                                                       {
                                                           "$substrBytes",
                                        new BsonArray
                                        {
                                            "$Name",0,3
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
            result.ForEach(x => Assert.AreEqual(x.Name.Length, 3));
        }

       //$toLower
        [Test]
        public void Find_and_convert_a_name_of_item_to_lower_case()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Name", new BsonDocument
                                                   {
                                                       {
                                                           "$toLower","$Name"
                                                       }
                                                   }}
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
            result.ForEach(x => Assert.IsTrue(!x.Name.Any(char.IsUpper)));
        }

        //$toString
        [Test]
        public void Find_and_convert_price_to_string()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Price",1 },
                                {"Name", new BsonDocument
                                                   {
                                                       {
                                                           "$toString","$Price"
                                                       }
                                                   }}
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
            result.ForEach(x => Assert.AreEqual(x.Name, x.Price.ToString()));
        }

        //todo
        //$trim
        [Test]
        public void Find_all_item_names_and_remove_leading_and_trailing_whitespace()
        {
            PrepareDatabase();           

            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Item",1 },
                                //We are using name to save the value.
                                {"Name", new BsonDocument
                                {
                                    {
                                    "$trim", new BsonDocument
                                    {
                                        {
                                             "input","$Item"
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
            Assert.AreEqual(result.ElementAt(0).Name, result.ElementAt(0).Item.TrimEnd(' '));
            Assert.AreEqual(result.ElementAt(3).Name, result.ElementAt(3).Item.TrimStart(' '));
        }

        //$toUpper
        [Test]
        public void Find_and_convert_a_name_of_item_to_upper_case()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"Name", new BsonDocument
                                                   {
                                                       {
                                                           "$toUpper","$Name"
                                                       }
                                                   }}
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = salesCollection.Aggregate<Sales>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 5);
            result.ForEach(x => Assert.IsTrue(!x.Name.Any(char.IsLower)));
        }

        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertSalesDetails(testData);
            salesCollection.InsertMany(documents);
        }
    }
}
