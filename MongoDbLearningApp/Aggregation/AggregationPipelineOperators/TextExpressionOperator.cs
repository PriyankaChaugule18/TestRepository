using MongoDbLearningApp.CrudOperations;
using MongoDbLearningApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDbLearningApp.Aggregation.AggregationPipelineOperators
{
    class TextExpressionOperator: SalesCollectionMongoDb
    {
        //todo

        private void PrepareDatabase()
        {
            var documents = InitializeData.InsertSalesDetails(testData);
            salesCollection.InsertMany(documents);
        }
    }
}
