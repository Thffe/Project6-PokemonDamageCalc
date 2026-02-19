using Project6_PokemonDamageCalc.Services;
using static Project6_PokemonDamageCalc.DataTransferObjs.AccountDTOs;

namespace Project6_PokemonDamageCalc.Endpoints
{
    public static class AccountEndpoints
    {
        public static void mapAccEndp(this WebApplication app)
        {
            var group = app.MapGroup("/api/accounts");

            // OPTIONS for collection
            group.MapMethods("", new[] { "OPTIONS" }, () =>
                Results.Json(new { allowedMethods = new[] { "GET", "POST", "OPTIONS" } })
            );

            // OPTIONS for item
            group.MapMethods("/{id}", new[] { "OPTIONS" }, () =>
                Results.Json(new { allowedMethods = new[] { "GET", "PUT", "PATCH", "DELETE", "OPTIONS" } })
            );

            // GET /api/accounts?limit=20
            group.MapGet("", async (AccountService svc, int? limit) =>
            {
                var items = await svc.getAllAsyncAccount(limit ?? 20);
                return Results.Json(new { items });
            });

            // GET /api/accounts/{id}
            group.MapGet("/{id}", async (AccountService svc, string id) =>
            {
                var account = await svc.getAccountByID(id);
                return account is null ? Results.NotFound() : Results.Json(account);
            });

            // GET /api/accounts/by-username/{username}
            group.MapGet("/by-username/{username}", async (AccountService svc, string username) =>
            {
                var acc = await svc.getAccountByUsername(username.Trim());
                return acc is null ? Results.NotFound() : Results.Json(acc);
            });

            // POST /api/accounts
            group.MapPost("", async (AccountService svc, accountCreateDTO dto) =>
            {
                if (string.IsNullOrWhiteSpace(dto.username))
                    return Results.BadRequest(new { ok = false, error = "username required" });

                var (account, error) = await svc.createAccountAsync(dto.username);

                if (account is null)
                    return Results.Conflict(new { ok = false, error = error ?? "Username already exists." });

                return Results.Created($"/api/accounts/{account.Id}", account);
            });

            // PUT /api/accounts/{id}
            group.MapPut("/{id}", async (AccountService svc, string id, accountReplaceDTO dto) =>
            {
                if (string.IsNullOrWhiteSpace(dto.username))
                    return Results.BadRequest(new { ok = false, error = "username required" });

                var existing = await svc.getAccountByID(id);
                if (existing is null) return Results.NotFound();

                var replacement = new Account
                {
                    Id = id,
                    username = dto.username.Trim(),
                    pfp = dto.pfp,
                    pfpType = dto.pfpType
                };

                var ok = await svc.replaceAccountAsync(id, replacement);
                return ok ? Results.Json(replacement) : Results.NotFound();
            });

            // PATCH /api/accounts/{id}
            group.MapPatch("/{id}", async (AccountService svc, string id, accountReplaceDTO dto) =>
            {
                var existing = await svc.getAccountByID(id);
                if (existing is null) return Results.NotFound();

                // For baseline, reuse accountReplaceDTO as “patch input”
                var ok = await svc.patchAccountAsync(id, dto.username, dto.pfp, dto.pfpType);
                return ok ? Results.Ok(new { ok = true }) : Results.BadRequest(new { ok = false, error = "No fields to update" });
            });

            // DELETE /api/accounts/{id}
            group.MapDelete("/{id}", async (AccountService svc, string id) =>
            {
                var ok = await svc.deleteAccountAsync(id);
                return ok ? Results.NoContent() : Results.NotFound();
            });
        }
    }
}
