using System.Linq;
using FluentValidation;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentValidation.Attributes;
using Product.Domain.CategoryAggregate;
using Product.Application.Infrastructure;
using Product.Domain.ProductAggregate.ValueObjects;

namespace Product.Application.Commands
{
    [Validator(typeof(UpdateProductCommandValidator))]
    public class UpdateProductCommand : Command
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }

        public string Code { get; set; }

        public int PhotoWidth { get; set; }

        public int PhotoHeight { get; set; }

        public string PhotoUrl { get; set; }

        public List<int> CategoryIds { get; set; }

        public async override Task<CommandExecutionResult> ExecuteAsync()
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(Id);

                var categoryIds = _db.Set<Category>()
                                     .Where(category => CategoryIds.Contains(category.Id))
                                     .Select(category => category.Id)
                                     .Distinct()
                                     .ToList();

                var photo = new Photo(PhotoUrl,
                                          PhotoWidth,
                                          PhotoHeight);

                product.ChangeDetails(Name,
                                      Amount,
                                      Code,
                                      categoryIds?.Any() == true ? string.Join(',', categoryIds) : string.Empty,
                                      photo);

                await SaveAsync(product, _productRepository);

                return await OkAsync(DomainOperationResult.Create(product.Id));
            }
            catch (System.Exception)
            {
                return await FailAsync(ErrorCode.Exception);
            }
        }
    }

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(product => product.Name).Custom((name, context) =>
            {
                if (name.Any(char.IsDigit))
                    context.AddFailure("Updating product failed: name shouldn't contain digits...");
            });
            RuleFor(product => product.Amount).GreaterThan(1).WithMessage("Updating product failed: product amount should be greater than one...");
            RuleFor(product => product.Id).GreaterThan(0).WithMessage("Updating product failed: Id should be greater than zero...");
        }
    }
}
