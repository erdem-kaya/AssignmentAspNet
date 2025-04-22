﻿using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class CookiesController : Controller
{
    //Byggde det här stället med Hans's video -> Cookie Consent Management i ASP.NET
    [HttpPost]
    public IActionResult SetCookies([FromBody] CookieConsent consent)
    {
        Response.Cookies.Append("SessionCookie", "Essential", new CookieOptions
        {
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.AddYears(1),
        });

        if (consent == null)
            return BadRequest();

        if (consent.Functional)
        {
            Response.Cookies.Append("FunctionalCookie", "Non-Esential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Lax,
                Path = "/",
            });
        }
        else
        {
            Response.Cookies.Delete("FunctionalCookie");
        }

        if (consent.Analytics)
        {
            Response.Cookies.Append("AnalyticsCookie", "Non-Esential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Lax,
                Path = "/",
            });
        }
        else
        {
            Response.Cookies.Delete("AnalyticsCookie");
        }

        if (consent.Marketing)
        {
            Response.Cookies.Append("MarketingCookie", "Non-Esential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Lax,
                Path = "/",
            });
        }
        else
        {
            Response.Cookies.Delete("MarketingCookie");
        }

        Response.Cookies.Append("cookieConsent", JsonSerializer.Serialize(consent), new CookieOptions
        {
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.AddDays(90),
            SameSite = SameSiteMode.Lax,
            Path = "/",
        });
        return Ok();
    }
}
