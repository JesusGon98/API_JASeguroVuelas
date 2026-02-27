using API_JASeguroVuelas.Models;
using MongoDB.Driver;

namespace API_JASeguroVuelas.Services
{
    public class UsuarioService
    {
        private readonly IMongoCollection<Usuario> _usuarios;

        public UsuarioService(IMongoDatabase database)
        {
            _usuarios = database.GetCollection<Usuario>("Usuarios");
        }

        public async Task<List<Usuario>> GetAsync()
        {
            return await _usuarios.Find(_ => true).ToListAsync();
        }

        public async Task<Usuario?> GetByIdAsync(string id)
        {
            var filter = Builders<Usuario>.Filter.Eq(u => u.Id, id);
            return await _usuarios.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            var filter = Builders<Usuario>.Filter.Eq(u => u.Email, email.ToLowerInvariant());
            return await _usuarios.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            usuario.Email = usuario.Email.ToLowerInvariant();
            await _usuarios.InsertOneAsync(usuario);
            return usuario;
        }

        public async Task UpdateAsync(string id, Usuario usuario)
        {
            var filter = Builders<Usuario>.Filter.Eq(u => u.Id, id);
            usuario.Email = usuario.Email.ToLowerInvariant();
            await _usuarios.ReplaceOneAsync(filter, usuario);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<Usuario>.Filter.Eq(u => u.Id, id);
            await _usuarios.DeleteOneAsync(filter);
        }
    }
}
