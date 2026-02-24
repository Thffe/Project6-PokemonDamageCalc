using Project6_PokemonDamageCalc.Services;
using System.Security.Claims;

namespace Project6_PokemonDamageCalc.Services
{
    public class CurrentAccountViewService
    {
        private readonly AccountService _accountService;

        public CurrentAccountViewService(AccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<(string? username, string? pfpUrl)> GetAsync(ClaimsPrincipal user)
        {
            if (user.Identity?.IsAuthenticated != true)
                return (null, null);

            var accountId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(accountId))
                return (null, null);

            var acc = await _accountService.getAccountByID(accountId);
            if (acc is null)
                return (null, null);

            return (acc.username, acc.pfp);
        }
    }
}
