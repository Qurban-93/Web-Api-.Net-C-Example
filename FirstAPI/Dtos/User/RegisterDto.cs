using FirstAPI.Dtos.CategoryDtos;
using FluentValidation;

namespace FirstAPI.Dtos.User
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string Email { get; set; }

    }

    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Bosh qoymaq olmaz")
                .MaximumLength(20).WithMessage("uzunluq 20 den cox olmalid deil").MinimumLength(5);

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Bosh qoymaq olmaz")
                .MaximumLength(20).WithMessage("uzunluq 20 den cox olmalid deil").MinimumLength(5);

            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("Bosh qoymaq olmaz")
               .MaximumLength(20).WithMessage("uzunluq 20 den cox olmalid deil").EmailAddress();

            RuleFor(x => x.Password)
                .MinimumLength(8).WithMessage("Minimum uzunluq 8 simvol !")
                .NotEmpty().WithMessage("Bosh qoyma");

            RuleFor(x => x.RepeatPassword)
                .MinimumLength(8).WithMessage("Minimum uzunluq 8 simvol !")
                .NotEmpty().WithMessage("Bosh qoyma");

            RuleFor(x => x).Custom((x, context) =>
            {
                if(x.Password != x.RepeatPassword)
                {
                    context.AddFailure("RepeatPassword", "Password not same !");
                }
            });

        }
    }
}
