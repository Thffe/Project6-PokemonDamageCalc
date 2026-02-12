using MongoDB.Driver;

namespace Project6_PokemonDamageCalc.Services
{
    public class PokelistService
    {
        private readonly IMongoCollection<Pokelist> _pokelists;

        public PokelistService(IMongoDatabase db)
        {
            _pokelists = db.GetCollection<Pokelist>("pokelists");
        }

        public async Task<List<Pokelist>> getAllPokelistsAsync(int limit = 50)
            => await _pokelists.Find(_ => true).Limit(limit).ToListAsync();

        public async Task<List<Pokelist>> getPokelistsFromAnAccount(string accountId, int limit = 2)
            => await _pokelists.Find(p => p.accountId == accountId).Limit(limit).ToListAsync();

        public async Task<Pokelist?> getByTeamIDAsync(string id)
            => await _pokelists.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task<Pokelist> createPokelistAsync(Pokelist list)
        {
            await _pokelists.InsertOneAsync(list);
            return list;
        }

        // PUT = replace whole list
        public async Task<bool> replaceAsync(string id, Pokelist newList)
        {
            newList.Id = id;
            var result = await _pokelists.ReplaceOneAsync(p => p.Id == id, newList);
            return result.MatchedCount == 1;
        }

        // PATCH = update only some fields (whatever is not null)
        public async Task<bool> patchAsync(string id, Pokelist patch)
        {
            var updates = new List<UpdateDefinition<Pokelist>>();

            if (!string.IsNullOrWhiteSpace(patch.accountId))
                updates.Add(Builders<Pokelist>.Update.Set(p => p.accountId, patch.accountId));

            if (patch.poke1id != null) updates.Add(Builders<Pokelist>.Update.Set(p => p.poke1id, patch.poke1id));
            if (patch.poke2id != null) updates.Add(Builders<Pokelist>.Update.Set(p => p.poke2id, patch.poke2id));
            if (patch.poke3id != null) updates.Add(Builders<Pokelist>.Update.Set(p => p.poke3id, patch.poke3id));
            if (patch.poke4id != null) updates.Add(Builders<Pokelist>.Update.Set(p => p.poke4id, patch.poke4id));
            if (patch.poke5id != null) updates.Add(Builders<Pokelist>.Update.Set(p => p.poke5id, patch.poke5id));
            if (patch.poke6id != null) updates.Add(Builders<Pokelist>.Update.Set(p => p.poke6id, patch.poke6id));

            if (updates.Count == 0) return false;

            var update = Builders<Pokelist>.Update.Combine(updates);
            var result = await _pokelists.UpdateOneAsync(p => p.Id == id, update);
            return result.MatchedCount == 1;
        }

        public async Task<bool> deleteAsync(string id)
        {
            var result = await _pokelists.DeleteOneAsync(p => p.Id == id);
            return result.DeletedCount == 1;
        }
    }
}
