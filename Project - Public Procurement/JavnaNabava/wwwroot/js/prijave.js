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

    $("#dokument-klasa, #dokument-urudzbeniBroj").bind('keydown', function (event) {
        if (event.which === 13) {
            event.preventDefault();
            dodajArtikl();
        }
    });


    $("#dokument-dodaj").click(function () {
        event.preventDefault();
        dodajArtikl();
    });
});

function dodajArtikl() {
    var sifra = $("#dokument-ime").val();
    if (sifra != '') {
        if ($("[name='Dokumenti[" + sifra + "].ImeDokumenta'").length > 0) {
            alert('Dokument je već u prijavi');
            return;
        }


        var urudzbeniBroj = $("#dokument-urudzbeniBroj").val();
        var klasa = $("#dokument-klasa").val();
        var sifraVrste = $("#dokument-sifraVrste").val();
        var urudzbeniBroj = $("#dokument-urudzbeniBroj").val();
        var template = $('#template').html();
        var vrsta = $("#dokument-vrsta").val();
        var tekst = $("#dokument-tekst").val();;
        

        //Alternativa ako su hr postavke sa zarezom //http://haacked.com/archive/2011/03/19/fixing-binding-to-decimals.aspx/
        //ili ovo http://intellitect.com/custom-model-binding-in-asp-net-core-1-0/

        template = template.replace(/--ime--/g, sifra)
            .replace(/--sifraVrste--/g, sifraVrste)
            .replace(/--vrsta--/g, vrsta)
            .replace(/--urudzbeniBroj--/g, urudzbeniBroj)
            .replace(/--klasa--/g, klasa)
            .replace(/--tekst--/g, tekst);
        $(template).find('tr').insertBefore($("#table-dokumenti").find('tr').last());

        $("#dokument-urudzbeniBroj").val('');
        $("#dokument-klasa").val('');
        $("#dokument-sifraVrste").val('');
        $("#dokument-vrsta").val('');
        $("#dokument-ime").val('');
        $("#dokument-tekst").val('');

        clearOldMessage();
    }
}