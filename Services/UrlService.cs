using MongoDB.Driver;
using EncurtadorUrl.Models;

namespace EncurtadorUrl.Services
{
    public class UrlService
    {
        private readonly IMongoCollection<ShortUrl> _collection;

        public UrlService(IConfiguration config)
        {
            var settings = config.GetSection("MongoSettings").Get<MongoSettings>();  
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<ShortUrl>(settings.CollectionName);
        }

        public async Task<ShortUrl> CriarEncurtamento(string urlOriginal)
        {
            var urlJaExiste = await _collection.Find(x => x.UrlOriginal == urlOriginal).FirstOrDefaultAsync();
            if (urlJaExiste != null) return urlJaExiste;

            string codigo;
            ShortUrl codigoExistente;

            do
            {
                codigo = Guid.NewGuid().ToString().Substring(0, 6);
                codigoExistente = await _collection.Find(x => x.Codigo == codigo).FirstOrDefaultAsync();
            } while (codigoExistente != null);

            var doc = new ShortUrl
            {
                Codigo = codigo,
                UrlOriginal = urlOriginal
            };

            await _collection.InsertOneAsync(doc);
            return doc;
        }

        public async Task<ShortUrl> CriarEncurtamentoComLinkPersonalizado(string urlOriginal, string linkPersonalizado)
        {

            if (string.IsNullOrWhiteSpace(linkPersonalizado) || linkPersonalizado.Length < 6)
                throw new ArgumentException("O link personalizado deve ter pelo menos 6 caracteres.");

            var linkJaExiste = await _collection.Find(x => x.Codigo == linkPersonalizado).FirstOrDefaultAsync();
            if (linkJaExiste != null)
            {
                throw new ArgumentException("O link personalizado já está em uso.");
            }

            var doc = new ShortUrl
            {
                Codigo = linkPersonalizado,
                UrlOriginal = urlOriginal
            };

            await _collection.InsertOneAsync(doc);
            return doc;
        }

        public async Task<ShortUrl> BuscarPorCodigo(string codigo)
        {
            return await _collection.Find(x => x.Codigo == codigo).FirstOrDefaultAsync();
        }
    }
}