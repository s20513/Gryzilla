using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gryzilla", Version = "v1" });
});
//AddScoped
builder.Services.AddScoped<IUserDbRepository, UserDbRepository>();
builder.Services.AddScoped<IFriendsDbRepository, FriendsDbRepository>();
builder.Services.AddScoped<IPostDbRepository, PostDbRepository>();
builder.Services.AddScoped<ICommentPostDbRepository, CommentPostDbRepository>();
builder.Services.AddScoped<ILikesPostDbRepository, LikesPostDbRepository>();
//trzeba zrobić
builder.Services.AddScoped<ITopCommentDbRepository, TopCommentDbRepository>();
builder.Services.AddScoped<IRankDbRepository, RankDbRepository>();
builder.Services.AddScoped<IAchievementDbRepository, AchievementDbRepository>();

var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["GryzillaDatabase-Global"].ConnectionString;
//DbContext -> MssqlDbConnString
builder.Services.AddDbContext<GryzillaContext>(options => 
    options.UseSqlServer(connectionString));
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gryzilla v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
