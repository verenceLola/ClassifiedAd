using System.Threading.Tasks;
using System;
using MarketPlace.Framework;


namespace MarketPlace.Services.ApplicationServices
{
    public static class ApplicationServiceextensions
    {
        public static async Task HandleUpdate<T, TId>(
            this Services.ApplicationServices.Interfaces.IApplicationService service,
            IAggregateStore store, TId aggregateId, Action<T> operation) where T : AggregateRoot<TId>
        {
            var aggregate = await store.Load<T, TId>(aggregateId);
            if (aggregate == null)
            {
                throw new InvalidOperationException(
                    $"Entity with id {aggregateId.ToString()} cannot be found"
                );
            }
            operation(aggregate);
            await store.Save<T, TId>(aggregate);
        }
    }
}
