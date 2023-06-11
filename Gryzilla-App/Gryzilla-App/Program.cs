using System.Text;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var includeSwaggerInPublish = true;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader() // Add this line to allow any header
                .AllowAnyMethod(); // Add this line to allow any HTTP method
                
        });
});

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gryzilla", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Here enter JWT Token with bearer format like bearer[space] token"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{ }
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //te 2 zakeomentować jak jesteś frontasiem
        ValidateIssuer = false,
        ValidateAudience = false,

        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers();

builder.Services.AddScoped<IUserDbRepository, UserDbRepository>();
builder.Services.AddScoped<IFriendsDbRepository, FriendsDbRepository>();
builder.Services.AddScoped<IPostDbRepository, PostDbRepository>();
builder.Services.AddScoped<ICommentPostDbRepository, CommentPostDbRepository>();
builder.Services.AddScoped<ILikesPostDbRepository, LikesPostDbRepository>();
builder.Services.AddScoped<ILikesArticleDbRepository, LikesArticleDbRepository>();
builder.Services.AddScoped<IRankDbRepository, RankDbRepository>();
builder.Services.AddScoped<IGroupDbRepository, GroupDbRepository>();
builder.Services.AddScoped<IArticleDbRepository, ArticleDbRepository>();
builder.Services.AddScoped<ICommentArticleDbRepository, CommentArticleDbRepository>();
builder.Services.AddScoped<ITagDbRepository, TagDbRepository>();
builder.Services.AddScoped<IReportCommentPostDbRepository, ReportCommentPostDbRepository>();
builder.Services.AddScoped<IReportCommentArticleDbRepository, ReportCommentArticleDbRepository>();
builder.Services.AddScoped<IReasonDbRepository, ReasonDbRepository>();
builder.Services.AddScoped<IProfileCommentDbRepository, ProfileCommentDbRepository>();
builder.Services.AddScoped<IBlockedUserDbRepository, BlockedUserDbRepository>();
builder.Services.AddScoped<ILinkDbRepository, LinkDbRepository>();
builder.Services.AddScoped<IReportPostDbRepository, ReportPostDbRepository>();
builder.Services.AddScoped<IReportUserDbRepository, ReportUserDbRepository>();
builder.Services.AddScoped<IReportProfileCommentDbRepository, ReportProfileCommentDbRepository>();
builder.Services.AddScoped<IGroupUserMessageDbRepository, GroupUserMessageDbRepository>();
builder.Services.AddScoped<ISearchDbRepository, SearchDbRepository>();

var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["GryzillaDatabase"].ConnectionString;

builder.Services.AddDbContext<GryzillaContext>(options => 
    options.UseSqlServer(connectionString));
    
var app = builder.Build();

if (app.Environment.IsDevelopment() || includeSwaggerInPublish)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gryzilla v1"));
}

//app.Urls.Add("https://192.168.0.221:5001");
//app.Urls.Add("https://localhost:1337");

app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();
