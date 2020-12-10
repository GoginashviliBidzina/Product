using System;
using Product.Infrastructure.DataBase;
using Product.Messaging.Infrastructure;
using Product.Domain.ProductAggregate.Repository;
using Product.Domain.CategoryAggregate.Repository;

namespace Product.Application.Infrastructure
{
    public abstract class ApplicationBase
    {
        protected DatabaseContext _db;
        protected UnitOfWork _unitOfWork;
        protected MessageSender _messageSender;
        protected IServiceProvider _serviceProvider;
        protected IProductRepository _productRepository;
        protected ICategoryRepository _categoryRepository;

        public void Resolve(DatabaseContext db,
                            UnitOfWork unitOfWork,
                            MessageSender messageSender,
                            IServiceProvider serviceProvider)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _messageSender = messageSender;
            _productRepository = GetService<IProductRepository>();
            _categoryRepository = GetService<ICategoryRepository>();
        }

        public T GetService<T>() => (T)_serviceProvider.GetService(typeof(T));
    }
}
