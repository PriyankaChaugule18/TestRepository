using MongoDbLearningApp.CrudOperations;
using MongoDB.Driver;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Linq;
using MongoDB.Bson;

namespace MongoDbLearningApp.Aggregation.AggregationPipelineOperators
{
    class TypeExpressionOperators : SalesCollectionMongoDb
    {
        //convert
        [Test]
        public void Convert_price_from_int_to_double_type()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"PriceValues", new BsonDocument
                                                   {
                                                       {
                                                           "$convert", new BsonDocument
                                                           {
                                                               {
                                                                   "input", "$Price" 
                                                                   //"to",
                                                               }
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

        //bool
        [Test]
        public void Convert_price_from_int_to_double_type1()
        {
            PrepareDatabase();
            var project = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"PriceValues", new BsonDocument
                                                   {
                                                       {
                                                           "$convert", new BsonDocument
                                                           {
                                                               {
                                                                   "input", "$Price" 
                                                                   //"to",
                                                               }
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



        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertSalesDetails(testData);
            salesCollection.InsertMany(documents);
        }
    }
}
