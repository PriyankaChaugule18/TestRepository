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

            var doc = travelCollection.Aggregate().AppendStage<BsonDocument>(stage).ToList();

            Assert.AreNotEqual(doc, null);
        }

        //Bucket
        [Test]
        public void Group_document_with_age_and_boundries()
        {
            #region Insert 10 documents
            var documents = InitializeData.InsertTravelDetails(testData);
            travelCollection.InsertMany(documents);
            #endregion

           //// AggregateExpressionDefinition<AirTravel, int> groupBy = travelCollection.Aggregate().Group(x => x.Age);
           // //var groupBy = travelCollection.Aggregate().
           // var boundries = new int[] { 20, 25 };

           // var bucketDoc = travelCollection.Aggregate().Bucket(groupBy, boundries);

           // Assert.AreNotEqual(bucketDoc, null);
        }

        //BucketAuto
        [Test]
        public void Group_document_with_age_and_boundries1()
        {
            #region Insert 10 documents
            var documents = InitializeData.InsertTravelDetails(testData);
            travelCollection.InsertMany(documents);
            #endregion

            //// AggregateExpressionDefinition<AirTravel, int> groupBy = travelCollection.Aggregate().Group(x => x.Age);
            // //var groupBy = travelCollection.Aggregate().
            // var boundries = new int[] { 20, 25 };

            // var bucketDoc = travelCollection.Aggregate().Bucket(groupBy, boundries);

            // Assert.AreNotEqual(bucketDoc, null);
        }

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
        //$Lookup
        [Test]
        public void Lookup_age_details_and_include_in_updated_details()
        {
            PrepareDatabase();
            var loopUpDocument = travelCollection.Aggregate().Lookup("Test", "Age", "CustomerAge", "UpdatedDetials").ToList();
            Assert.AreNotEqual(loopUpDocument, null);
        }

        //$Match
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

        [Ignore("Correct use of out needs to be made!")]
        //$out
        [Test]
        public void Find_all_passenger_names_grouped_with_same_age()
        {
            PrepareDatabase();
            var doc = travelCollection.Aggregate().Group(x =>x.Age,grp=> new { }).Out("Passengers").ToList();
            Assert.AreNotEqual(doc, null);
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
        public void Sort_by_count()
        {
            PrepareDatabase();
            var document = travelCollection.Aggregate().SortByCount(x=>x.Age).ToList();
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
