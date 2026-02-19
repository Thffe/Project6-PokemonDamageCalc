using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project6_PokemonDamageCalc
{
    public class CalcEntry
    {
        [BsonElement("attackerName")]
        public string attackerName { get; set; } = "";

        [BsonElement("defenderName")]
        public string defenderName { get; set; } = "";

        [BsonElement("moveType")]
        public string moveType { get; set; } = "Normal";

        // physical / special (matches your Category.cs meaning)
        [BsonElement("category")]
        public string category { get; set; } = "physical";

        [BsonElement("power")]
        public int power { get; set; } = 85;

        [BsonElement("attackerLevel")]
        public int attackerLevel { get; set; } = 100;

        [BsonElement("defenderLevel")]
        public int defenderLevel { get; set; } = 100;

        [BsonElement("damagePercent")]
        public double damagePercent { get; set; }

        [BsonElement("createdUtc")]
        public DateTime createdUtc { get; set; } = DateTime.UtcNow;
    }

    public class Pokelist
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // keep your DB field name if you already have data, but consistent property name
        [BsonElement("accountID")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string accountId { get; set; } = string.Empty;

        // One active list stores up to 3 entries
        [BsonElement("entries")]
        public List<CalcEntry> entries { get; set; } = new();

        [BsonElement("updatedUtc")]
        public DateTime updatedUtc { get; set; } = DateTime.UtcNow;

        [BsonElement("createdUtc")]
        public DateTime createdUtc { get; set; } = DateTime.UtcNow;
    }
}
