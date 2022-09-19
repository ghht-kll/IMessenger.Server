using IMessenger.Server.Authorization;
using IMessenger.Server.Hubs;
using IMessenger.Server.Model;
using IMessenger.Server.Services;
using IMessenger.Server.Src;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
builder.Services.AddJwtAuthentication();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", async (LoginModel loginModel, ApplicationContext applicationContext) =>            
{
    User? user = await applicationContext?.Users?.FirstOrDefaultAsync(p => p.Login.Equals(loginModel.Login) && p.Password.Equals(loginModel.Password));

    if (user is null) return Results.Unauthorized();

    var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Userid) };

    var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

    AuthorizedUserModel? response = new AuthorizedUserModel(encodedJwt, user.Username, user.Userid);

    return Results.Json(response);
});

app.MapGet("/getAllMessages", [Authorize] async (HttpContext context, ApplicationContext applicationContext) =>
{
    var currentUser = context.User?.Identity?.Name;

    var messages = applicationContext?.Messages.Where(p => p.Senderid.Equals(currentUser) || p.Receiverid.Equals(currentUser)).ToList();
    await context.Response.WriteAsJsonAsync(messages);
});

app.MapGet("/getAllMessagesTest",  async (HttpContext context, ApplicationContext applicationContext) =>
{
    var messages = applicationContext?.Messages.Where(p => p.Senderid.Equals("@kggh") || p.Receiverid.Equals("@kggh")).ToList();
    var response = JsonSerializer.Serialize(messages);

    await context.Response.WriteAsync(response);
});

app.MapHub<ChatHub>("/chat");

app.Run();