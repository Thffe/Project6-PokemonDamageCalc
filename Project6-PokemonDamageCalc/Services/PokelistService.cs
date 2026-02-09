using Microsoft.Extensions.Options;
using MongoDB.Driver;
namespace Project6_PokemonDamageCalc.Services
{
    public class PokelistService
    {
        private readonly IMongoCollection<Pokelist> pokelists;
        public PokelistService(IOptions<MongoPrep>settings) 
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var db = client.GetDatabase(settings.Value.DatabaseName);
            pokelists = db.GetCollection<Pokelist>("pokelists");
        }

        //get all pokelist limit 50(?)
        public async Task<List<Pokelist>> getAllPokelistsAsync(int limit=50)
            => await pokelists.Find( p => true).Limit(limit).ToListAsync();

        //get pokelists from an account (acc id)
        public async Task<List<Pokelist>> getPokelistsFromAnAccount(int accID, int limit=2)
            => await pokelists.Find(p=>p.accountID==accID).Limit(limit).ToListAsync();

        //get pokelist by its id (teamID)
        public async Task<Pokelist?> getByTeamIDAsync(int id) 
            => await pokelists.Find(p => p.teamID == id).FirstOrDefaultAsync();

        //create a pokelist
        public async Task<Pokelist> createPokelistAsync(Pokelist list)
        {
            await pokelists.InsertOneAsync(list);
            return list;
        }

        //replace a pokelist
        public async Task<bool> replaceAsync(int id, Pokelist newTeam)
        {
            newTeam.teamID = id;
            var result = await pokelists.ReplaceOneAsync(p => p.teamID == id, newTeam);
            return result.MatchedCount == 1;
        }


    }
}
