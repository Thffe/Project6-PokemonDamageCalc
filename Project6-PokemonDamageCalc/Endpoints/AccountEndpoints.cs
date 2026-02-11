using Project6_PokemonDamageCalc.DataTransferObjs;
using Project6_PokemonDamageCalc.Services;
using static Project6_PokemonDamageCalc.DataTransferObjs.AccountDTOs;

namespace Project6_PokemonDamageCalc.Endpoints
{
    public static class AccountEndpoints
    {

        public static void mapAccEndp(this WebApplication app)
        {
            //base route, enpoint grouping
            var group = app.MapGroup("/api/accounts");

            //options for collection: GET, POST, OPTIONS
            group.MapMethods("", new[] { "OPTIONS" }, () =>
                Results.Json(new { allowedMethods = new[] { "GET", "POST", "OPTIONS" } })
            );

            // options for items: GET, PUT, OPTIONS
            group.MapMethods("/{id}", new[] { "OPTIONS" }, () =>
                Results.Json(new { allowedMethods = new[] { "GET", "PUT", "OPTIONS" } })
            );

            //GET /api/accounts
            group.MapGet("", async (AccountService svc, int? limit) =>
            {
                var items = await svc.getAllAsyncAccount(limit ?? 20);
                return Results.Json(new { items });
            });

            //GET /api/account/{id}
            group.MapGet("/{id}", async (AccountService svc, int id) =>
            {
                var account = await svc.getAccountByID(id);
                if (account == null) return Results.NotFound();
                return Results.Json(account);
            });

            // POST /api/accounts
            group.MapPost("", async (AccountService svc, accountCreateDTO dto) =>
            {
                if (string.IsNullOrWhiteSpace(dto.username))
                    return Results.BadRequest(new { ok = false, error = "username required" });

                var account = new Account
                {
                    username = dto.username.Trim(),
                };

                await svc.createAccountAsync(account);
                return Results.Created($"/api/accounts/{account.accountID}", account);
            });

            // PUT /api/accounts/{id}
            group.MapPut("/{id}", async (AccountService svc, int id, accountReplaceDTO dto) =>
            {
                if (string.IsNullOrWhiteSpace(dto.username))
                    return Results.BadRequest(new { ok = false, error = "username required" });

                var existing = await svc.getAccountByID(id);
                if (existing is null) return Results.NotFound();

                var replacement = new Account
                {
                    accountID = id,
                    username = dto.username.Trim(),
                    pfp = dto.pfp,
                    pfpType = dto.pfpType,
                };

                var ok = await svc.replaceAccountAsync(id, replacement);
                return ok ? Results.Json(replacement) : Results.NotFound();
            });
        }

    }
}
