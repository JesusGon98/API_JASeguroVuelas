using API_JASeguroVuelas.Models;
using MongoDB.Driver;

namespace API_JASeguroVuelas.Services
{
    public class DestinoService
    {
        private readonly IMongoCollection<Destino> _destinos;

        public DestinoService(IMongoDatabase database)
        {
            _destinos = database.GetCollection<Destino>("Destinos");
        }

        public async Task<List<Destino>> GetAsync()
        {
            return await _destinos.Find(_ => true).ToListAsync();
        }

        public async Task<Destino?> GetByIdAsync(string id)
        {
            var filter = Builders<Destino>.Filter.Eq(d => d.Id, id);
            return await _destinos.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Destino> CreateAsync(Destino destino)
        {
            await _destinos.InsertOneAsync(destino);
            return destino;
        }

        public async Task UpdateAsync(string id, Destino destino)
        {
            var filter = Builders<Destino>.Filter.Eq(d => d.Id, id);
            await _destinos.ReplaceOneAsync(filter, destino);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<Destino>.Filter.Eq(d => d.Id, id);
            await _destinos.DeleteOneAsync(filter);
        }
    }
}
