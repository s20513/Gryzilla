using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins("http://localhost:3000",
                "https://localhost:3000", "http://localhost");
        });
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gryzilla", Version = "v1" });
});

//Adding controllers in .net 6
builder.Services.AddControllers();


builder.Services.AddScoped<IUserDbRepository, UserDbRepository>();
builder.Services.AddScoped<IFriendsDbRepository, FriendsDbRepository>();
builder.Services.AddScoped<IPostDbRepository, PostDbRepository>();
builder.Services.AddScoped<ICommentPostDbRepository, CommentPostDbRepository>();
builder.Services.AddScoped<ILikesPostDbRepository, LikesPostDbRepository>();
builder.Services.AddScoped<ILikesArticleDbRepository, LikesArticleDbRepository>();
 //trzeba zrobić
builder.Services.AddScoped<ITopCommentDbRepository, TopCommentDbRepository>();
builder.Services.AddScoped<IRankDbRepository, RankDbRepository>();
builder.Services.AddScoped<IAchievementDbRepository, AchievementDbRepository>();
builder.Services.AddScoped<IGroupDbRepository, GroupMssqlDbRepository>();
builder.Services.AddScoped<IArticleDbRepository, ArticleMssqlDbRepository>();
builder.Services.AddScoped<ICommentArticleDbRepository, CommentArticleMssqlDbRepository>();
builder.Services.AddScoped<ITagDbRepository, TagDbRepository>();


var connectionString = "Data Source=89.68.94.143,2105;Initial Catalog=Gryzilla;User ID=sa;Password=Poziomka100";
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

//app.Urls.Add("https://192.168.0.221:1337");
//app.Urls.Add("https://localhost:1337");

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(MyAllowSpecificOrigins);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();
