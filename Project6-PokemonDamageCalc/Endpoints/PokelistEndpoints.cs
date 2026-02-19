using Microsoft.AspNetCore.Authorization;
using Project6_PokemonDamageCalc.DataTransferObjs;
using Project6_PokemonDamageCalc.Services;
using System.Security.Claims;

namespace Project6_PokemonDamageCalc.Endpoints
{
    public static class PokelistEndpoints
    {
        public static void mapPokelistEndp(this WebApplication app)
        {
            var group = app.MapGroup("/api/pokelist").RequireAuthorization();

            group.MapGet("/active", async (PokelistService svc, ClaimsPrincipal user) =>
            {
                var accountId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(accountId)) return Results.Unauthorized();

                var list = await svc.GetOrCreateActiveAsync(accountId);
                return Results.Ok(list);
            });

            group.MapPost("/active/entry", async (PokelistService svc, ClaimsPrincipal user, PokelistDTOs.SaveEntryDTO dto) =>
            {
                var accountId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(accountId)) return Results.Unauthorized();

                var e = dto.entry;

                if (string.IsNullOrWhiteSpace(e.attackerName) || string.IsNullOrWhiteSpace(e.defenderName))
                    return Results.BadRequest(new { ok = false, error = "attackerName and defenderName required" });

                if (dto.entryIndex < 0 || dto.entryIndex > 2)
                    return Results.BadRequest(new { ok = false, error = "entryIndex must be 0,1,2" });

                var entry = new CalcEntry
                {
                    attackerName = e.attackerName.Trim(),
                    defenderName = e.defenderName.Trim(),
                    moveType = e.moveType,
                    category = e.category,
                    power = e.power,
                    attackerLevel = e.attackerLevel,
                    defenderLevel = e.defenderLevel,
                    createdUtc = DateTime.UtcNow
                };

                var list = await svc.AddOrReplaceEntryAsync(accountId, dto.entryIndex, entry);
                return Results.Ok(list);
            });

            group.MapDelete("/active", async (PokelistService svc, ClaimsPrincipal user) =>
            {
                var accountId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(accountId)) return Results.Unauthorized();

                var ok = await svc.ClearAsync(accountId);
                return ok ? Results.NoContent() : Results.NotFound();
            });
        }
    }
}
