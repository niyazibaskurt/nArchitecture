using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Languages.Commands.DeleteLanguage
{
    public class DeleteLanguageCommandValidator : AbstractValidator<DeleteLanguageCommand>
    {
        public DeleteLanguageCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.Id).GreaterThan(0).WithMessage("ID must be greater than 0.");
        }

    }
}
