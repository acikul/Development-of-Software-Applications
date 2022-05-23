using FluentValidation;
using JavnaNabava.Models;

namespace MVC.ModelsValidation
{
    public class KompetencijaValidator : AbstractValidator<Kompetencija>
    {
        public KompetencijaValidator()
        {
            RuleFor(d => d.DetaljiKompetencije)
              .NotEmpty().WithMessage("Naziv  kompetencije je obvezno polje");
            RuleFor(d => d.Oibstrucnjaka)
              .NotEmpty().WithMessage("OIB stručnjaka je obvezno polje");
            RuleFor(d => d.DetaljiKompetencije)
              .NotEmpty().WithMessage("Detalji kompetencije je obvezno polje");
            RuleFor(d => d.SifVrsteKompetencije)
              .NotEmpty().WithMessage("Vrsta kompetencije je obvezno polje");
        }
    }
}
