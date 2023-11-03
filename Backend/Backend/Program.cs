using System.Text;
using Backend.Application;
using Backend.Application.AuctionData.IRepository;
using Backend.Application.AuctionData.UseCases;
using Backend.Application.LotData.IRepository;
using Backend.Application.LotData.UseCases;
using Backend.Application.UserData.IRepository;
using Backend.Application.UserData.UseCases;
using Backend.Controllers;
using Backend.Database.PostgreSQL;
using Backend.Database.Repositories;
using Backend.Hubs;
using Backend.Notifications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<PgsqlConnection>()
    .Bind(builder.Configuration.GetSection("Config:PgsqlConnection"));

builder.Services.AddOptions<AuthorityHandler>()
    .Bind(builder.Configuration.GetSection("Config:AuthorityHandler"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,

            ValidateAudience = false,
            ValidateIssuer = false,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes("db3OIsj+BXE9NZDy0t8W3TcNekrF+2")
            ),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = long.MaxValue; });

builder.Services.AddSingleton<IAuctionRepository, AuctionRepository>();
builder.Services.AddSingleton<ILotRepository, LotRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.AddSingleton<PgsqlHandler>();

builder.Services.AddSingleton<ChangeAuctionStatusHandler>();
builder.Services.AddSingleton<SetDateEndAuctionHandler>();
builder.Services.AddSingleton<SetDateStartAuctionHandler>();

builder.Services.AddSingleton<CreateAuctionHandler>();
builder.Services.AddSingleton<DeleteAuctionHandler>();
builder.Services.AddSingleton<GetAuctionsHandler>();
builder.Services.AddSingleton<GetAuctionByIdHandler>();
builder.Services.AddSingleton<UpdateAuctionHandler>();

builder.Services.AddSingleton<BuyoutLotHandler>();
builder.Services.AddSingleton<ChangeLotStatusHandler>();
builder.Services.AddSingleton<DoBetHandler>();
builder.Services.AddSingleton<GetLotsHandler>();

builder.Services.AddSingleton<CreateLotHandler>();
builder.Services.AddSingleton<DeleteLotHandler>();
builder.Services.AddSingleton<UpdateLotHandler>();
builder.Services.AddSingleton<GetLotsByAuctionHandler>();

builder.Services.AddSingleton<SignUpUserHandler>();
builder.Services.AddSingleton<SignInUserHandler>();
builder.Services.AddSingleton<SignOutUserHandler>();
builder.Services.AddSingleton<AuthorityHandler>();

builder.Services.AddSingleton<DeleteUserHandler>();
builder.Services.AddSingleton<GetUsersHandler>();
builder.Services.AddSingleton<GetUserByIdHandler>();
builder.Services.AddSingleton<UpdateUserHandler>();

builder.Services.AddSingleton<INotificationHandler, NotificationHandler>();
builder.Services.AddSingleton<AuctionController>();
builder.Services.AddSingleton<LotController>();
builder.Services.AddSingleton<UserController>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .WithOrigins(
        "localhost",
        "localhost:3000")
    .AllowAnyHeader()
    .AllowCredentials()
    .SetIsOriginAllowed(_ => true));

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.Use(async (context, next) =>
{
    var token = context.Request.Cookies[".AspNet.Application.Id"];
    if (!string.IsNullOrEmpty(token))
        context.Request.Headers.Add("Authorization", "Bearer " + token);

    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.MapHub<AuctionHub>("/auction");

app.Run();