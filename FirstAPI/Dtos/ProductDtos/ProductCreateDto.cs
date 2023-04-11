using FluentValidation;

namespace FirstAPI.Dtos.ProductDtos
{
    public class ProductCreateDto
    {
        public string? Name { get; set; }
        public double Price { get; set; }
        public IFormFile? Photo { get; set; }
        public double? SalePrice { get; set; }
        public bool IsDeleted { get; set; }


    }

    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(p => p.Name).
                MinimumLength(5).WithMessage("Minimum uzunluq 5 simvol")
                .MaximumLength(30).WithMessage("uzunluq 50 simvoldan cox ola bilmez!")
                .NotNull().WithMessage("bosh olmaz");
            RuleFor(p => p.SalePrice)
                .NotNull().WithMessage("bosh olmaz")
                .GreaterThanOrEqualTo(0).WithMessage("0 dan boyuk olmalidi");
            RuleFor(p => p.SalePrice)
              .NotNull().WithMessage("bosh olmaz")
              .GreaterThanOrEqualTo(0).WithMessage("0 dan boyuk olmalidi");
            RuleFor(p => p).Custom((p, context) =>
            {
                if (p.SalePrice < p.Price)
                {
                    context.AddFailure("price", "Price Saledan boyuk olmalidi!");
                }
            });
        }
    }
}
