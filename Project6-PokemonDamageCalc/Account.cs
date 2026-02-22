using System.Reflection.Metadata;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project6_PokemonDamageCalc
{
    public class Account
    {
        //mongo doc PK (ID) = accountID
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }   // Mongo _id

        [BsonElement("username")]
        public string username { get; set; } = string.Empty;

        [BsonElement("pfp")]
        public string? pfp { get; set; }  // allow null

        [BsonElement("pfpType")]
        public string? pfpType { get; set; }


        public Account() { }

        public Account(string accountID, string username)
        {
            this.Id=accountID;
            this.username=username;
        }

               

    }

}
