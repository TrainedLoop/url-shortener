using System.Threading.Tasks;
using MongoDB.Driver;
using UrlShortener.Repository.Model;

namespace UrlShortener.Repository.Contract
{
    public interface IUrlShortedRepository
    {
        IMongoCollection<UrlShorted> Collection { get; }

        Task<UrlShorted> CountClick(string hash);
        Task CreateAsync(UrlShorted urlShorted);
        Task<UrlShorted> GetHashAsync(string hash);
    }
}