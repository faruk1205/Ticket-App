using FluentValidation;

namespace TicketingApp.Application.Events.CreateEvent;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Etkinlik adı boş olamaz.")
            .MaximumLength(150).WithMessage("Etkinlik adı en fazla 150 karakter olabilir.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Etkinlik lokasyonu boş olamaz.")
            .MaximumLength(200).WithMessage("Etkinlik lokasyonu en fazla 200 karakter olabilir.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Başlangıç tarihi belirtilmelidir.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("Bitiş tarihi belirtilmelidir.")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("Bitiş tarihi, başlangıç tarihinden önce veya eşit olmalıdır.");
    }
}
