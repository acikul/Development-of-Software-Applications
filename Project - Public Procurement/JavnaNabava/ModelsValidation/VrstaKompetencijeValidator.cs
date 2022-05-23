using FluentValidation;
using JavnaNabava.Models;

namespace MVC.ModelsValidation
{
    public class VrstaKompetencijeValidator : AbstractValidator<VrstaKompetencije>
    {
        public VrstaKompetencijeValidator()
        {
            RuleFor(d => d.NazivVrsteKompetencije)
              .NotEmpty().WithMessage("Naziv vrste kompetencije je obvezno polje");

        }
    }
}
