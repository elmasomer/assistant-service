using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class ChatRequestValidator : AbstractValidator<ChatRequestDto>
{
    public ChatRequestValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Mesaj alanı boş olamaz.")
            .MinimumLength(1)
            .WithMessage("Mesaj en az 1 karakter olmalıdır.")
            .MaximumLength(5000)
            .WithMessage("Mesaj en fazla 5000 karakter olmalıdır.");
    }
}