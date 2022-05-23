using FluentValidation;
using JavnaNabava.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ModelsValidation
{
    public class StavkaValidator : AbstractValidator<Stavka>
    {

        public StavkaValidator()
        {
            RuleFor(m => m.NazivStavke)
        .NotEmpty().WithMessage("Potrebno je unijeti naziv stavke");
            RuleFor(m => m.JedCijena)
        .NotEmpty().WithMessage("Potrebno je unijeti jedinicnu cijenu stavke");
            RuleFor(m => m.StopaPdv)
        .NotEmpty().WithMessage("Potrebno je unijeti stopu PDV-a stavke");
            RuleFor(m => m.Kolicina)
        .NotEmpty().WithMessage("Potrebno je unijeti kolicinu stavke");
            RuleFor(m => m.UkCijena)
        .NotEmpty().WithMessage("Potrebno je unijeti ukupnu cijenu stavke");
            RuleFor(m => m.NapomenaPonuditelja)
        .NotEmpty().WithMessage("Potrebno je unijeti napomenu ponuditelja");
            RuleFor(m => m.JedCijenaSaPdv)
        .NotEmpty().WithMessage("Potrebno je unijeti jedinicnu cijenu s PDV-om stavke");
            RuleFor(m => m.UkCijenaSaPdv)
        .NotEmpty().WithMessage("Potrebno je unijeti ukupnu cijenu s PDV-om stavke");
            RuleFor(m => m.IznosPdv)
        .NotEmpty().WithMessage("Potrebno je unijeti iznos PDV-a");
            RuleFor(m => m.NapomenaNarucitelja)
        .NotEmpty().WithMessage("Potrebno je unijeti napomenu narucitelja");
            RuleFor(m => m.SifTroskovnik)
        .NotEmpty().WithMessage("Potrebno je odabrati sifru troskovnika");
            RuleFor(m => m.SifVrstaStavke)
        .NotEmpty().WithMessage("Potrebno je odabrati sifru vrste stavke");




        }

    }
}
