using MongoDB.Driver;
using NUnit.Framework;
using System.Text;
using WonderTools.JsonSectionReader;

namespace MongoDbLearningApp.Model
{
    class SalesCollectionMongoDb : MongoDb
    {
        protected IMongoCollection<Sales> salesCollection;
        protected JSection testData;


        [SetUp]
        public void AirTravelCollectionMongoDbSetUp()
        {
            salesCollection = new MongoClient(_connectionString).GetDatabase("testdb").GetCollection<Sales>("sales");
            testData = JSectionReader.Section("Sales.json", Encoding.UTF8);
        }
    }
}
