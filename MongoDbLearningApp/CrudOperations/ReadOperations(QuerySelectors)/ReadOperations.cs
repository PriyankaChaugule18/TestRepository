using MongoDB.Driver;
using MongoDbLearningApp.CrudOperations.TestVerification;
using MongoDbLearningApp.Model;
using NUnit.Framework;

namespace MongoDbLearningApp.CrudOperations
{
    class ReadOperations : TestCollectionMongoDb
    {
        [Test]
        public void Find_all_documents()
        {
            PrepareDatabase();            
            var filterFindAll = Builders<Test>.Filter.Empty;
            var dbDocuments = mongoCollection.Find(filterFindAll).ToList();
            CrudOperationsVerifier.VerifyFindAll(dbDocuments);
        }

        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertMany();
            mongoCollection.InsertMany(documents);
        }       
    }
}
