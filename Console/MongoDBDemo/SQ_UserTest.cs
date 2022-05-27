using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDBDemo
{
    public class SQ_UserTest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string vcUser { get; set; }
        public int snType { get; set; }

        public string vcName { get; set; }

        public long snParentId { get; set; }

        public string vcParentName { get; set; }

        public long nMessageCount { get; set; }

        public DateTime dateTime { get; set; }
    }
}
