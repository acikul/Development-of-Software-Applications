using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;

namespace JavnaNabava.ModelsValidation
{
    public class PrijavaNaNatjecajValidator : AbstractValidator<PrijavaNaNatjecajViewModel>
    {
        public PrijavaNaNatjecajValidator()
        {
            RuleFor(m => m.OIBKonzorcij)
        .NotEmpty().When(m => m.VrstaPonuditelja=="konzorcij").WithMessage("Potrebno je odabrati konzorcij");
            RuleFor(m => m.OIBPonuditelj)
        .NotEmpty().When(m => m.VrstaPonuditelja=="fizicka osoba").WithMessage("Potrebno je odabrati ponuditelja");
            RuleFor(m => m.SifNatjecaja)
        .NotEmpty().WithMessage("Potrebno je odabrati natječaj");
            RuleFor(m => m.SifStatus)
        .NotEmpty().WithMessage("Potrebno je odabrati status prijave");
            RuleFor(m => m.SifTroskovnika)
       .NotEmpty().WithMessage("Potrebno je odabrati troskovnik");
        }
    }
}
