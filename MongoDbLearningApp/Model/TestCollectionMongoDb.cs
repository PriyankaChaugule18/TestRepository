using Mongo2Go;
using MongoDB.Driver;
using NUnit.Framework;

namespace MongoDbLearningApp.Model
{
    class TestCollectionMongoDb : MongoDb
    {
        protected IMongoCollection<Test> mongoCollection;

        [SetUp]
        public void TestCollectionMongoDbSetUp()
        {
            mongoCollection = new MongoClient(_connectionString)
                .GetDatabase("testdb")
                .GetCollection<Test>("testcollection");
        }
    }
}
