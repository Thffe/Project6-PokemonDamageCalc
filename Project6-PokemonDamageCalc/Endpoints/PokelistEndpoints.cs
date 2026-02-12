using Project6_PokemonDamageCalc.Services;
using static Project6_PokemonDamageCalc.DataTransferObjs.PokelistDTOs;

namespace Project6_PokemonDamageCalc.Endpoints
{
    public static class PokelistEndpoints
    {
        public static void mapPokelistEndp(this WebApplication app)
        {
            var group = app.MapGroup("/api/pokelist");

            group.MapMethods("", new[] { "OPTIONS" }, () =>
                Results.Json(new { allowedMethods = new[] { "GET", "POST", "OPTIONS" } })
            );

            group.MapMethods("/{id}", new[] { "OPTIONS" }, () =>
                Results.Json(new { allowedMethods = new[] { "GET", "PUT", "PATCH", "DELETE", "OPTIONS" } })
            );

            // GET /api/pokelist?accountId=<objectId>&limit=2
            group.MapGet("", async (PokelistService svc, string? accountId, int? limit) =>
            {
                var max = limit ?? 2;

                if (!string.IsNullOrWhiteSpace(accountId))
                {
                    var items = await svc.getPokelistsFromAnAccount(accountId.Trim(), max);
                    return Results.Json(new { items });
                }

                var all = await svc.getAllPokelistsAsync(max);
                return Results.Json(new { items = all });
            });

            // GET /api/pokelist/{id}
            group.MapGet("/{id}", async (PokelistService svc, string id) =>
            {
                var list = await svc.getByTeamIDAsync(id);
                return list is null ? Results.NotFound() : Results.Json(list);
            });

            // POST /api/pokelist
            group.MapPost("", async (PokelistService svc, PokelistCreateDTOs dto) =>
            {
                if (string.IsNullOrWhiteSpace(dto.accountId))
                    return Results.BadRequest(new { ok = false, error = "accountId required" });

                var list = new Pokelist
                {
                    accountId = dto.accountId.Trim(),
                    poke1id = dto.poke1id,
                    poke2id = dto.poke2id,
                    poke3id = dto.poke3id,
                    poke4id = dto.poke4id,
                    poke5id = dto.poke5id,
                    poke6id = dto.poke6id
                };

                await svc.createPokelistAsync(list);
                return Results.Created($"/api/pokelist/{list.Id}", list);
            });

            // PUT /api/pokelist/{id}
            group.MapPut("/{id}", async (PokelistService svc, string id, PokelistReplaceDTOs dto) =>
            {
                if (string.IsNullOrWhiteSpace(dto.accountId))
                    return Results.BadRequest(new { ok = false, error = "accountId required" });

                var existing = await svc.getByTeamIDAsync(id);
                if (existing is null) return Results.NotFound();

                var replacement = new Pokelist
                {
                    Id = id,
                    accountId = dto.accountId.Trim(),
                    poke1id = dto.poke1id,
                    poke2id = dto.poke2id,
                    poke3id = dto.poke3id,
                    poke4id = dto.poke4id,
                    poke5id = dto.poke5id,
                    poke6id = dto.poke6id
                };

                var ok = await svc.replaceAsync(id, replacement);
                return ok ? Results.Json(replacement) : Results.NotFound();
            });

            // PATCH /api/pokelist/{id}
            group.MapPatch("/{id}", async (PokelistService svc, string id, PokelistPatchDTOs dto) =>
            {
                var existing = await svc.getByTeamIDAsync(id);
                if (existing is null) return Results.NotFound();

                // Build a "patch" object (only set fields that were provided)
                var patch = new Pokelist
                {
                    accountId = dto.accountId ?? string.Empty,
                    poke1id = dto.poke1id,
                    poke2id = dto.poke2id,
                    poke3id = dto.poke3id,
                    poke4id = dto.poke4id,
                    poke5id = dto.poke5id,
                    poke6id = dto.poke6id
                };

                var ok = await svc.patchAsync(id, patch);
                return ok ? Results.Ok(new { ok = true }) : Results.BadRequest(new { ok = false, error = "No fields to update" });
            });

            // DELETE /api/pokelist/{id}
            group.MapDelete("/{id}", async (PokelistService svc, string id) =>
            {
                var ok = await svc.deleteAsync(id);
                return ok ? Results.NoContent() : Results.NotFound();
            });
        }
    }
}
