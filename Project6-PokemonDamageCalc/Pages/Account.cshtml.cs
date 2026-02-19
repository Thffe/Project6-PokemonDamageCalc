using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project6_PokemonDamageCalc.Services;
using System.Security.Claims;

namespace Project6_PokemonDamageCalc.Pages;

public class AccountModel : PageModel
{
    private readonly AccountService _accountService;

    public AccountModel(AccountService accountService)
    {
        _accountService = accountService;
    }

    [BindProperty]
    public string Username { get; set; } = "";

    public string? Message { get; set; }

    public string? CurrentUsername { get; set; }
    public string? CurrentAccountId { get; set; }

    public async Task OnGetAsync()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            CurrentUsername = User.Identity.Name;
            CurrentAccountId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // If you want to verify it still exists in DB:
            // var acc = await _accountService.getAccountByID(CurrentAccountId!);
            // if (acc is null) { await HttpContext.SignOutAsync(); }
        }
    }

    // REGISTER
    public async Task<IActionResult> OnPostRegisterAsync()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            Message = "Username is required.";
            return Page();
        }

        var (account, error) = await _accountService.createAccountAsync(Username);
        if (account is null)
        {
            Message = error ?? "Registration failed.";
            return Page();
        }

        await SignInAccount(account.Id!, account.username);
        Message = "Registered and logged in.";
        return RedirectToPage();
    }

    // LOGIN
    public async Task<IActionResult> OnPostLoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            Message = "Username is required.";
            return Page();
        }

        var acc = await _accountService.getAccountByUsername(Username);
        if (acc is null)
        {
            Message = "Account not found. Register first.";
            return Page();
        }

        await SignInAccount(acc.Id!, acc.username);
        Message = "Logged in.";
        return RedirectToPage();
    }

    // LOGOUT
    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Message = "Logged out.";
        return RedirectToPage();
    }

    private async Task SignInAccount(string id, string username)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Name, username),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = true }
        );
    }
}
