using System;
using System.Linq;
using Template.Shared;
using System.Threading.Tasks;
using System.Collections.Generic;
using Product.Messaging.Infrastructure;
using Product.Infrastructure.EvnetDispatching;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Product.Infrastructure.DataBase
{
    public class UnitOfWork
    {
        private readonly DatabaseContext _db;
        private readonly IServiceProvider _serviceProvider;
        private readonly InternalDomainEventDispatcher _internalDomainEventDispatcher;

        public UnitOfWork(DatabaseContext db,
                          IServiceProvider serviceProvider,
                          InternalDomainEventDispatcher internalDomainEventDispatcher)
        {
            _db = db;
            _serviceProvider = serviceProvider;
            _internalDomainEventDispatcher = internalDomainEventDispatcher;
        }

        public async Task SaveAsync()
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            var modifiedEntries = _db.ChangeTracker.Entries<IHasDomainEvents>().ToList();
            await _db.SaveChangesAsync();

            ProcessUncommitedChanges(modifiedEntries);

            await _db.SaveChangesAsync();

            await transaction.CommitAsync();
        }

        private void ProcessUncommitedChanges(List<EntityEntry<IHasDomainEvents>> modifiedEntries)
        {
            foreach (var entry in modifiedEntries)
            {
                var events = entry.Entity.UncommittedChanges();
                if (!events.Any()) continue;
                _internalDomainEventDispatcher.Dispatch(events, _db);

                var sender = GetService<MessageSender>();
                sender.SendMessageAsync(entry.Entity as AggregateRoot);

                entry.Entity.MarkChangesAsCommitted();
            }
        }

        public T GetService<T>() => (T)_serviceProvider.GetService(typeof(T));
    }
}
