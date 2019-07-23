using MongoDB.Driver;
using MongoDbLearningApp.CrudOperations;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Linq;
using MongoDB.Bson;

namespace MongoDbLearningApp.Aggregation
{
    class AggregationPipelineStages : AirTravelCollectionMongoDb
    {        
        //TODO: 
        //addField
        [Test]
        public void Add_travelCreditScore_field_to_document()
        {           
            var stageElement = new BsonElement();
            var stage = new BsonDocument(stageElement);

            var stagingValue = new BsonArray
            {
                new BsonDocument
                {
                    {
                        "$addFields", new BsonDocument
                        {
                            {
                                "travelCreditScore",new BsonDocument
                                {
                                    {
                                        "$sum","$TravelRating"
                                    }
                                }
                            }
                        }
                    }
                }, 
                new BsonDocument
                {
                    {
                        "$addFields", new BsonDocument
                        {
                            {
                                "",""
                            }
                        }
                    }
                }
           };

            var pipeline = new[] { stagingValue };
           // var doc = travelCollection.Aggregate<AirTravel>(pipeline).ToList();

            //var doc = travelCollection.Aggregate().AppendStage<AirTravel>(stage).ToList();

           // Assert.AreNotEqual(doc, null);
        }

        //Bucket
        [Test]
        public void Group_document_with_age_and_boundries()
        {
            PrepareDatabase();
           // AggregateExpressionDefinition<AirTravel, int> groupBy = travelCollection.Aggregate<AirTravel>().Group(x => x.Age);
           
            //var boundries = new int[] { 20, 25 };

           // var bucketDoc = travelCollection.Aggregate().Bucket(groupBy, boundries);

           // Assert.AreNotEqual(bucketDoc, null);
        }

        //BucketAuto
        [Test]
        public void Group_document_with_age_and_boundries1()
        {
            PrepareDatabase();
            //// AggregateExpressionDefinition<AirTravel, int> groupBy = travelCollection.Aggregate().Group(x => x.Age);
            // //var groupBy = travelCollection.Aggregate().
            // var boundries = new int[] { 20, 25 };

            // var bucketDoc = travelCollection.Aggregate().Bucket(groupBy, boundries);

            // Assert.AreNotEqual(bucketDoc, null);
        }

        //TODO
        //collStats

        //$Count
        [Test]
        public void Find_the_total_count_of_documents_with_age_greater_than_25()
        {
            PrepareDatabase();
            var filter = Builders<AirTravel>.Filter.Gt(x => x.Age, 25);
            var document = travelCollection.Find(filter);

            var docCount = travelCollection.CountDocuments(filter);
            var docEstimateCount = travelCollection.EstimatedDocumentCount();            

            Assert.AreNotEqual(docCount, null);
            Assert.AreEqual(docCount, 3);
            Assert.AreNotEqual(docEstimateCount, null);
            Assert.AreEqual(docEstimateCount, 6);
        }

        //TODO
        //$curentOp

        //TODO
        //$facet

        //TODO
        //$geoNear

        //TODO
        //$graphLookup

        //group 
        [Test]
        public void Group_document_by_firstName()
        {
            PrepareDatabase();
            var query = travelCollection.Aggregate().Group(x => x.FirstName, group => new { Name = group.Select(x => x.FirstName), Count = group.Count()});
            var docs = query.ToList().OrderByDescending(x=>x.Count);
            var document = docs.FirstOrDefault();

            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Name.First(), "Sonali");
            Assert.AreEqual(docs.Count(), 6);
        }

        //Todo
        //$indexStats
        [Test]
        public void Index()
        {
            PrepareDatabase();
            var doc = new BsonDocument
            {
                {
                    "firstName",1
                },
                {
                    "age",1
                }
            };

            var stats = new BsonDocument
            {
                {
                    "$indexStats",new BsonDocument
                    {
                    }
                }
            };

            string index = travelCollection.Indexes.CreateOne(doc);
            var findQuery = travelCollection.Find(x => x.FirstName =="Krishna");
            var pipeline = new[] { stats };
            var query = travelCollection.Aggregate<AirTravel>(pipeline); 

            //Assert.AreNotEqual(document, null);
            //Assert.AreEqual(document.Name.First(), "Sonali");
            //Assert.AreEqual(docs.Count(), 6);
        }
        
        //$Limit
        [Test]
        public void Find_first_two_document_with_age_greater_than_25()
        {
            PrepareDatabase();
            var filter = Builders<AirTravel>.Filter.Gt(x => x.Age, 25);
            var document = travelCollection.Find(filter);

            var limit = travelCollection.Aggregate().Limit(2).ToList();

            Assert.AreNotEqual(limit, null);
            Assert.AreEqual(limit.Count, 2);
        }

        //TODO
        //listLocalSessions 

        //TODO
        //listSessions 

        //TODO
        //$Lookup
        [Test]
        public void Lookup_age_details_and_include_in_updated_details()
        {
            PrepareDatabase();
            var loopUpDocument = travelCollection.Aggregate().Lookup("Test", "Age", "CustomerAge", "UpdatedDetials").ToList();
            Assert.AreNotEqual(loopUpDocument, null);
        }

        //$match
        [Test]
        public void Find_all_documents_with_matching_age()
        {
            PrepareDatabase();
            var filter = Builders<AirTravel>.Filter.Eq(x => x.Age, 26);
            var document = travelCollection.Find(filter);

            var doc = travelCollection.Aggregate().Match(filter).ToList();

            Assert.AreNotEqual(doc, null);
            Assert.AreEqual(doc.Count, 2);
        }
        
        //$out
        [Test]
        public void Find_all_passenger_names_grouped_with_same_age()
        {
            PrepareDatabase();
            var document = travelCollection.Aggregate().Group(x => x.Age, group => new { Name = group.Select(x => x.Age), Count = group.Count() }).Out("Passengers").ToList();
            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count(), 5);
            Assert.AreEqual(document.LastOrDefault().Count, 2);
        }

        //$project
        [Test]
        public void Project_first_name_last_name_travel_history_of_passengers()
        {
            PrepareDatabase();
            var doc = travelCollection.Aggregate().Project(x => new { x.FirstName, x.LastName, hist = x.TravelHistory.First() }).ToList();
            Assert.AreNotEqual(doc, null);
            Assert.True(doc.ElementAt(0).hist.TravelDate < doc.ElementAt(1).hist.TravelDate);
        }

        //$replaceRoot
        [Test]
        public void ReplaceRoot()
        {
            PrepareDatabase();            
            //var doc = travelCollection.Aggregate().ReplaceRoot()

            //Assert.AreNotEqual(doc, null);
            //Assert.True(doc.ElementAt(0).hist.TravelDate < doc.ElementAt(1).hist.TravelDate);
        }

        //$skip
        [Test]
        public void Find_2_documents_with_age_greater_than_20()
        {
            PrepareDatabase();
            var filter = Builders<AirTravel>.Filter.Gt(x => x.Age, 20);
            var sortDefinition = Builders<AirTravel>.Sort.Descending(x => x.FirstName);
            var document = travelCollection.Find(filter).Sort(sortDefinition).Skip(2).ToList();

            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count(), 2);
            document.ForEach(x => Assert.Greater(x.Age, 20));
        }

        //$sort
        [Test]
        public void Find_all_documents_of_men_with_age_sorted_in_descending_order()
        {
            PrepareDatabase();
            var filter = Builders<AirTravel>.Filter.Eq(x=>x.gender, Gender.Male);
            var sortDefinition = Builders<AirTravel>.Sort.Descending(x => x.Age);
            var document = travelCollection.Find(filter).Sort(sortDefinition).ToList();

            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count(), 5);
            document.ForEach(x => Assert.AreEqual(x.gender, Gender.Male));
        }

        //$sort by count
        [Test]
        public void Find_all_documents_with_age_sort_by_count()
        {
            PrepareDatabase();
            var document = travelCollection.Aggregate().SortByCount(x=>x.Age).ToList();
            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count(), 5);
        }

        //todo
        //$unwind
        [Test]
        public void Find_all_documents_of_seat_preference()
        {
            PrepareDatabase();
            var doc = new BsonDocument
            {
                {
                    "$unwind","$seatPreferences"
                }
            };

            var pipeline = new[] { doc };
            var document = travelCollection.Aggregate<AirTravel>(pipeline).ToList();
            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count(), 5);
        }

        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertTravelDetails(testData);
            travelCollection.InsertMany(documents);
        }
    }
}
