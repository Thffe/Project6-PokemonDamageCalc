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

        public async Task<Pokelist> GetOrCreateActiveAsync(string accountId)
        {
            var existing = await _pokelists.Find(p => p.accountId == accountId).FirstOrDefaultAsync();
            if (existing != null) return existing;

            var created = new Pokelist
            {
                accountId = accountId,
                createdUtc = DateTime.UtcNow,
                updatedUtc = DateTime.UtcNow,
                entries = new List<CalcEntry>()
            };

            await _pokelists.InsertOneAsync(created);
            return created;
        }

        // entryIndex: 0,1,2 (max 3 entries)
        public async Task<Pokelist> AddOrReplaceEntryAsync(string accountId, int entryIndex, CalcEntry entry)
        {
            if (entryIndex < 0 || entryIndex > 2)
                throw new ArgumentOutOfRangeException(nameof(entryIndex), "entryIndex must be 0, 1, or 2.");

            var list = await GetOrCreateActiveAsync(accountId);

            // Ensure list has correct size up to entryIndex
            while (list.entries.Count <= entryIndex)
                list.entries.Add(new CalcEntry());

            list.entries[entryIndex] = entry;
            list.updatedUtc = DateTime.UtcNow;

            await _pokelists.ReplaceOneAsync(p => p.Id == list.Id, list);
            return list;
        }

        public async Task<Pokelist?> GetByIdAsync(string id)
            => await _pokelists.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task<bool> DeleteByIdAsync(string id)
        {
            var result = await _pokelists.DeleteOneAsync(p => p.Id == id);
            return result.DeletedCount == 1;
        }

        public async Task<bool> ClearAsync(string accountId)
        {
            var list = await _pokelists.Find(p => p.accountId == accountId).FirstOrDefaultAsync();
            if (list == null) return false;

            list.entries.Clear();
            list.updatedUtc = DateTime.UtcNow;

            await _pokelists.ReplaceOneAsync(p => p.Id == list.Id, list);
            return true;
        }
    }
}
