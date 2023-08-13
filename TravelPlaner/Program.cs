using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelPlaner;
using TravelPlaner.Services;
using TravelPlanerLib.Entities;
using TravelPlanerLib.Models;

var builder = WebApplication.CreateBuilder();

string connection = builder.Configuration.GetConnectionString("TravelPlanerConnection");

builder.Services.AddDbContext<TravelPlanerDbContext>(options => options.UseSqlServer(connection));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/login");
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/usersList", [Authorize] (TravelPlanerDbContext db) => db.User.ToList());

app.MapGet("/", (HttpContext context) => 
{
    var user = context.User.Identity;
    if(user is not null &&  user.IsAuthenticated)
    {
        return Results.Redirect("/usersList");
    }
    else
    {
        return Results.Redirect("/login");
    }
});
app.MapGet("/registration", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    string registrationForm = @"<!DOCTYPE html>
    <html>
    <head>
        <meta charset='utf-8' />
        <title>METANIT.COM</title>
    </head>
    <body>
        <h2>Login Form</h2>
        <form method='post'>
            <p>
                <label>ФИО пользователя</label><br />
                <input name='name' />
            </p>
            <p>
                <label>Логин</label><br />
                <input name='login' />
            </p>
            <p>
                <label>Email</label><br />
                <input type='email' name='email' />
            </p>
            <p>
                <label>Пароль</label><br />
                <input type='password' name='password' />
            </p>
            <p>
                <label>Повторите пароль</label><br />
                <input type='password' name='passwordConfirm' />
            </p>
            <input type='submit' value='registration' />
        </form>
    </body>
    </html>";
    await context.Response.WriteAsync(registrationForm);
});

app.MapPost("/registration", (string? returnUrl, HttpContext context, TravelPlanerDbContext db) =>
{
    var constants = new Constants();
    var form = context.Request.Form;
    if (form["name"] == string.Empty || 
        form["login"] == string.Empty || 
        form["password"] == string.Empty || 
        form["email"] == string.Empty || 
        form["passwordConfirm"] == string.Empty)
        return Results.BadRequest(constants.RequiredFieldsError);

    string name = form["name"];
    string login = form["login"];
    string password = form["password"];
    string email = form["email"];
    string passwordConfirm = form["passwordConfirm"];

    if(passwordConfirm != password)
        return Results.BadRequest(constants.PasswordNotCoincidedError);

    var userRegister = new RegisterViewModel()
    {
        Name = name,
        Login = login,
        Email = email,
        Password = password,
        PasswordConfirm = passwordConfirm
    };
    
    if(userRegister != null)
    {
        var existingUser = db.User.FirstOrDefault(e => e.Email == email || e.Login == login);
        if (existingUser != null)
        {
            return Results.BadRequest(constants.ExistingUserForEmailOrLoginError);
        }
        else
        {
            var user = new User()
            {
                Name = name,
                Login = login,
                Email = email,
                Password = password
            };
            db.User.Add(user);
            db.SaveChanges();

            var emailService = new EmailService();
            emailService.SendEmail(email, constants.EndRegistrationNotificationSubject, constants.EndRegistrationNotificationBody);
        }
    }

    return Results.Redirect(returnUrl ?? "/login");
});

app.MapGet("/login", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    string loginForm = @"<!DOCTYPE html>
    <html>
    <head>
        <meta charset='utf-8' />
        <title>METANIT.COM</title>
    </head>
    <body>
        <h2>Login Form</h2>
        <form method='post'>
            <p>
                <label>Login</label><br />
                <input name='login' />
            </p>
            <p>
                <label>Password</label><br />
                <input type='password' name='password' />
            </p>
            <input type='submit' value='Login' />
        </form>
    </body>
    </html>";
    await context.Response.WriteAsync(loginForm);
});

app.MapPost("/login", async (string? returnUrl, HttpContext context, TravelPlanerDbContext db) =>
{
    var constants = new Constants();
    var form = context.Request.Form;
    if (!form.ContainsKey("login") || !form.ContainsKey("password"))
        return Results.BadRequest(constants.LoginOrPasswordNotFound);

    string login = form["login"];
    string password = form["password"];

    var user = db.User.FirstOrDefault(p => p.Login == login && p.Password == password);
    if (user is null) return Results.Unauthorized();

    var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login) };
    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    return Results.Redirect(returnUrl ?? "/usersList");
});

app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
});

app.Run();