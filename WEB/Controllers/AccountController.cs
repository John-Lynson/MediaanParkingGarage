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
using DALL.Context;
using WEB.Controllers;

public class AccountController : Controller
{
    private readonly AccountService _accountService;

    public AccountController(GarageContext context)
    {
        this._accountService = new AccountService(new AccountRepository(context));
    }

    public async Task Login()
    {
        var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(Url.Action(nameof(this.Auth0Callback))) // Triggers the Auth0Callback-method in this controller
            .Build();

        await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    }

    public RedirectResult Auth0Callback() // Auth0 login callback
    {
        if (this.User.Identity.IsAuthenticated) // Is user logged in?
        {
            string? auth0Id = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value; // Get Auth0Id. Never NULL here, because it passed IsAuthenticated().
            Account account = this._accountService.GetAccountByAuth0Id(auth0Id);
            if (account == null) // Does account exist? If not, create new account in the database.
            {
                string? username = this.User.Claims.FirstOrDefault(c => c.Type == "nickname")?.Value;
                account = this._accountService.Create(username, auth0Id, ""); // TODO: hash or generate password
            }
        }

        return Redirect("/home");
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
