using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project6_PokemonDamageCalc.DataTransferObjs;
using System.Net.Http.Json;
using static Project6_PokemonDamageCalc.DataTransferObjs.AccountDTOs;

namespace Project6_PokemonDamageCalc.Pages;

public class AccountModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public string Username { get; set; } = "";

    public accountDTO? CurrentAccount { get; set; }
    public string? Message { get; set; }

    public void OnGet()
    {
        // optional: you could read a cookie/session later
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var client = _httpClientFactory.CreateClient("api");

        // You already built DTOs/endpoints — this assumes POST /api/accounts creates or returns existing
        var res = await client.PostAsJsonAsync("/api/accounts", new accountCreateDTO(Username));

        if (!res.IsSuccessStatusCode)
        {
            Message = $"Account create/login failed: {(int)res.StatusCode}";
            return Page();
        }

        CurrentAccount = await res.Content.ReadFromJsonAsync<accountDTO>();
        Message = "Account loaded.";
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync([FromForm] string id)
    {
        var client = _httpClientFactory.CreateClient("api");

        var res = await client.DeleteAsync($"/api/accounts/{id}");
        Message = res.IsSuccessStatusCode ? "Account deleted." : $"Delete failed: {(int)res.StatusCode}";
        CurrentAccount = null;

        return Page();
    }
}
