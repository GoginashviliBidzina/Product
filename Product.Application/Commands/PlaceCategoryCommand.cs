using System;
using FluentValidation;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using Product.Domain.CategoryAggregate;
using Product.Application.Infrastructure;

namespace Product.Application.Commands
{
    [Validator(typeof(PlaceCategoryCommandValidator))]
    public class PlaceCategoryCommand : Command
    {
        public string Name { get; set; }

        public async override Task<CommandExecutionResult> ExecuteAsync()
        {
            try
            {
                var category = new Category(Name);

                await SaveAsync(category, _categoryRepository);

                return await OkAsync(DomainOperationResult.Create(category.Id));
            }
            catch (Exception)
            {
                return await FailAsync(ErrorCode.Exception);
            }
        }
    }

    internal class PlaceCategoryCommandValidator : AbstractValidator<PlaceCategoryCommand>
    {
        public PlaceCategoryCommandValidator()
        {
            RuleFor(category => category.Name).NotEmpty().NotNull().WithMessage("Placing category failed: please fill category name...");
        }
    }
}
