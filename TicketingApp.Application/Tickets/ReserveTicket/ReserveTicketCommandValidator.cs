using FluentValidation;

namespace TicketingApp.Application.Tickets.ReserveTicket;

// Sadece bu komuta özel "şekilsel" doğrulama kuralları
public class ReserveTicketCommandValidator : AbstractValidator<ReserveTicketCommand>
{
    public ReserveTicketCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty()
            .WithMessage("Bilet Id boş olamaz.");
    }
}