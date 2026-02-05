using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Setup MongoDB Client
const string connectionUri = "mongodb+srv://group6admin:Secret55@pokedmgcalccluster.cklzyid.mongodb.net/?appName=PokeDmgCalcCluster";
var settings = MongoClientSettings.FromConnectionString(connectionUri);
// Set the ServerApi field of the settings object to set the version of the Stable API on the client
settings.ServerApi = new ServerApi(ServerApiVersion.V1);
// Create a new client and connect to the server
var client = new MongoClient(settings);
// Send a ping to confirm a successful connection
try
{
    var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
    Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

builder.Services.AddSingleton<IMongoDatabase>(sp => client.GetDatabase("admin"));

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapGet("/db-test", async ([FromServices] IMongoDatabase db) => {
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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
