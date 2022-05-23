using System;
using FluentValidation;
using JavnaNabava.Models;

namespace JavnaNabava.ModelsValidation
{
    public class ValutumValidator : AbstractValidator<Valutum>
    {
        public ValutumValidator()
        {
            RuleFor(m => m.ImeValute)
        .NotEmpty().WithMessage("Potrebno je unijeti ime valute");
            RuleFor(m => m.OznValute)
        .NotEmpty().WithMessage("Potrebno je odabrati oznaku valute");
        
        
        }
    }
}
