using MongoDB.Bson;
using MongoDbLearningApp.CrudOperations.TestVerification;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Collections.Generic;

namespace MongoDbLearningApp.CrudOperations
{
    class CreateOperations : TestCollectionMongoDb
    {
        [Test]
        public void Insert_one_document()
        {
            var objectId = ObjectId.GenerateNewId().ToString();
            mongoCollection.InsertOne(new Test()
            { Id = objectId, Name = "TestName", Age = 20 });
            CrudOperationsVerifier.VerifyInsertOne(Runner.ConnectionString, objectId);
        }

        [Test]
        public void Insert_many_documents()
        {
            //Type 1
            List<Test> documents = InsertTwoDocuments();            
            mongoCollection.InsertMany(documents);
            CrudOperationsVerifier.VerifyInsertMany(Runner.ConnectionString, documents);

        }

        public List<Test> InsertTwoDocuments()
        {
            var documents = new List<Test>()
            {
                new Test
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "TestName0",
                    Age = 10
                },
                new Test
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "TestName1",
                    Age = 20
                }
            };
            return documents;
        }

    }
}
