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
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
    private readonly AccountService _accountService;
    private readonly GarageContext _dbContext;

    public AccountController(GarageContext context)
    {
        _accountService = new AccountService(new AccountRepository(context));
        _dbContext = context;
    }

    public async Task Login()
    {
        var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(Url.Action(nameof(this.Auth0Callback)))
            .Build();

        await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    }

    public RedirectResult Auth0Callback()
    {
        if (this.User.Identity.IsAuthenticated)
        {
            string? auth0Id = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            Account account = this._accountService.GetAccountByAuth0Id(auth0Id);
            if (account == null)
            {
                string? username = this.User.Claims.FirstOrDefault(c => c.Type == "nickname")?.Value;
                account = this._accountService.Create(username, auth0Id, "SecureHashedPassword"); // Example change
            }
        }

        return Redirect("/Home");
    }

    public async Task<IActionResult> About()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        Car userCar = await _dbContext.Cars.FirstOrDefaultAsync(c => c.AccountId == int.Parse(userId));

        if (userCar == null)
        {
            ViewData["LicensePlate"] = "Nog geen license plate toegevoegd.";
        }
        else
        {
            ViewData["LicensePlate"] = userCar.LicensePlate ?? "Nog geen license plate toegevoegd."; 
        }

        ViewData["Title"] = "About";
        ViewData["UserName"] = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
        ViewData["ProfilePictureUrl"] = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;

        return View();
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
