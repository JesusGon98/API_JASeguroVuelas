using API_JASeguroVuelas.Models;
using MongoDB.Driver;

namespace API_JASeguroVuelas.Services
{
    public class ReservacionService
    {
        private readonly IMongoCollection<Reservacion> _reservaciones;

        public ReservacionService(IMongoDatabase database)
        {
            _reservaciones = database.GetCollection<Reservacion>("Reservaciones");
        }

        public async Task<List<Reservacion>> GetAsync()
        {
            return await _reservaciones.Find(_ => true).ToListAsync();
        }

        public async Task<Reservacion?> GetByIdAsync(string id)
        {
            var filter = Builders<Reservacion>.Filter.Eq(r => r.Id, id);
            return await _reservaciones.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Reservacion> CreateAsync(Reservacion reservacion)
        {
            await _reservaciones.InsertOneAsync(reservacion);
            return reservacion;
        }

        public async Task UpdateAsync(string id, Reservacion reservacion)
        {
            var filter = Builders<Reservacion>.Filter.Eq(r => r.Id, id);
            await _reservaciones.ReplaceOneAsync(filter, reservacion);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<Reservacion>.Filter.Eq(r => r.Id, id);
            await _reservaciones.DeleteOneAsync(filter);
        }
    }
}
