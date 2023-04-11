using FluentValidation;

namespace FirstAPI.Dtos.CategoryDtos
{
    public class CategoryUpdateDto
    {
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public IFormFile? Photo { get; set; }
    }

    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto> 
    {
        public CategoryUpdateDtoValidator()
        {
            RuleFor(x => x.CategoryName)
               .NotEmpty().WithMessage("Bosh qoymaq olmaz")
               .MaximumLength(20).WithMessage("uzunluq 20 den cox olmalid deil");

            RuleFor(x => x.CategoryDescription)
                .MinimumLength(10).WithMessage("Minimum uzunluq 10 simvol !")
                .NotEmpty().WithMessage("Bosh qoyma");
        }
    
    } 
}
