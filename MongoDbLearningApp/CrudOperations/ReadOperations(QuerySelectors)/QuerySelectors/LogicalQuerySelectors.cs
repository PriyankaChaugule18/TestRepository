using MongoDB.Driver;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MongoDbLearningApp.CrudOperations.ReadOperations_QuerySelectors_
{
    class LogicalQuerySelectors : AirTravelCollectionMongoDb
    {
        [Test]
        public void Find_document_with_age_greater_than_25_and_foodpreference_is_IndianVegNonVeg()
        {
            PrepareDatabase();
            var expression1 = Builders<AirTravel>.Filter.Gt(x => x.Age, 25);
            var expression2 = Builders<AirTravel>.Filter.Eq(x => x.FoodPreferences,new List<FoodTypes>() { FoodTypes.Indian_NonVeg, FoodTypes.Indian_Veg });

            var filter = Builders<AirTravel>.Filter.And(expression1, expression2);
            var document = travelCollection.Find(filter).ToList();

            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count, 2);
            document.ForEach(x => { Assert.Greater(x.Age,25);
                Assert.AreEqual(x.FoodPreferences, new List<FoodTypes>() { FoodTypes.Indian_NonVeg, FoodTypes.Indian_Veg }); });

        }

        [Test]
        public void Find_document_with_age_not_greater_than_25()
        {
            PrepareDatabase();
            var expression1 = Builders<AirTravel>.Filter.Gt(x => x.Age, 25);        
            var filter = Builders<AirTravel>.Filter.Not(expression1);
            var document = travelCollection.Find(filter).ToList();

            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count, 3);
            document.ForEach(x => {
                Assert.Less(x.Age, 25);
            });

        }

        [Test]
        public void Find_document_with_age_not_greater_than_25_and_foodpreference_is_not_IndianVegNonVeg()
        {
            //#region Insert 10 documents
            //var documents = InitializeData.InsertTravelDetails(testData);
            //travelCollection.InsertMany(documents);
            //#endregion

            //var expression1 = Builders<AirTravel>.Filter.Gt(x => x.Age, 25);
            //var expression2 = Builders<AirTravel>.Filter.Eq(x => x.FoodPreferences, new List<FoodTypes>() { FoodTypes.Indian_NonVeg, FoodTypes.Indian_Veg });
            
            //var filter = Builders<AirTravel>.Filter.Nor(expression1, expression2);
            //var document = travelCollection.Find(filter).ToList();

            //Assert.AreNotEqual(document, null);
            //Assert.AreEqual(document.Count, 2);
            //document.ForEach(x => {
            //    Assert.Greater(x.Age, 25);
            //    Assert.AreEqual(x.FoodPreferences, new List<FoodTypes>() { FoodTypes.Indian_NonVeg, FoodTypes.Indian_Veg });
            //});
        }

        [Test]
        public void Find_document_with_either_age_greater_than_25_or_foodpreference_is_IndianVegNonVeg()
        {
            PrepareDatabase();
            var expression1 = Builders<AirTravel>.Filter.Gt(x => x.Age, 25);
            var expression2 = Builders<AirTravel>.Filter.Eq(x => x.FoodPreferences, new List<FoodTypes>() { FoodTypes.Indian_NonVeg, FoodTypes.Indian_Veg });

            var filter = Builders<AirTravel>.Filter.Or(expression1, expression2);
            var document = travelCollection.Find(filter).ToList();

            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Count, 3);
            document.ForEach(x => Assert.Greater(x.Age, 25));

            //TODO: 
            //document.ForEach(x => Assert.GreaterOrEqual());
        }
        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertTravelDetails(testData);
            travelCollection.InsertMany(documents);
        }
    }
}
