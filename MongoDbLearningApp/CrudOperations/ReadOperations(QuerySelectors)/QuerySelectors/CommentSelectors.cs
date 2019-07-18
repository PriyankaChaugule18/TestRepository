using MongoDB.Driver;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Linq;
using MongoDB.Bson;

namespace MongoDbLearningApp.CrudOperations.ReadOperations_QuerySelectors_
{
    class CommentSelectors : SalesCollectionMongoDb
    {  
        //TODO
        [Test]
        public void Find_the_fee_mod_5_of_all_items()
        {
            PrepareDatabase();
            var match = new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                            {
                                {
                                   "x", new BsonDocument
                                   {
                                       {
                                            "gt",0
                                       }
                                   }
                                },
                                // "$comment", "Blah"
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

        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertSalesDetails(testData);
            salesCollection.InsertMany(documents);
        }
    }
}
