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
    class ArrayQuerySelectors : AirTravelCollectionMongoDb
    {
        [Test]
        public void Find_document_with_all_food_preferences()
        {
            PrepareDatabase();
            var filter = Builders<AirTravel>.Filter.All(x => x.FoodPreferences, new List<FoodTypes>() { FoodTypes.Chinese, FoodTypes.Indian_NonVeg, FoodTypes.Indian_Veg });
            var document = travelCollection.Find(filter).ToList();

            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count, 1);
            Assert.AreEqual(document.FirstOrDefault().FoodPreferences, new List<FoodTypes>() { FoodTypes.Chinese, FoodTypes.Indian_NonVeg, FoodTypes.Indian_Veg });
        }

        [Test]
        public void Find_document_with_travel_history_from_or_to_Delhi()
        {
            PrepareDatabase();
            var filter = Builders<AirTravel>.Filter.ElemMatch(x => x.TravelHistory, his => his.From == "Delhi")
                            | Builders<AirTravel>.Filter.ElemMatch(x => x.TravelHistory, his => his.Destination == "Delhi");

            var document = travelCollection.Find(filter).ToList();
            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count, 3);
        }

        [Test]
        public void Find_document_with_one_food_preference()
        {
            PrepareDatabase();
            var filter = Builders<AirTravel>.Filter.Size(x => x.FoodPreferences, 1);

            var document = travelCollection.Find(filter).ToList();
            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count, 2);
            document.ForEach(x => Assert.AreEqual(x.FoodPreferences.Count, 1));
        }

        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertTravelDetails(testData);
            travelCollection.InsertMany(documents);
        }
    }
}
