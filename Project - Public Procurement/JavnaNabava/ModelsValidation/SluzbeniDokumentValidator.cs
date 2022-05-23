using System;
using FluentValidation;
using JavnaNabava.Models;

namespace JavnaNabava.ModelsValidation
{
    public class SluzbeniDokumentValidator : AbstractValidator<SluzbeniDokument>
    {
        public SluzbeniDokumentValidator()
        {
            RuleFor(m => m.ImeDokumenta)
        .NotEmpty().WithMessage("Potrebno je unijeti ime dokumenta");
            RuleFor(m => m.SifVrsteDokumenta)
        .NotEmpty().WithMessage("Potrebno je odabrati vrstu dokumenta");
            RuleFor(m => m.SifPrijave)
        .NotEmpty().WithMessage("Potrebno je odabrati prijavu na natječaj");
            RuleFor(m => m.UrudzbeniBroj)
        .NotEmpty().WithMessage("Potrebno je unjeti urudžbeni broj");
            RuleFor(m => m.KlasaDokumenta)
       .NotEmpty().WithMessage("Potrebno je unjeti klasu dokumenta");
           
        }
    }
}
