using Microsoft.AspNetCore.Mvc;
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

    public IndexModel(IMongoCollection<PokemonDocument> pokemon, PokelistService pokelistService)
    {
        _pokemon = pokemon;
        _pokelistService = pokelistService;
    }

    public Pokemon? Attacker { get; private set; }
    public Pokemon? Defender { get; private set; }

    // Active list for user
    public Pokelist? ActiveList { get; private set; }

    // Form fields
    [BindProperty] public int EntryIndex { get; set; } = 0; // 0,1,2
    [BindProperty] public string AttackerName { get; set; } = "";
    [BindProperty] public string DefenderName { get; set; } = "";
    [BindProperty] public string MoveType { get; set; } = "Normal";
    [BindProperty] public int Power { get; set; } = 85;
    [BindProperty] public int AttackerLevel { get; set; } = 100;
    [BindProperty] public int DefenderLevel { get; set; } = 100;
    [BindProperty] public string Category { get; set; } = "physical";

    public double? LastDamagePercent { get; private set; }
    public string? UiMessage { get; private set; }

    public async Task OnGetAsync()
    {
        await LoadActiveListIfLoggedIn();

        // If user has entries, prefill with slot 0 to make it feel integrated
        if (ActiveList?.entries.Count > 0)
        {
            var e = ActiveList.entries[0];
            EntryIndex = 0;
            AttackerName = e.attackerName;
            DefenderName = e.defenderName;
            MoveType = e.moveType;
            Category = e.category;
            Power = e.power;
            AttackerLevel = e.attackerLevel;
            DefenderLevel = e.defenderLevel;

            await LoadPokemonStatsForCurrentForm();
        }
    }

    public async Task<IActionResult> OnPostCalculateAndSaveAsync()
    {
        if (User.Identity?.IsAuthenticated != true)
            return RedirectToPage("/Account");

        var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(accountId))
            return RedirectToPage("/Account");

        if (EntryIndex < 0 || EntryIndex > 2)
        {
            UiMessage = "Entry slot must be 1, 2, or 3.";
            await LoadActiveListIfLoggedIn();
            return Page();
        }

        if (string.IsNullOrWhiteSpace(AttackerName) || string.IsNullOrWhiteSpace(DefenderName))
        {
            UiMessage = "Attacker and Defender are required.";
            await LoadActiveListIfLoggedIn();
            return Page();
        }

        if (Power <= 0) Power = 1;
        if (AttackerLevel <= 0) AttackerLevel = 100;
        if (DefenderLevel <= 0) DefenderLevel = 100;

        var attackerDoc = await FindPokemonByNameCaseInsensitive(AttackerName);
        var defenderDoc = await FindPokemonByNameCaseInsensitive(DefenderName);

        if (attackerDoc is null || defenderDoc is null)
        {
            UiMessage = "Could not find one or both Pokémon. Try exact names.";
            await LoadActiveListIfLoggedIn();
            return Page();
        }

        var atk = MapToPokemon(attackerDoc, AttackerLevel);
        var def = MapToPokemon(defenderDoc, DefenderLevel);

        var typeEnum = ParseType(MoveType);
        var catEnum = string.Equals(Category, "special", StringComparison.OrdinalIgnoreCase)
            ? Movecategory.special
            : Movecategory.physical;

        var categoryString = (catEnum == Movecategory.special) ? "special" : "physical";

        var move = new Move(Power, typeEnum, catEnum);
        var dmgPct = DamageCalc.PrepareCalc(atk, def, move);

        LastDamagePercent = dmgPct;

        var entry = new CalcEntry
        {
            attackerName = attackerDoc.Name,
            defenderName = defenderDoc.Name,
            moveType = MoveType,
            category = categoryString,
            power = Power,
            attackerLevel = AttackerLevel,
            defenderLevel = DefenderLevel,
            damagePercent = dmgPct,
            createdUtc = DateTime.UtcNow
        };

        // Save into slot EntryIndex (0-2)
        ActiveList = await _pokelistService.AddOrReplaceEntryAsync(accountId, EntryIndex, entry);

        // Also update displayed stats
        Attacker = atk;
        Defender = def;

        UiMessage = $"Saved slot {EntryIndex + 1}: {Math.Round(dmgPct * 100, 2)}%";

        return Page(); // stay on page so you see result instantly
    }

    public async Task<IActionResult> OnPostClearListAsync()
    {
        if (User.Identity?.IsAuthenticated != true)
            return RedirectToPage("/Account");

        var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(accountId))
            return RedirectToPage("/Account");

        await _pokelistService.ClearAsync(accountId);
        return RedirectToPage();
    }

    private async Task LoadActiveListIfLoggedIn()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrWhiteSpace(accountId))
            {
                ActiveList = await _pokelistService.GetOrCreateActiveAsync(accountId);
            }
        }
    }

    private async Task LoadPokemonStatsForCurrentForm()
    {
        var attackerDoc = await FindPokemonByNameCaseInsensitive(AttackerName);
        var defenderDoc = await FindPokemonByNameCaseInsensitive(DefenderName);

        Attacker = attackerDoc is null ? null : MapToPokemon(attackerDoc, AttackerLevel);
        Defender = defenderDoc is null ? null : MapToPokemon(defenderDoc, DefenderLevel);
    }

    private async Task<PokemonDocument?> FindPokemonByNameCaseInsensitive(string name)
    {
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
            300, // TODO: replace with real HP when you add it to PokemonDocument
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
