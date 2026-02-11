using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Project6_PokemonDamageCalc;

var builder = WebApplication.CreateBuilder(args);

// Use env var in Docker / teammates machines:
//   MONGODB_URI="mongodb+srv://..."
// Fallback only for local dev (can remove fallback later).
var mongoUri =
    Environment.GetEnvironmentVariable("MONGODB_URI")
    ?? builder.Configuration["MongoDb:ConnectionString"]
    ?? throw new InvalidOperationException("Missing MongoDB connection string. Set MONGODB_URI or MongoDb:ConnectionString.");

var mongoDatabaseName =
    Environment.GetEnvironmentVariable("MONGODB_DB")
    ?? builder.Configuration["MongoDb:Database"]
    ?? "PokemonData";

var mongoCollectionName =
    Environment.GetEnvironmentVariable("MONGODB_COLLECTION")
    ?? builder.Configuration["MongoDb:Collection"]
    ?? "Pokemon";

var settings = MongoClientSettings.FromConnectionString(mongoUri);
settings.ServerApi = new ServerApi(ServerApiVersion.V1);

var client = new MongoClient(settings);

// Optional ping on startup
try
{
    client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
    Console.WriteLine("Connected to MongoDB (ping ok).");
}
catch (Exception ex)
{
    Console.WriteLine("MongoDB ping failed: " + ex.Message);
}

builder.Services.AddSingleton<IMongoClient>(client);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDatabaseName));
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IMongoDatabase>().GetCollection<PokemonDocument>(mongoCollectionName));

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Test endpoint
app.MapGet("/db-test", async ([FromServices] IMongoDatabase db) =>
{
    try
    {
        await db.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1));
        return Results.Ok("Database Connection Successful!");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Connection Failed: {ex.Message}");
    }
});

// API  test endpoint:
app.MapGet("/api/pokemon", async ([FromServices] IMongoCollection<PokemonDocument> col, [FromQuery] string name) =>
{
    if (string.IsNullOrWhiteSpace(name))
        return Results.BadRequest("Missing ?name=");

    var doc = await col.Find(x => x.Name == name).FirstOrDefaultAsync();
    return doc is null ? Results.NotFound() : Results.Ok(doc);
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
