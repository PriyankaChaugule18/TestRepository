using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Linq;
using System.Text.RegularExpressions;

namespace MongoDbLearningApp.CrudOperations.ReadOperations_QuerySelectors_
{
    class EvaluationQuerySelectors : AirTravelCollectionMongoDb
    {
        [Test]
        public void Find_document_with_age_greater_than_25_with_expression()
        {
            PrepareDatabase();
            var expression = Builders<AirTravel>.Filter.Gt(x => x.Age, 25);           
            
            var document = travelCollection.Find(expression).ToList();

            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count, 3);
        }

        [Test]
        public void Find_document_with_first_name_krishna()
        {
            PrepareDatabase();
           // TODO
         //   string jsonSchema = @"{
         //   'required': 'FirstName', 'LastName', 'Age',
         //   'properties':
         //   {
         //      'FirstName': {'bsonType':'string'},
         //      'LastName': {'bsonType':'string'},
         //      'Age': {'bsonType':'int'},
         //   }
         //}";

            BsonDocument bsonDocument = new BsonDocument()
            {
                {"$firstName", "Krishna" }
            };

            var expression = Builders<AirTravel>.Filter.JsonSchema(bsonDocument);

            var document = travelCollection.Find(expression).ToList();

            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count, 1);
            Assert.AreEqual(document.FirstOrDefault().FirstName, "Krishna");
        }

        [Test]
        public void Find_document_with_age_mod_5_is_0()
        {
            PrepareDatabase();
            var filter = Builders<AirTravel>.Filter.Mod(x => x.Age, 5, 0);
            var document = travelCollection.Find(filter).ToList();

            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count, 1);
            var remainder = document.FirstOrDefault().Age % 5;
            Assert.AreEqual(remainder, 0);
        }

        [Test]
        public void Find_passengers_with_name_starting_with_S()
        {
            PrepareDatabase();
            var filter = Builders<AirTravel>.Filter.Regex(x => x.FirstName, BsonRegularExpression.Create(new Regex("S.*")));
            var document = travelCollection.Find(filter).ToList();

            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count, 4);
        }
        
        //todo
        [Test]
        public void Find_document_with_text_search()
        {
            PrepareDatabase();
            var filter = Builders<AirTravel>.Filter.Text("Krishna", "en-Us");
             var document = travelCollection.Find(filter).ToList();

            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count, 1);
        }

        [Test]
        public void Find_passengers_with_name_starting_with_Sh()
        {
            PrepareDatabase();
            var filter = Builders<AirTravel>.Filter.Where(x=>x.FirstName.StartsWith("Sh"));
            var document = travelCollection.Find(filter).ToList();

            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count, 2);
        }
        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertTravelDetails(testData);
            travelCollection.InsertMany(documents);
        }
    }
}
