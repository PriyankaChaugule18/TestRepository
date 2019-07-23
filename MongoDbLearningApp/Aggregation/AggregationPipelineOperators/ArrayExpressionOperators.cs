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
        //$arrayElemAt
        [Test]
        public void Find_the_item_at_specific_index_from_array()
        {
            PrepareDatabase();
            var elementToFindAtIndex = 0;
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"FirstName",1 },
                                {"FoodPreferences",1 },
                                {"Result", new BsonDocument
                                                   {
                                                       {
                                                           "$arrayElemAt", new BsonArray
                                                           {
                                                               "$FoodPreferences",elementToFindAtIndex
                                                           }
                                                       }
                                                   }
                                }
                            }
                    }
                };

            var pipeline = new[] { project };
            var result = travelCollection.Aggregate<AirTravel>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 6);
            result.ForEach(x => Assert.AreEqual(x.FoodPreferences.ElementAt(elementToFindAtIndex).ToString(), x.Result));
        }

        //$arrayToObject
        [Test]
        public void Convert_the_array_into_single_document()
        {
            PrepareDatabase();
            var project = new BsonDocument
            {
                {
                    "$project",
                    new BsonDocument
                    {
                        {"FirstName",1 },
                        {"Beverages",1 },
                        {"BsonResult", new BsonDocument
                            {
                                {
                                    "$arrayToObject", "$Beverages"
                                }
                            }
                        }
                    }
                }
            };

            var pipeline = new[] { project };
            var result = travelCollection.Aggregate<AirTravel>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 6);
            var data = result.First();
            Assert.AreEqual(data.Beverages.Length, data.BsonResult.ToBsonDocument().Elements.Count());
        }

        //$concatArrays
        [Test]
        public void Merge_the_food_and_beverages_in_single_array()
        {
            PrepareDatabase();
            var project = new BsonDocument
            {
                {
                    "$project",
                    new BsonDocument
                    {
                        {"FirstName",1 },
                        {"FoodPreferences",1 },
                        {"Beverages",1 },
                        {"BsonResult", new BsonDocument
                            {
                                {
                                    "$concatArrays", new BsonArray()
                                    {
                                        "$FoodPreferences","$Beverages"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var pipeline = new[] { project };
            var result = travelCollection.Aggregate<AirTravel>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 6);
            var data = result.First();
            var resultDocument = data.BsonResult.ToJson();
            foreach (var dataFoodPreference in data.FoodPreferences)
            {
                Assert.IsTrue(resultDocument.Contains(dataFoodPreference.ToString()));
            }

        }

        //$filter
        //[Ignore("TODO")]
        [Test]
        public void Select_the_indian_food_preferences_from_the_passengers()
        {
            PrepareDatabase();
            var project = new BsonDocument
            {
                {
                    "$project",
                    new BsonDocument
                    {
                        {"FirstName",1 },
                        {"FoodPreferences",1 },
                        {"FoodPreferences", new BsonDocument
                            {
                                {
                                    "$filter", new BsonDocument()
                                    {
                                         {
                                             "input",new BsonArray()
                                         {
                                             "$FoodPreferences"
                                         }
                                         },
                                        {"as","string"},
                                        {"cond",new BsonDocument()
                                        {
                                            {
                                                "$eq", new BsonArray()
                                                {
                                                    "$FoodTypes", "Chinese"
                                                }
                                            }
                                        } }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var pipeline = new[] { project };
            var result = travelCollection.Aggregate<AirTravel>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 6);
            var data = result.First();
            var resultDocument = data.BsonResult.ToBsonDocument().GetValue(0).ToString();
            foreach (var dataFoodPreference in data.FoodPreferences)
            {
                Assert.IsTrue(resultDocument.Contains(dataFoodPreference.ToString()));
            }
        }

        //$in
        [Test]
        public void Check_whether_passengers_have_Indian_Veg_as_food_preferences()
        {
            PrepareDatabase();
            var project = new BsonDocument
            {
                {
                    "$project",
                    new BsonDocument
                    {
                        {"FirstName",1 },
                        {"FoodPreferences",1 },
                        {"BooleanResult", new BsonDocument
                            {
                                {
                                    "$in", new BsonArray()
                                    {
                                       "Indian_Veg","$FoodPreferences"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var pipeline = new[] { project };
            var result = travelCollection.Aggregate<AirTravel>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 6);
            foreach (var airTravel in result)
            {
                Assert.AreEqual(airTravel.FoodPreferences.Any(x => x.ToString().Equals("Indian_Veg")), airTravel.BooleanResult);
            }

        }

        //$indexOfArray
        [Test]
        public void Check_whether_passengers_have_Indian_Veg_as_food_preferences_using_indexOfArray_operator()
        {
            PrepareDatabase();
            var project = new BsonDocument
            {
                {
                    "$project",
                    new BsonDocument
                    {
                        {"FirstName",1 },
                        {"FoodPreferences",1 },
                        {"IntegerResult", new BsonDocument
                            {
                                {
                                    "$indexOfArray", new BsonArray()
                                    {
                                        "$FoodPreferences","Indian_Veg",
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var pipeline = new[] { project };
            var result = travelCollection.Aggregate<AirTravel>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 6);
            foreach (var airTravel in result)
            {
                Assert.AreEqual(airTravel.FoodPreferences.IndexOf(FoodTypes.Indian_Veg), airTravel.IntegerResult);
            }

        }

        //$size
        [Test]
        public void Calculate_number_of_food_preferences_of_a_passenger()
        {
            PrepareDatabase();
            var project = new BsonDocument
            {
                {
                    "$project",
                    new BsonDocument
                    {
                        {"FirstName",1 },
                        {"FoodPreferences",1 },
                        {"IntegerResult", new BsonDocument
                            {
                                {
                                    "$size", new BsonArray()
                                    {
                                        "$FoodPreferences"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var pipeline = new[] { project };
            var result = travelCollection.Aggregate<AirTravel>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 6);
            foreach (var airTravel in result)
            {
                Assert.AreEqual(airTravel.FoodPreferences.Count, airTravel.IntegerResult);
            }

        }

        //$slice
        [Test]
        public void Select_last_two_food_preferences_of_each_passenger_from_their_list_of_food_preferences()
        {
            PrepareDatabase();
            var project = new BsonDocument
            {
                {
                    "$project",
                    new BsonDocument
                    {
                        {"FirstName",1 },
                        {"FoodPreferences",1 },
                        {"BsonResult", new BsonDocument
                            {
                                {
                                    "$slice", new BsonArray()
                                    {
                                        "$FoodPreferences", -2
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var pipeline = new[] { project };
            var result = travelCollection.Aggregate<AirTravel>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 6);
            foreach (var airTravel in result)
            {
                var totalFoodPreferences = airTravel.FoodPreferences.Count;
                var queryResult = airTravel.BsonResult.ToJson();
                Assert.IsTrue(queryResult.Contains(airTravel.FoodPreferences[totalFoodPreferences - 1].ToString()));
                if (airTravel.FoodPreferences.Count > 1)
                {
                    Assert.IsTrue(queryResult.Contains(airTravel.FoodPreferences[totalFoodPreferences - 2].ToString()));
                }
            }

        }

        //$isArray
        //$cond
        [Test]
        public void Merge_the_food_and_beverages_in_single_array_if_they_are_array()
        {
            PrepareDatabase();
            var project = new BsonDocument
            {
                {
                    "$project",
                    new BsonDocument
                    {
                        {"FirstName",1 },
                        {"FoodPreferences",1 },
                        {"Beverages",1 },
                        {"BsonResult", new BsonDocument
                            {
                                {
                                    "$cond", new BsonDocument()
                                    {
                                        {
                                            $"if",new BsonDocument()
                                            {
                                                {
                                                    "$and",new BsonArray()
                                                    {

                                                        new BsonDocument()
                                                        {
                                                        {
                                                            "$isArray","$FoodPreferences"
                                                        }},
                                                        new BsonDocument()
                                                        {
                                                        {
                                                            "$isArray","$Beverages"
                                                        }},

                                                    }
                                                }
                                            }
                                            },
                                        {
                                            $"then",new BsonDocument()
                                            {
                                                {
                                                    "$concatArrays", new BsonArray()
                                                    {
                                                        "$FoodPreferences","$Beverages"
                                                    }
                                                }
                                            }
                                        },
                                        {
                                            $"else", "One or more fields is not an array."
                                        }

                                    }
                                    }
                                }

                                }
                            }
                    }
            };

            var pipeline = new[] { project };
            var result = travelCollection.Aggregate<AirTravel>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 6);

            foreach (var airTravel in result)
            {
                var resultDocument = airTravel.BsonResult.ToString();
                if (airTravel.FoodPreferences == null || airTravel.Beverages == null)
                {
                    Assert.IsTrue(resultDocument.Equals("One or more fields is not an array."));
                }
            }

        }

        //$reverseArray
        [Test]
        public void Reverse_the_food_preferences_of_each_passenger_from_their_list_of_food_preferences()
        {
            PrepareDatabase();
            var project = new BsonDocument
            {
                {
                    "$project",
                    new BsonDocument
                    {
                        {"FirstName",1 },
                        {"FoodPreferences",1 },
                        {"ArrayResult", new BsonDocument
                            {
                                {
                                    "$reverseArray", new BsonArray()
                                    {
                                        "$FoodPreferences",
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var pipeline = new[] { project };
            var result = travelCollection.Aggregate<AirTravel>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 6);
            foreach (var airTravel in result)
            {
                var array = airTravel.ArrayResult.Reverse().ToArray();
                for (int i = 0; i < airTravel.FoodPreferences.Count; i++)
                {
                    Assert.AreEqual(airTravel.FoodPreferences[i].ToString(), array[i]);
                }
            }

        }

        //Data needed
        //$objectToArray
        [Test]
        public void Convert_the_object_into_array()
        {
            PrepareDatabase();
            var project = new BsonDocument
            {
                {
                    "$project",
                    new BsonDocument
                    {
                        {"FirstName",1 },
                        {"Beverages",1 },
                        {"ArrayResult", new BsonDocument
                            {
                                {
                                    "$objectToArray", "$TravelHistory"
                                }
                            }
                        }
                    }
                }
            };

            var pipeline = new[] { project };
            var result = travelCollection.Aggregate<AirTravel>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 6);
            var data = result.First();
            Assert.AreEqual(data.Beverages.Length, data.BsonResult.ToBsonDocument().Elements.Count());
        }

        //$reduce
        [Test]
        public void Find_all_the_food_preferences_of_the_passengers()
        {
            PrepareDatabase();
            var project = new BsonDocument
            {
                {
                    "$project",
                    new BsonDocument
                    {
                        {"FirstName",1 },
                        {"FoodPreferences",1 },
                        {"Result", new BsonDocument
                            {
                                {
                                    "$reduce", new BsonDocument()
                                    {
                                        {
                                            $"input","$FoodPreferences"
                                        },
                                        {
                                            $"initialValue","My Food Preferences are: "
                                        },
                                        {
                                            $"in", new BsonDocument()
                                            {
                                                {
                                                    "$concat",new BsonArray()
                                                    {
                                                        "$$value",
                                                        new BsonDocument()
                                                        {
                                                            {
                                                                "$cond",new BsonDocument()
                                                                {
                                                                    {
                                                                        $"if",new BsonDocument()
                                            {
                                                {
                                                    "$eq",new BsonArray()
                                                    {
                                                        "$$value",
                                                        "My Food Preferences are: "
                                                    }
                                                }
                                            }
                                            },
                                        {
                                            $"then",""
                                        },
                                        {
                                            $"else", ", "
                                        }
                                                                }
                                                            }
                                                        },
                                                        "$$this"
                                                    }
                                                }
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
            var result = travelCollection.Aggregate<AirTravel>(pipeline).ToList();

            Assert.AreNotEqual(result, null);
            Assert.AreEqual(result.Count, 6);
            foreach (var airTravel in result)
            {
                Assert.IsTrue(airTravel.Result.Contains("My Food Preferences are:"));
            }

        }


        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertTravelDetails(testData);
            travelCollection.InsertMany(documents);
        }
    }
}
