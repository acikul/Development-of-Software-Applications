$(document).on('click', '.deleterow', function () {
    event.preventDefault();
    var tr = $(this).parents("tr");
    tr.remove();
    clearOldMessage();
});

$(function () {
    $(".form-control").bind('keydown', function (event) {
        if (event.which === 13) {
            event.preventDefault();
        }
    });

    $("#komp-detalji, #vrsta-komp-naziv").bind('keydown', function (event) {
        if (event.which === 13) {
            event.preventDefault();
            dodajArtikl();
        }
    });


    $("#komp-dodaj").click(function () {
        event.preventDefault();
        dodajArtikl();
    });
});

function dodajArtikl() {
    var sifra = $("#komp-detalji").val();
    /*if (sifra != '') {
        if ($("[name='Kompetencije[" + sifra + "].SifKompetencije'").length > 0) {
            alert('Kompetencija vec postoji');
            return;
        }*/

        
        var template = $('#template').html();
        var detalji = $('#komp-detalji').val();
        var sifVrste = $('#vrsta-komp-sifra').val();
        var naziv = $("#vrsta-komp-naziv").val();

        template = template.replace(/--sifra--/g, 0)
            .replace(/--detalji--/g, detalji)
            .replace(/--nazivVrste--/g, naziv)
            .replace(/--sifVrste--/g, sifVrste)
        $(template).find('tr').insertBefore($("#table-stavke").find('tr').last());

        $("#komp-sifra").val('');
        $("#komp-detalji").val('');
        $("#vrsta-komp-sifra").val('');
        $("#vrsta-komp-naziv").val('');

        clearOldMessage();
    //}
}