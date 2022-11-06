using Authorisation2.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Authorisation2.Entities;
using Authorisation2.DataBase;
using Authorisation2.Model;

namespace Authorisation2.Controllers;

[Route ("Api/[Controller]")]
[ApiController]
public class LoginController : Controller
{
    private readonly JsonDB jsonDB;
    public LoginController (JsonDB _jsonDB)
    {
        jsonDB = _jsonDB;
    }

    [HttpPost]
    public IActionResult AddUser(userModel usermodel)
    {
        var key = Guid.NewGuid().ToString("N").Substring(10);
        var user = new User()
        {
            Email = usermodel.Email,
            Name = usermodel.Name,
            Password = usermodel.Password,
            Role = usermodel.Role,
            Key = key,
        };
        
        var userList = jsonDB.ReadFromJson() ?? new List<User>();
        userList.Add(user);
        jsonDB.WriteToJson(userList);
        return Ok(key);
    }

    //[TypeFilter(typeof(AuthFilterAttribute))]
    ///[TypeFilter(typeof(AuthFilterAttribute), Arguments = new object[] {"Admin"})]
    ///[Role ("Admin")] dedani yuqoridagini alohida classga olib chiqib qoyilgani xolos
    [HttpGet]
    [Role("Admin")]
    public IActionResult GetAdminInfo ()
    {
        var mail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
        return Ok(mail?.Value);
    }


    [HttpGet("public")]
    [Role("Admin,User")]
    public IActionResult GetUserInfo()
    {
        var name = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
        var role = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
        return Ok(name?.Value + " v " + role?.Value);
    }
}