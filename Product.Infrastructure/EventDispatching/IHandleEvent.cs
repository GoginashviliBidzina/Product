using Product.Infrastructure.DataBase;
using Template.Shared;

namespace PersonBook.Infrastructure.EvnetDispatching
{
    public interface IHandleEvent<in TEvent> where TEvent : DomainEvent
    {
        void Handle(TEvent @event, DatabaseContext db);
    }
}
