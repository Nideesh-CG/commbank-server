using CommBank.Models;
using CommBank.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// FORCED CONFIGURATION: Hardcoding your connection string parameters directly
var connectionString = "mongodb+srv://dbUser:CommbankPass2026@commbankcluster.zb1huaw.mongodb.net/?appName=CommbankCluster";
var databaseName = "commbank"; // matches your lowercase Atlas database

var mongoClient = new MongoClient(connectionString);
var mongoDatabase = mongoClient.GetDatabase(databaseName);

IAccountsService accountsService = new AccountsService(mongoDatabase);
IAuthService authService = new AuthService(mongoDatabase);
IGoalsService goalsService = new GoalsService(mongoDatabase);
ITagsService tagsService = new TagsService(mongoDatabase);
ITransactionsService transactionsService = new TransactionsService(mongoDatabase);
IUsersService usersService = new UsersService(mongoDatabase);

builder.Services.AddSingleton(accountsService);
builder.Services.AddSingleton(authService);
builder.Services.AddSingleton(goalsService);
builder.Services.AddSingleton(tagsService);
builder.Services.AddSingleton(transactionsService);
builder.Services.AddSingleton(usersService);

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(builder => builder
   .AllowAnyOrigin()
   .AllowAnyMethod()
   .AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Commented out to prevent local port redirect mismatches
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();