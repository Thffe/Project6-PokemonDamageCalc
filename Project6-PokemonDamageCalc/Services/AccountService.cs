using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Project6_PokemonDamageCalc;

//endpoint -> service -> db
namespace Project6_PokemonDamageCalc.Services
{
    public class AccountService
    {
        private readonly IMongoCollection<Account> accounts;

        // read mongo settings from app config
        public AccountService(IOptions<MongoPrep> settings) 
        {
            //create client connection
            var client = new MongoClient(settings.Value.ConnectionString);
            var db = client.GetDatabase(settings.Value.DatabaseName);
            accounts = db.GetCollection<Account>("accounts");
        }

        //get all accounts, limit 20(?)
        public async Task<List<Account>> getAllAsyncAccount(int limit = 20)
        => await accounts.Find(_ => true).Limit(limit).ToListAsync();

        //get acc by ID
        public async Task<Account?> getAccountByID(int id)
            => await accounts.Find(a => a.accountID == id).FirstOrDefaultAsync();

        //create new acc
        public async Task<Account> createAccountAsync(Account account)
        {
            await accounts.InsertOneAsync(account);
            return account;
        }

        //replace acc, PUT
        public async Task<bool> replaceAccountAsync(int id, Account newAcc)
        {
            newAcc.accountID = id;
            var result = await accounts.ReplaceOneAsync(a=>a.accountID==id, newAcc);
            return result.MatchedCount==1;

        }

        //delete acc
        public async Task<bool> deleteAccountAsync(int id)
        {
            var result = await accounts.DeleteOneAsync(a => a.accountID==id);
            return result.DeletedCount==1;
        }
    }
}
