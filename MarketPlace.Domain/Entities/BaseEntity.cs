using System.Collections.Generic;
using System.Linq;


namespace MarketPlace.Domain.Entities
{
    public abstract class BaseEntity
    {
        private readonly List<object> _events;
        protected BaseEntity() => _events = new List<object>();
        protected void Raise(object @event) => _events.Add(@event);
        public IEnumerable<object> GetChanges() => _events.AsEnumerable();
        public void ClearChanges() => _events.Clear();
        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            _events.Add(@event);
        }
        protected abstract void When(object @event);
        protected abstract void EnsureValidState();
    }
}
