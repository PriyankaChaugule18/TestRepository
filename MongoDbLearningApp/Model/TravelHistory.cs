using System;

namespace MongoDbLearningApp.Model
{
    public class TravelHistory
    {
        public string BookingID { get; set; }
        public DateTime BookingDate { get; set; }
        public string From { get; set; }
        public string Destination { get; set; }
        public DateTime TravelDate { get; set; }

    }
}
