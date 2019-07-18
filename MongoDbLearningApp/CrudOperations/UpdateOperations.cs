using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbLearningApp.CrudOperations.TestVerification;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Collections.Generic;

namespace MongoDbLearningApp.CrudOperations
{
    class UpdateOperations : TestCollectionMongoDb
    {
        [Test]
        public void Update_one_document_of_height_5feet_2inch_and_set_name_to_updatedName()
        {
            var documents = PrepareDatabase();
            var beforeUpdate = mongoCollection.Find(Builders<Test>.Filter.Empty).ToList();
            var filter = Builders<Test>.Filter.Eq(x => x.Height, 5.2);
            var updateOneOperation = Builders<Test>.Update.Set(x => x.Name, "UpdatedName");
            mongoCollection.UpdateOne(filter, updateOneOperation);

            CrudOperationsVerifier.VerifyUpdateOne(Runner.ConnectionString, documents);
        }

        [Test]
        public void Update_all_documents_and_set_name_to_UpdatedName()
        {
            var documents = PrepareDatabase();
            var beforeUpdate = mongoCollection.Find(Builders<Test>.Filter.Empty).ToList();
            var filter = Builders<Test>.Filter.Empty;
            var updateOperation = Builders<Test>.Update.Set(x => x.Name, "UpdatedName");
            mongoCollection.UpdateMany(filter, updateOperation);

            CrudOperationsVerifier.VerifyUpdateMany(Runner.ConnectionString, documents);
        }

        [Test]
        public void Replace_one_document_of_name_TestName0_with_new_values()
        {
            var documents = PrepareDatabase();
            var beforeReplace = mongoCollection.Find(Builders<Test>.Filter.Empty).ToList();
            var id = beforeReplace.Find(x => x.Name == "TestName0").Id;
            var filter = Builders<Test>.Filter.Eq(x => x.Id, id);
            mongoCollection.ReplaceOne(filter, new Test() { Id=id, Name = "ReplacedName", Age = 23 }, new UpdateOptions() { IsUpsert = false });

            CrudOperationsVerifier.VerifyReplaceOne(Runner.ConnectionString, documents);
        }

        private List<Test> PrepareDatabase()
        {
            var documents = InitializeData.InsertMany();
            mongoCollection.InsertMany(documents);
            return documents;
        }
    }
}
