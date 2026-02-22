using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project6_PokemonDamageCalc;

[BsonIgnoreExtraElements]
public class PokemonDocument
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("pokedex_number")]
    public int PokedexNumber { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = "";

    [BsonElement("type_1")]
    public string Type1 { get; set; } = "";

    [BsonElement("type_2")]
    public string? Type2 { get; set; }

    [BsonElement("hp")]
    public int Hp { get; set; }

    [BsonElement("attack")]
    public int Attack { get; set; }

    [BsonElement("defense")]
    public int Defense { get; set; }

    [BsonElement("sp_attack")]
    public int SpAttack { get; set; }

    [BsonElement("sp_defense")]
    public int SpDefense { get; set; }

    [BsonElement("height_m")]
    public double HeightM { get; set; }

    [BsonElement("weight_kg")]
    public double WeightKg { get; set; }
}
