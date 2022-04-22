using Gryzilla_App.Repositories.Implementations;
using Gryzilla_App.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IUserDbRepository, UserDbRepository>();
builder.Services.AddControllers();

//connectionString
var connectionString = builder.Configuration.GetConnectionString("MssqlDbConnString");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
