using Microsoft.Azure.Cosmos;
using UserProfiles;
using UserProfiles.Application.Repositories;
using UserProfiles.Application.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton(new CosmosClient(configuration.GetConnectionString(Constants.ConnetionStrings.CosmosDbConnectionString)));
builder.Services.AddSingleton<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddTransient<IUserProfileService, UserProfileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
