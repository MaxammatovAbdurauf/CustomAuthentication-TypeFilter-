using Authorisation2.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;

namespace Authorisation2.Attributes;

public class AuthFilterAttribute : ActionFilterAttribute
{
    private readonly JsonDB jsonDB;
    private readonly List <string> Roles;
    public AuthFilterAttribute (JsonDB _jsonDB, List <string> args)
    {
        jsonDB = _jsonDB;
        Roles = args;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.Headers.ContainsKey (HeaderNames.Authorization))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var authorisation = context.HttpContext.Request.Headers[HeaderNames.Authorization];

        if (jsonDB.ReadFromJson()?.FirstOrDefault(x =>x.Key == authorisation) == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var user = jsonDB.ReadFromJson()!.First(x => x.Key == authorisation);

        if (!Roles.Contains(user.Role!))
        {
            context.Result = new JsonResult(new {error = "access denied"});
            return;
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name,user.Name),
            new Claim("Password",$"{user.Password}"),
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(ClaimTypes.Role,user.Role),
        };

        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        context.HttpContext.User = principal;
    }
}
