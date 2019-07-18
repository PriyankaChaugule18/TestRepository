using Mongo2Go;
using MongoDB.Driver;
using MongoDbLearningApp.Model;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MongoDbLearningApp.CrudOperations.ReadOperations_QuerySelectors_
{
    class ComparisonQuerySelector : TestCollectionMongoDb
    {
        [Test]
        public void Find_document_with_name_equal_to_TestName0()
        {
            PrepareDatabase();
            var filterFindOne = Builders<Test>.Filter.Eq(x => x.Name, "TestName0");
            var document = mongoCollection.Find(filterFindOne).FirstOrDefault();

            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Name, "TestName0");
        }

        [Test]
        public void Find_document_with_age_greater_than_20()
        {
            PrepareDatabase();
            var filter = Builders<Test>.Filter.Gt(x => x.Age, 20);
            var dBdocuments = mongoCollection.Find(filter).ToList();

            Assert.AreNotEqual(dBdocuments, null);
            Assert.AreEqual(dBdocuments.Count, 8);
        }

        [Test]
        public void Find_document_with_age_greater_than_equal_to_20()
        {
            PrepareDatabase();
            var filter = Builders<Test>.Filter.Gte(x => x.Age, 20);
            var dBdocuments = mongoCollection.Find(filter).ToList();

            Assert.AreNotEqual(dBdocuments, null);
            Assert.AreEqual(dBdocuments.Count, 9);
        }

        [Test]
        public void Find_document_with_age_equal_to_50_or_80()
        {
            PrepareDatabase();
            var ageList = new List<int>
            {
                50,
                80
            };

            var filter = Builders<Test>.Filter.In(x => x.Age, ageList);
            var dBdocuments = mongoCollection.Find(filter).ToList();

            var documentAges = dBdocuments.Select(x => x.Age).ToList();

            Assert.AreNotEqual(dBdocuments, null);
            Assert.AreEqual(dBdocuments.Count, 2);
            CollectionAssert.DoesNotContain(documentAges, 20);
            CollectionAssert.Contains(documentAges, 50);
            CollectionAssert.Contains(documentAges, 80);
        }

        [Test]
        public void Find_document_with_age_less_than_50()
        {
            PrepareDatabase();
            var filter = Builders<Test>.Filter.Lt(x => x.Age, 50);
            var dBdocuments = mongoCollection.Find(filter).ToList();

            Assert.AreNotEqual(dBdocuments, null);
            Assert.AreEqual(dBdocuments.Count, 4);
        }

        [Test]
        public void Find_document_with_age_less_than_equal_to_50()
        {
            PrepareDatabase();
            var filter = Builders<Test>.Filter.Lte(x => x.Age, 50);
            var dBdocuments = mongoCollection.Find(filter).ToList();

            Assert.AreNotEqual(dBdocuments, null);
            Assert.AreEqual(dBdocuments.Count, 5);
        }

        [Test]
        public void Find_document_with_age_not_equal_to_50()
        {
            PrepareDatabase();
            var filter = Builders<Test>.Filter.Ne(x => x.Age, 50);
            var dBdocuments = mongoCollection.Find(filter).ToList();

            Assert.AreNotEqual(dBdocuments, null);
            Assert.AreEqual(dBdocuments.Count, 9);
        }

        [Test]
        public void Find_document_with_age_not_equal_to_50_nor_80()
        {
            PrepareDatabase();
            var ageList = new List<int>
            {
                50,
                80
            };

            var filter = Builders<Test>.Filter.Nin(x => x.Age,ageList);
            var dBdocuments = mongoCollection.Find(filter).ToList();

            var documentAges = dBdocuments.Select(x => x.Age).ToList();

            Assert.AreNotEqual(dBdocuments, null);
            Assert.AreEqual(dBdocuments.Count, 8);
            CollectionAssert.DoesNotContain(documentAges, 50);
            CollectionAssert.DoesNotContain(documentAges, 80);
        }

        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertMany();
            mongoCollection.InsertMany(documents);
        }
    }
}
