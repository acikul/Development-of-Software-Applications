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
    })
});

$("#kontakt-tekstKontakta").bind('keydown', function () {
    if (event.which === 13) {
        event.preventDefault();
        dodajKontakt();
    }
});


$("#kontakt-dodaj").click(function () {
    event.preventDefault()
    dodajKontakt();
});

function dodajKontakt() {
    var sifVrsteKontakta = document.getElementById("kontakt-sifVrsteKontakta").value
    if (sifVrsteKontakta != undefined) {
        if ($("[name='kontakti[" + sifVrsteKontakta + "].SifVrsteKontakta'").length > 0) {
            alert('Kontakt je već u dokumentu');
            return;
        }

        var tekstKontakta = $("#kontakt-tekstKontakta").val();
        var nazivVrsteKontakta = sifVrsteKontakta === "1" ? "telefon" : "email"

        var template = $('#template').html();

        //Alternativa ako su hr postavke sa zarezom //http://haacked.com/archive/2011/03/19/fixing-binding-to-decimals.aspx/
        //ili ovo http://intellitect.com/custom-model-binding-in-asp-net-core-1-0/

        template = template.replace(/--sifra--/g, sifVrsteKontakta)
            .replace(/--tekstKontakta--/g, tekstKontakta)
            .replace(/--nazivVrsteKontakta--/g, nazivVrsteKontakta)
        $(template).find('tr').insertBefore($("#table-stavke").find('tr').last());

        $("#kontakt-tekstKontakta").val('');

        clearOldMessage();
    }
}