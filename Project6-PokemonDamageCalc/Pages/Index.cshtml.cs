using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace Project6_PokemonDamageCalc.Pages;

public class IndexModel : PageModel
{
    private readonly IMongoCollection<PokemonDocument> _pokemon;

    public IndexModel(IMongoCollection<PokemonDocument> pokemon)
    {
        _pokemon = pokemon;
    }

    public Pokemon? Attacker { get; private set; }
    public Pokemon? Defender { get; private set; }

    public async Task OnGetAsync(string? attackerName = null, string? defenderName = null)
    {
        attackerName ??= "Gengar";
        defenderName ??= "Jirachi";

        var attackerDoc = await _pokemon.Find(p => p.Name == attackerName).FirstOrDefaultAsync();
        var defenderDoc = await _pokemon.Find(p => p.Name == defenderName).FirstOrDefaultAsync();

        // Level is not in your Mongo doc in a way you want to use for battles.
        // For baseline, keep level = 100 like your existing page.
        Attacker = attackerDoc is null ? null : MapToPokemon(attackerDoc, lvl: 100);
        Defender = defenderDoc is null ? null : MapToPokemon(defenderDoc, lvl: 89); // your old Jirachi was 89
    }

    private static Pokemon MapToPokemon(PokemonDocument d, int lvl)
    {
        // Your enum is lowercase. Dataset strings are Title case.
        var t1 = ParseType(d.Type1);
        var t2 = string.IsNullOrWhiteSpace(d.Type2) ? Type.Normal : ParseType(d.Type2);

        return new Pokemon(
            d.PokedexNumber,
            d.Name,
            lvl,
            t1,
            t2,
            //NOT ACTUALLY GETTING HEALTH CURRENTLY (TEMP FIX)
            300,
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
