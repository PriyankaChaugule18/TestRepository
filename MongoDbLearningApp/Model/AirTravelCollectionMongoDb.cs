using MongoDB.Driver;
using NUnit.Framework;
using System.Text;
using WonderTools.JsonSectionReader;

namespace MongoDbLearningApp.Model
{
    class AirTravelCollectionMongoDb : MongoDb
    {
        protected IMongoCollection<AirTravel> travelCollection;
        protected JSection testData;


        [SetUp]
        public void AirTravelCollectionMongoDbSetUp()
        {
            travelCollection = new MongoClient(_connectionString).GetDatabase("testdb").GetCollection<AirTravel>("travel");
            testData = JSectionReader.Section("AirTravel.json", Encoding.UTF8);
        }       
    }
}
