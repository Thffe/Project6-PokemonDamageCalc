using MongoDB.Bson;

using MongoDB.Bson.Serialization.Attributes;

namespace Project6_PokemonDamageCalc
{
    public class Pokelist
    {
        //mongo PK = teamID
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        //link to account's doc id
        [BsonElement("accountID")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string accountId { get; set; } = string.Empty;

        [BsonElement("poke1id")]
        public string poke1id { get; set; }
        [BsonElement("poke2id")]
        public string poke2id { get; set; }
        [BsonElement("poke3id")]
        public string poke3id { get; set; }

        [BsonElement("poke4id")]
        public string poke4id { get; set; }

        [BsonElement("poke5id")]
        public string poke5id { get; set; }
        [BsonElement("poke6id")]
        public string poke6id { get; set; }

        public Pokelist() { }

        public Pokelist(string teamID, string accountID)
        {
            this.Id=teamID;
            this.accountId=accountID;
        }

        
    }
}
