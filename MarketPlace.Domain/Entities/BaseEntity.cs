using System;
using System.Collections.Generic;
using System.Linq;
using MarketPlace.Framework;

namespace MarketPlace.Domain.Entities
{
    public abstract class BaseEntity<TId> : IInternalEventHandler where TId : Value<TId>
    {
        public TId Id { get; protected set; }
        public List<object> _events;
        private readonly Action<object> _applier;
        protected BaseEntity(Action<object> applier) => _applier = applier;
        protected BaseEntity() => _events = new List<object>();
        protected void Raise(object @event) => _events.Add(@event);
        public IEnumerable<object> GetChanges() => _events.AsEnumerable();
        public void ClearChanges() => _events.Clear();
        protected void Apply(object @event)
        {
            When(@event);
            _applier(@event);
        }
        protected abstract void When(object @event);
        void IInternalEventHandler.Handle(object @event) => When(@event);
        protected abstract void EnsureValidState();
    }
}
