using System.Threading.Tasks;
using MarketPlace.Domain.Entities;
using MarketPlace.Domain.ValueObjects;

namespace MarketPlace.Domain.Interfaces
{
    public interface IUserProfileRepository
    {
        Task Add(UserProfile entity);
        Task<bool> Exists(UserId id);
        Task<UserProfile> Load(UserId id);
    }
}
