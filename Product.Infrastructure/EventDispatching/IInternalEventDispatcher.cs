using System.Collections.Generic;
using Product.Infrastructure.DataBase;

namespace Product.Infrastructure.EvnetDispatching
{
    public interface IInternalEventDispatcher<TDomainEvent>
    {
        void Dispatch(IReadOnlyList<TDomainEvent> domainEvents, DatabaseContext dbContext);
    }
}
