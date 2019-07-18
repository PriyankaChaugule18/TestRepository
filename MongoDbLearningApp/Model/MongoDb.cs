using Mongo2Go;
using NUnit.Framework;

namespace MongoDbLearningApp.Model
{
    class MongoDb
    {
        protected MongoDbRunner Runner;
        protected string _connectionString;

        [SetUp]
        public void MongoDbSetup()
        {
            Runner = MongoDbRunner.Start();
            _connectionString = Runner.ConnectionString;
        }

        [TearDown]
        public void CleanUp()
        {
            Runner.Dispose();
        }
    }
}
