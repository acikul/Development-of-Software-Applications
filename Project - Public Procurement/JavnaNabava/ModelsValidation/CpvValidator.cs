using System;
using FluentValidation;
using JavnaNabava.Models;
using System.Text.RegularExpressions;

namespace JavnaNabava.ModelsValidation
{
    public class CpvValidator : AbstractValidator<Cpv>
    {
        public CpvValidator()
        {
            RuleFor(m => m.SifCpv)
        .NotEmpty().WithMessage("Potrebno je unijeti sifru CPV-a").Must(isValidCpv).WithMessage("Unesite valjani CPV");
            RuleFor(m => m.OpisCpv)
        .NotEmpty().WithMessage("Potrebno je unijeti opis CPV-a");

        }

        protected bool isValidCpv(string cpv)
        {
            Regex rgx = new Regex("^[0-9]{8}[-][0-9]$");
            return rgx.IsMatch(cpv);
        }

    }

}
