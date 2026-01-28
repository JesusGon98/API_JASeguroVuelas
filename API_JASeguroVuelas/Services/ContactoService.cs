using API_JASeguroVuelas.Models;
using MongoDB.Driver;

namespace API_JASeguroVuelas.Services
{
    public class ContactoService
    {
        private readonly IMongoCollection<Contacto> _contactos;

        public ContactoService(IMongoDatabase database)
        {
            // La colección se crea automáticamente si no existe
            _contactos = database.GetCollection<Contacto>("Contactos");
        }

        public async Task<List<Contacto>> GetAsync()
        {
            return await _contactos.Find(_ => true).ToListAsync();
        }

        public async Task<Contacto?> GetByIdAsync(string id)
        {
            var filter = Builders<Contacto>.Filter.Eq(c => c.Id, id);
            return await _contactos.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Contacto> CreateAsync(Contacto contacto)
        {
            await _contactos.InsertOneAsync(contacto);
            return contacto;
        }

        public async Task UpdateAsync(string id, Contacto contacto)
        {
            var filter = Builders<Contacto>.Filter.Eq(c => c.Id, id);
            await _contactos.ReplaceOneAsync(filter, contacto);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<Contacto>.Filter.Eq(c => c.Id, id);
            await _contactos.DeleteOneAsync(filter);
        }
    }
}
