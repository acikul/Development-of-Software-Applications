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

    $("#stavka-klasa, #stavka-urudzbeniBroj").bind('keydown', function (event) {
        if (event.which === 13) {
            event.preventDefault();
            dodajArtikl();
        }
    });


    $("#stavka-dodaj").click(function (event) {
        event.preventDefault();
        dodajArtikl();
    });
})

function dodajArtikl() {
    var sifra = $("#stavka-EvidencijskiBroj").val();
    if (sifra != '') {

        var template = $('#template').html();
          



        var EvidencijskiBroj = $("#stavka-EvidencijskiBroj").val();
        var PredmetNabave = $("#stavka-PredmetNabave").val();
        var VrijediOd = $("#stavka-VrijediOd").val();
        var VrijediDo = $("#stavka-VrijediDo").val();
        var Napomena = $('#stavka-Napomena').val();
        var VrstaPostupka = $("#stavka-VrstaPostupka").val();
        var VrijednostNabave = $("#stavka-VrijednostNabave").val();;
        var Status = $("#stavka-Status").val();;

        console.log("EvidencijskiBroj")
        console.log(EvidencijskiBroj)
        

        //Alternativa ako su hr postavke sa zarezom //http://haacked.com/archive/2011/03/19/fixing-binding-to-decimals.aspx/
        //ili ovo http://intellitect.com/custom-model-binding-in-asp-net-core-1-0/

        template = template
            .replace(/--ime--/g, EvidencijskiBroj)
            .replace(/--EvidencijskiBroj--/g, EvidencijskiBroj)
            .replace(/--PredmetNabave--/g, PredmetNabave)
            .replace(/--VrijediOd--/g, VrijediOd)
            .replace(/--VrijediDo--/g, VrijediDo)
            .replace(/--Napomena--/g, Napomena)
            .replace(/--VrstaPostupka--/g, VrstaPostupka)
            .replace(/--VrijednostNabave--/g, VrijednostNabave)
            .replace(/--Status--/g, Status);
        $(template).find('tr').insertBefore($("#table-stavke").find('tr').last());


        $("#stavka-EvidencijskiBroj").val('');
        $("#stavka-PredmetNabave").val('');
        $("#stavka-VrijediOd").val('');
        $("#stavka-VrijediDo").val('');
        $("#stavka-Napomena").val('');
        $("#stavka-VrijednostNabave").val('');
        $("#stavka-Status").val('');

        clearOldMessage();
    }
}