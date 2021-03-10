using System.Threading.Tasks;
using MarketPlace.Domain.Entities;
using MarketPlace.Domain.ValueObjects;


namespace MarketPlace.Domain.Interfaces
{
    public interface IClassifiedAdRepository
    {
        Task<ClassfiedAd> Load(ClassfiedAdId id);
        Task Add(ClassfiedAd entity);
        Task<bool> Exists(ClassfiedAdId id);
    }
}
