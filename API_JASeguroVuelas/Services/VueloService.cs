using API_JASeguroVuelas.Models;
using MongoDB.Driver;

namespace API_JASeguroVuelas.Services
{
    public class VueloService
    {
        private readonly IMongoCollection<Vuelo> _vuelos;

        public VueloService(IMongoDatabase database)
        {
            _vuelos = database.GetCollection<Vuelo>("Vuelos");
        }

        public async Task<List<Vuelo>> GetAsync()
        {
            return await _vuelos.Find(_ => true).ToListAsync();
        }

        public async Task<Vuelo?> GetByIdAsync(string id)
        {
            var filter = Builders<Vuelo>.Filter.Eq(v => v.Id, id);
            return await _vuelos.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Vuelo> CreateAsync(Vuelo vuelo)
        {
            await _vuelos.InsertOneAsync(vuelo);
            return vuelo;
        }

        public async Task UpdateAsync(string id, Vuelo vuelo)
        {
            var filter = Builders<Vuelo>.Filter.Eq(v => v.Id, id);
            await _vuelos.ReplaceOneAsync(filter, vuelo);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<Vuelo>.Filter.Eq(v => v.Id, id);
            await _vuelos.DeleteOneAsync(filter);
        }
    }
}
