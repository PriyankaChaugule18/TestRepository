using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbLearningApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoDbLearningApp.CrudOperations
{
    public class InitializeData
    {
        public static List<Test> InsertMany()
        {
            var documents = Enumerable.Range(0, 10)
                .Select(i => new Test()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "TestName" + i,
                    Age = 10 + i * 10,
                    Height = Convert.ToDouble(i/10d) + 5.2
                }).ToList();
            return documents;
        }

        public static List<AirTravel> InsertTravelDetails(WonderTools.JsonSectionReader.JSection testData)
        {
            var travelData = testData.GetSection("AirTravel").GetObject<List<AirTravel>>();
            return travelData;
        }

        public static List<Sales> InsertSalesDetails(WonderTools.JsonSectionReader.JSection testData)
        {
            var salesData = testData.GetSection("Sales").GetObject<List<Sales>>();
            return salesData;
        }
    }
}
