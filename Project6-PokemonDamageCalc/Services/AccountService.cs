using MongoDB.Driver;

namespace Project6_PokemonDamageCalc.Services
{
    public class AccountService
    {
        private readonly IMongoCollection<Account> _accounts;

        public AccountService(IMongoDatabase db)
        {
            _accounts = db.GetCollection<Account>("accounts");
            //// Ensure unique index on username (tolerant if index already exists)
            //try
            //{
            //    var existingIndexes = _accounts.Indexes.List().ToList();

            //    // Look for any index whose key is { username: 1 }
            //    var hasUsernameIndex = existingIndexes.Any(idx =>
            //        idx.Contains("key") &&
            //        idx["key"].AsBsonDocument.TryGetValue("username", out var v) &&
            //        v.ToInt32() == 1
            //    );

            //    if (!hasUsernameIndex)
            //    {
            //        var indexKeys = Builders<Account>.IndexKeys.Ascending(a => a.username);
            //        var indexModel = new CreateIndexModel<Account>(
            //            indexKeys,
            //            new CreateIndexOptions { Unique = true, Name = "ux_accounts_username" }
            //        );
            //        _accounts.Indexes.CreateOne(indexModel);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Don't block app startup in production if index ops fail
            //    Console.WriteLine("Index check/create failed: " + ex.Message);
            //}
        }

        private static string NormalizeUsername(string username)
            => username.Trim().ToLowerInvariant();

        public async Task<List<Account>> getAllAsyncAccount(int limit = 20)
            => await _accounts.Find(_ => true).Limit(limit).ToListAsync();

        public async Task<Account?> getAccountByID(string id)
            => await _accounts.Find(a => a.Id == id).FirstOrDefaultAsync();

        // Always search normalized
        public async Task<Account?> getAccountByUsername(string username)
        {
            var u = NormalizeUsername(username);
            return await _accounts.Find(a => a.username == u).FirstOrDefaultAsync();
        }

        // Returns (createdAccount, errorMessage)
        public async Task<(Account? account, string? error)> createAccountAsync(string username)
        {
            var normalized = NormalizeUsername(username);

            var account = new Account
            {
                username = normalized
            };

            try
            {
                await _accounts.InsertOneAsync(account);
                return (account, null);
            }
            catch (MongoWriteException ex) when (ex.WriteError?.Category == ServerErrorCategory.DuplicateKey)
            {
                return (null, "Username already exists. Please choose a different one.");
            }
        }

        public async Task<bool> replaceAccountAsync(string id, Account newAcc)
        {
            newAcc.Id = id;
            // keep normalized username if present
            if (!string.IsNullOrWhiteSpace(newAcc.username))
                newAcc.username = NormalizeUsername(newAcc.username);

            var result = await _accounts.ReplaceOneAsync(a => a.Id == id, newAcc);
            return result.MatchedCount == 1;
        }

        public async Task<bool> patchAccountAsync(string id, string? username, string? pfp, string? pfpType)
        {
            var updates = new List<UpdateDefinition<Account>>();

            if (!string.IsNullOrWhiteSpace(username))
                updates.Add(Builders<Account>.Update.Set(a => a.username, NormalizeUsername(username)));

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

        public async Task<bool> updateProfilePicAsync(string accountId, string pfpUrl, string contentType)
        {
            var update = Builders<Account>.Update
                .Set(a => a.pfp, pfpUrl)
                .Set(a => a.pfpType, contentType);

            var result = await _accounts.UpdateOneAsync(a => a.Id == accountId, update);
            return result.MatchedCount == 1;
        }
    }
}
