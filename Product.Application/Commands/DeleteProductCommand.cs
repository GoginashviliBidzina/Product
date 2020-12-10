using FluentValidation;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using Product.Application.Infrastructure;
using System;

namespace Product.Application.Commands
{
    [Validator(typeof(DeleteProductCommandValidator))]
    public class DeleteProductCommand : Command
    {
        public int Id { get; set; }

        public async override Task<CommandExecutionResult> ExecuteAsync()
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(Id);

                if (product == null)
                    return await FailAsync(ErrorCode.NotFound);

                product.RaiseDeleteEvent();

                _productRepository.Delete(product);
                await _unitOfWork.SaveAsync();

                return await OkAsync(DomainOperationResult.CreateEmpty());
            }
            catch (Exception)
            {
                return await FailAsync(ErrorCode.Exception);
            }
        }
    }

    internal class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(product => product.Id).NotEmpty();
        }
    }
}
