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
        public int accountID { get; set; }

        [BsonElement("username")]
        public string username {  get; set; } = string.Empty;

        //base64 keeps load stay json 
        [BsonElement("pfp")]
        public string pfp { get; set; } = null!;

        [BsonElement("pfpType")]
        public string pfpType { get; set; } = null!; // "image/png", "image/jpeg"

        public Account() { }

        public Account(int accountID, string username)
        {
            this.accountID=accountID;
            this.username=username;
        }

               

    }

}
