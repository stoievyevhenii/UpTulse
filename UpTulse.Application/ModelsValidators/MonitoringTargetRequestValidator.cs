using FluentValidation;

using UpTulse.Application.Models;

namespace UpTulse.Application.ModelsValidators
{
    public class MonitoringTargetRequestValidator : AbstractValidator<MonitoringTargetRequest>
    {
        public MonitoringTargetRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.");

            RuleFor(x => x.Address)
                .NotEmpty();
        }
    }
}