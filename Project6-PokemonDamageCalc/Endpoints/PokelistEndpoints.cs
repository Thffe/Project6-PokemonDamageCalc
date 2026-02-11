using Project6_PokemonDamageCalc.DataTransferObjs;
using Project6_PokemonDamageCalc.Services;
using static Project6_PokemonDamageCalc.DataTransferObjs.PokelistDTOs;

namespace Project6_PokemonDamageCalc.Endpoints
{
    public static class PokelistEndpoints
    {
        public static void mapPokelistEndp(this WebApplication app)
        {
            var group = app.MapGroup("/api/pokelist");

            //options for collections: get, post, options
            group.MapMethods("", new[] { "OPTIONS" }, () =>
                Results.Json(new { allowedMethods = new[] { "GET", "POST", "OPTIONS" } })
            );

            //options for items: get, put, option
            group.MapMethods("/{id}", new[] { "OPTIONS" }, () =>
                Results.Json(new { allowedMethods = new[] { "GET", "PUT", "OPTIONS" } })
            );

            //GET /api/pokelist
            group.MapGet("", async (PokelistService svc, int? accountId, int? limit) =>
            {
                var max = limit ?? 2;

                if (accountId.HasValue)
                {

                    if (accountId <= 0)
                        return Results.BadRequest(new { ok = false, error = "accountId must be a positive integer" });

                    var items = await svc.getPokelistsFromAnAccount(accountId.Value, max);
                    return Results.Json(new { items });
                }

                var all = await svc.getAllPokelistsAsync(max);
                return Results.Json(new { items = all });
            });

            // GET /api/pokelists/{id}
            group.MapGet("/{id}", async (PokelistService svc, int id) =>
            {
                var list = await svc.getByTeamIDAsync(id);
                return list is null ? Results.NotFound() : Results.Json(list);
            });

            // POST /api/pokelists
            group.MapPost("", async (PokelistService svc, PokelistCreateDTOs dto) =>
            {
                if (dto.accountID<=0)
                    return Results.BadRequest(new { ok = false, error = "accountId required" });

                var list = new Pokelist
                {
                    accountID = dto.accountID,
                    teamID = dto.teamID,

                    poke1id = dto.poke1ID,
                    poke2id = dto.poke2ID,
                    poke3id = dto.poke3ID,
                    poke4id = dto.poke4ID,
                    poke5id = dto.poke5ID,
                    poke6id = dto.poke6ID,

                    
                };
                await svc.createPokelistAsync(list);
                return Results.Created($"/api/pokelists/{list.teamID}", list);
            });

            // PUT /api/pokelists/{id}
            group.MapPut("/{id}", async (PokelistService svc, int id, PokelistReplaceDTOs dto) =>
            {
                if (dto.accountID==null)
                    return Results.BadRequest(new { ok = false, error = "accountId required" });

                var existing = await svc.getByTeamIDAsync(id);
                if (existing is null) return Results.NotFound();

                var replacement = new Pokelist
                {
                    teamID = id,
                    accountID = dto.accountID,

                    poke1id = dto.poke1ID,
                    poke2id = dto.poke2ID,
                    poke3id = dto.poke3ID,
                    poke4id = dto.poke4ID,
                    poke5id = dto.poke5ID,
                    poke6id = dto.poke6ID
                };

                var ok = await svc.replaceAsync(id, replacement);
                return ok ? Results.Json(replacement) : Results.NotFound();
            });
        }
    }
}
