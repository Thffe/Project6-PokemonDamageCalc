using MongoDB.Driver;

namespace Project6_PokemonDamageCalc.Services
{
    public class AccountService
    {
        private readonly IMongoCollection<Account> _accounts;

        public AccountService(IMongoDatabase db)
        {
            _accounts = db.GetCollection<Account>("accounts");
        }

        public async Task<List<Account>> getAllAsyncAccount(int limit = 20)
            => await _accounts.Find(_ => true).Limit(limit).ToListAsync();

        public async Task<Account?> getAccountByID(string id)
            => await _accounts.Find(a => a.Id == id).FirstOrDefaultAsync();

        public async Task<Account?> getAccountByUsername(string username)
            => await _accounts.Find(a => a.username == username).FirstOrDefaultAsync();

        public async Task<Account> createAccountAsync(Account account)
        {
            await _accounts.InsertOneAsync(account);
            return account;
        }

        // PUT = replace whole document (except _id which stays the same)
        public async Task<bool> replaceAccountAsync(string id, Account newAcc)
        {
            newAcc.Id = id;
            var result = await _accounts.ReplaceOneAsync(a => a.Id == id, newAcc);
            return result.MatchedCount == 1;
        }

        // PATCH = update only some fields
        public async Task<bool> patchAccountAsync(string id, string? username, string? pfp, string? pfpType)
        {
            var updates = new List<UpdateDefinition<Account>>();

            if (!string.IsNullOrWhiteSpace(username))
                updates.Add(Builders<Account>.Update.Set(a => a.username, username.Trim()));

            if (pfp != null)
                updates.Add(Builders<Account>.Update.Set(a => a.pfp, pfp));

            if (pfpType != null)
                updates.Add(Builders<Account>.Update.Set(a => a.pfpType, pfpType));

            if (updates.Count == 0) return false;

            var update = Builders<Account>.Update.Combine(updates);
            var result = await _accounts.UpdateOneAsync(a => a.Id == id, update);

            return result.MatchedCount == 1;
        }

        public async Task<bool> deleteAccountAsync(string id)
        {
            var result = await _accounts.DeleteOneAsync(a => a.Id == id);
            return result.DeletedCount == 1;
        }
    }
}
