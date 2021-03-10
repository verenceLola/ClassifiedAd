using System.Threading.Tasks;

namespace MarketPlace.Domain.Services.Interfaces
{
    public interface ICustomerCreditService
    {
        Task<bool> EnsureEnoughCredit(int customerId, decimal amount);
    }
}
