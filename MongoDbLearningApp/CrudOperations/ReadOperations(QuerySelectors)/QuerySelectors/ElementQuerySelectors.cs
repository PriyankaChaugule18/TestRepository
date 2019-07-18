using Mongo2Go;
using MongoDB.Driver;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WonderTools.JsonSectionReader;
namespace MongoDbLearningApp.CrudOperations.ReadOperations_QuerySelectors_
{
    class ElementQuerySelectors : AirTravelCollectionMongoDb
    {
        [Test]
        public void Find_document_where_Travel_frequency_information_exists()
        {
            PrepareDatabase();
            var filter = Builders<AirTravel>.Filter.Exists(x=>x.TravelFrequency);
            var document = travelCollection.Find(filter).ToList();

            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count, 6);
        }

        [Test]
        public void Find_document_where_food_preference_is_of_type_string()
        {
            PrepareDatabase();
            var filter = Builders<AirTravel>.Filter.Type(x => x.FoodPreferences,MongoDB.Bson.BsonType.String);
            var wrongFilter = Builders<AirTravel>.Filter.Type(x => x.FoodPreferences, MongoDB.Bson.BsonType.Int32);
            var document = travelCollection.Find(filter).ToList();
            var dbDocuments = travelCollection.Find(wrongFilter).ToList();

            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count, 6);
            Assert.AreEqual(dbDocuments.Count, 0);
        }

        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertTravelDetails(testData);
            travelCollection.InsertMany(documents);
        }
    }
}
