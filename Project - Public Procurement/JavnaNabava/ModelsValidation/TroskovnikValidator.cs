using FluentValidation;
using JavnaNabava.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ModelsValidation
{
    public class TroskovnikValidator : AbstractValidator<Troskovnik>
    {

        public TroskovnikValidator()
        {

            RuleFor(m => m.IspravnoPopunjen)
            .NotNull().WithMessage("Potrebno je unijeti oznaku ispravnosti");
            RuleFor(m => m.SifValuta)
            .NotEmpty().WithMessage("Potrebno je odabrati sifru valute");

        }

    }
}
