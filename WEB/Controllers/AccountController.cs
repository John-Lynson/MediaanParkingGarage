using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using CORE.Entities;
using CORE.Services;
using System.Runtime.CompilerServices;
using CORE.Interfaces.IRepositories;
using DALL.Repositories;

public class AccountController : Controller
{
    private readonly AccountService _accountService;

    public AccountController(AccountRepository accountRepo)
    {
        this._accountService = new AccountService(accountRepo);
    }

    public async Task Login(string returnUrl = "/")
    {
        var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(returnUrl)
            .Build();

        await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        
        /*
        // Get Auth0 token
        string auth0Id = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        // Get the account from database. If it does not exist, then create new account.
        Account account = this._accountService.GetAccountByAuth0Id(auth0Id);
        if (account == null)
        {
            account = this._accountService.Create(this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value, auth0Id, "" ); // TODO: hash or generate password
        }

        //TODO: do something with variable account, like storing it for later use
        */
    }

    public async Task Logout()
    {
        var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            .WithRedirectUri(Url.Action("Index", "Home"))
            .Build();

        await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
