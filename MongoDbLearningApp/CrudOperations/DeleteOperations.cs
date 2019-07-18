using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbLearningApp.CrudOperations.TestVerification;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDbLearningApp.CrudOperations
{
    class DeleteOperations :TestCollectionMongoDb
    {
        [Test]
        public void Delete_one_document_with_name_TestName0()
        {
            var documents = PrepareDatabase();
            var beforeUpdate = mongoCollection.Find(Builders<Test>.Filter.Empty).ToList();
            var filter = Builders<Test>.Filter.Eq(x => x.Name, "TestName0");
            mongoCollection.DeleteOne(filter);

            CrudOperationsVerifier.VerifyDeleteOne(Runner.ConnectionString, documents);
        }

        [Test]
        public void Delete_many_documents_with_name_TestName0()
        {
            var documents=PrepareDatabase();
            var beforeUpdate = mongoCollection.Find(Builders<Test>.Filter.Empty).ToList();
            var filter = Builders<Test>.Filter.Gt(x => x.Age, 20);
            mongoCollection.DeleteMany(filter);

            CrudOperationsVerifier.VerifyDeleteMany(Runner.ConnectionString, documents);
        }

        private List<Test> PrepareDatabase()
        {
            var documents = InitializeData.InsertMany();
            mongoCollection.InsertMany(documents);
            return documents;
        }
    }
}
