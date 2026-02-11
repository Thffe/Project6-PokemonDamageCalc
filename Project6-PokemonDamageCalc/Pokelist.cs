using MongoDB.Bson;

using MongoDB.Bson.Serialization.Attributes;

namespace Project6_PokemonDamageCalc
{
    public class Pokelist
    {
        //mongo PK = teamID
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int teamID { get; set; }

        //link to account's doc id
        [BsonElement("accountID")]
        [BsonRepresentation(BsonType.ObjectId)]
        public int accountID { get; set; }

        [BsonElement("poke1id")]
        public int poke1id { get; set; }
        [BsonElement("poke2id")]
        public int poke2id { get; set; }
        [BsonElement("poke3id")]
        public int poke3id { get; set; }

        [BsonElement("poke4id")]
        public int poke4id { get; set; }

        [BsonElement("poke5id")]
        public int poke5id { get; set; }
        [BsonElement("poke6id")]
        public int poke6id { get; set; }

        public Pokelist() { }

        public Pokelist(int teamID, int accountID)
        {
            this.teamID=teamID;
            this.accountID=accountID;
        }

        
    }
}
