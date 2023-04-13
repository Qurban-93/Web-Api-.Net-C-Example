using FluentValidation;

namespace FirstAPI.Dtos.User
{
    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; } 
    }
    public class LoginDtoValidator : AbstractValidator<LoginDto> 
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Bosh qoymaq olmaz")
                .MaximumLength(20).WithMessage("uzunluq 20 den cox olmalid deil").MinimumLength(5);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Bosh qoymaq olmaz")
                .MaximumLength(20).WithMessage("uzunluq 20 den cox olmalid deil").MinimumLength(8);
        }
    }
}
