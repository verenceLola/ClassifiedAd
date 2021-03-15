using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Framework
{
    public abstract class AggregateRoot<TId> : IInternalEventHandler
    {
        public TId Id { get; protected set; }
        protected abstract void When(object @event);
        public int Version { get; private set; } = -1;
        private readonly List<object> _changes;
        protected AggregateRoot() => _changes = new List<object>();
        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            _changes.Add(@event);
        }
        public IEnumerable<object> GetChanges() => _changes.AsEnumerable();
        public void ClearChanges() => _changes.Clear();
        protected abstract void EnsureValidState();
        protected void ApplyToEntity(IInternalEventHandler entity, object @event) => entity?.Handle(@event);
        void IInternalEventHandler.Handle(object @event) => When(@event);
        public void Load(IEnumerable<object> history)
        {
            foreach (var e in history)
            {
                When(e);
                Version++;
            }
        }
    }
}
