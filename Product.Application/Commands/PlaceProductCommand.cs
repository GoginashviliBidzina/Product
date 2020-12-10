using System;
using System.Linq;
using FluentValidation;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentValidation.Attributes;
using Product.Application.Infrastructure;
using Product.Domain.ProductAggregate.ValueObjects;

namespace Product.Application.Commands
{
    [Validator(typeof(PlaceProductCommandValidator))]
    public class PlaceProductCommand : Command
    {
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
                var photo = new Photo(PhotoUrl,
                                      PhotoWidth,
                                      PhotoHeight);

                var product = new Domain.ProductAggregate.Product(Name,
                                                                  Amount,
                                                                  Code,
                                                                  CategoryIds?.Any() == true ? string.Join(',', CategoryIds.Distinct()) : string.Empty,
                                                                  photo);

                await SaveAsync(product, _productRepository);

                return await OkAsync(DomainOperationResult.Create(product.Id));
            }
            catch (Exception)
            {
                return await FailAsync(ErrorCode.Exception);
            }
        }
    }

    internal class PlaceProductCommandValidator : AbstractValidator<PlaceProductCommand>
    {
        public PlaceProductCommandValidator()
        {
            RuleFor(product => product.Name).Custom((name, context) =>
            {
                if (name.Any(char.IsDigit))
                    context.AddFailure("Placing product failed: name shouldn't contain digits...");
            });
            RuleFor(product => product.Amount).GreaterThan(1).WithMessage("Placing product failed: product amount should be greater than one...");
        }
    }
}
