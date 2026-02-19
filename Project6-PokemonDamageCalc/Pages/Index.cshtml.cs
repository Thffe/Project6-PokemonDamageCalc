using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using MongoDB.Driver;
using Project6_PokemonDamageCalc.Services;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Project6_PokemonDamageCalc.Pages;

public class IndexModel : PageModel
{
    private readonly IMongoCollection<PokemonDocument> _pokemon;
    private readonly PokelistService _pokelistService;

    public IndexModel(
        IMongoCollection<PokemonDocument> pokemon,
        PokelistService pokelistService)
    {
        _pokemon = pokemon;
        _pokelistService = pokelistService;
    }

    public Pokemon? Attacker { get; private set; }
    public Pokemon? Defender { get; private set; }

    // show logged-in user's saved lists
    public List<Pokelist> MyPokelists { get; private set; } = new();

    public async Task OnGetAsync(string? attackerName = null, string? defenderName = null)
    {
        // Load saved pokelists if authenticated
        if (User.Identity?.IsAuthenticated == true)
        {
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrWhiteSpace(accountId))
            {
                MyPokelists = await _pokelistService.getPokelistsFromAnAccount(accountId, limit: 50);
            }
        }

        attackerName ??= "Gengar";
        defenderName ??= "Jirachi";

        var attackerDoc = await FindPokemonByNameCaseInsensitive(attackerName);
        var defenderDoc = await FindPokemonByNameCaseInsensitive(defenderName);

        Attacker = attackerDoc is null ? null : MapToPokemon(attackerDoc, lvl: 100);
        Defender = defenderDoc is null ? null : MapToPokemon(defenderDoc, lvl: 100);
    }

    private async Task<PokemonDocument?> FindPokemonByNameCaseInsensitive(string name)
    {
        // Exact match, case-insensitive: ^name$
        var filter = Builders<PokemonDocument>.Filter.Regex(
            x => x.Name,
            new BsonRegularExpression($"^{Regex.Escape(name)}$", "i")
        );

        return await _pokemon.Find(filter).FirstOrDefaultAsync();
    }

    private static Pokemon MapToPokemon(PokemonDocument d, int lvl)
    {
        var t1 = ParseType(d.Type1);
        var t2 = string.IsNullOrWhiteSpace(d.Type2) ? Type.Normal : ParseType(d.Type2);

        return new Pokemon(
            d.PokedexNumber,
            d.Name,
            lvl,
            t1,
            t2,
            300, // temp HP
            d.Attack,
            d.Defense,
            d.SpAttack,
            d.SpDefense,
            d.HeightM,
            d.WeightKg
        );
    }

    private static Type ParseType(string s)
        => Enum.TryParse<Type>(s, ignoreCase: true, out var t) ? t : Type.Normal;
}
