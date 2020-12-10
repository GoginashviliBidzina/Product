using System.Collections.Generic;

namespace Template.Shared
{
    public interface IHasDomainEvents
    {
        IReadOnlyList<DomainEvent> UncommittedChanges();

        void MarkChangesAsCommitted();

        void Raise(DomainEvent @event);
    }
}