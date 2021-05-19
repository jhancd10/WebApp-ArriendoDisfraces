///
function DownloadUtils(url) {
    
    var params = {
        rutaArchivo: url
    };
    $.post('/Monitoreo/Alertas/AbrirArchivo', params, function (data) {
        if (data.length == 0) {
            $('#lblTituloMensaje').text("Advertencia");
            $('#lblMensaje').text("Esta consulta no encontro ningun registro, por favor valide su consulta e intente nuevamente.");
            $('#mMensaje').modal('show');
            $('#dReporte').css('visibility', 'invisible');
            return;
        }
        DescargarArchivoUtils(data);
    }).fail(function () {
        console.log("Se ha presentado un error");
        $('#lblTituloMensaje').text("Alerta");
        $('#lblMensaje').text("Se ha presentado un problema al generar el reporte. Intente nuevamente, de lo contrario consulte con el administrador del sistema.");

    }).always(function () {
        
    });

};

function DescargarArchivo(result) {
    
    if (result[0].Estado == "True") {
        window.location = '/Monitoreo/Reglas/Download?fileGuid=' + result[1].FileGuid
            + '&filename=' + encodeURIComponent(result[1].FileName);

    }
    else {
        alert(result[0].Mensaje);
    }
    
}

function DescargarArchivoUtils(result)
{
    if (result[0].Estado) {
        window.location = '../../Utils/DownloadFile?fileGuid=' + result[1].FileGuid
            + '&filename=' + encodeURIComponent(result[1].FileName) + '&contentType=' + result[1].ContentType;
    }
    else {
        alert(result[0].Mensaje);
    }
}
function ValidarNivelDificultadClave(password) {
    var upperCase = new RegExp('[A-Z]');
    var lowerCase = new RegExp('[a-z]');
    var caracterEspe = new RegExp('[!#$@|/\?¡¿();,%^&*._=+-]');
    var numbers = new RegExp('[0-9]');


    var characters = (password.length >= 8) ? 1 : 0;
    var capitalletters = password.match(upperCase) ? 1 : 0;
    var loweletters = password.match(lowerCase) ? 1 : 0;
    var caracterLetters = password.match(caracterEspe) ? 1 : 0;
    var number = password.match(numbers) ? 1 : 0;

    var containWhiteSpaceSum = password.indexOf(' ') >= 0 ? 0 : 1;

    var total = characters + capitalletters + loweletters + caracterLetters + number + containWhiteSpaceSum;
    if (total < 6) {

        return false;
    }
    return true;
}
var formatCultureDate = 'es-CO';

function ConvertDateTimeJSfromDateJSON(dateString) {

    if (!dateString) {
        return "";
    }
    else {

        return ConvertDateJSfromDateJSON(dateString) + " " + ConvertTimeJSfromDateJSON(dateString);
    }
}
function ConvertTimeJSfromDateJSON(dateString) {

    if (!dateString) {
        return "";
    }
    else {
        var myDate = new Date(dateString.match(/\d+/)[0] * 1);
        const options = { hour12: false, hour: '2-digit', minute: '2-digit', second: '2-digit' };

        return myDate.toLocaleTimeString(formatCultureDate, options);
    }
}

function ConvertDateJSfromDateJSON(dateString) {

    if (!dateString) {
        return "";
    }
    else {
        var myDate = new Date(dateString.match(/\d+/)[0] * 1);
        const options = { year: 'numeric', month: '2-digit', day: '2-digit' };

        return myDate.toLocaleDateString(formatCultureDate, options);
    }
}

function ConvertDateJSfromDateJSONFormatYYYYMMDD(dateString) {
    if (!dateString) {
        return "";
    }
    else {
        var myDate = new Date(dateString.match(/\d+/)[0] * 1);
        const options = { year: 'numeric', month: '2-digit', day: '2-digit' };

        return myDate.toLocaleDateString('fr-CA', options);
    }
}
///Obtiee la fehca y hora de una fecha JSON
function GetStringFechaHora(dateString) {
    dateString = dateString.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var mes = currentTime.getMonth() + 1;
    var dia = currentTime.getDate();
    var year = currentTime.getFullYear();
    var hora = currentTime.toLocaleTimeString().toLowerCase();

    if (mes < 10) { mes = '0' + mes }
    if (dia < 10) { dia = '0' + dia }

    var date = dia + "/" + mes + "/" + year + " " + hora;
    return date;
}

function UtilOPT_Round(value, exp) {
    if (typeof exp === 'undefined' || +exp === 0)
        return Math.round(value);

    value = +value;
    exp = +exp;

    if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0))
        return NaN;

    // Shift
    value = value.toString().split('e');
    value = Math.round(+(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp)));

    // Shift back
    value = value.toString().split('e');
    return +(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp));
}

function cambiar_icono(idImg, nombreIcono) {
    $('#' + idImg).attr('src', '../../Images/Iconos/' + nombreIcono + '.png');
}

function FormatoNumero(num, fix) {

    if (num == null)
        return "0";

    var p = num.toFixed(fix).split(".");
    return p[0].split("").reduceRight(function (acc, num, i, orig) {
        if ("-" === num && 0 === i) {
            return num + acc;
        }
        var pos = orig.length - i - 1
        return num + (pos && !(pos % 3) ? "." : "") + acc;
    }, "") + (p[1] ? "," + p[1] : "");
}

/*function getSimboloMoneda() {
    return '$';
}*/

function formatCurrency(simboloMoneda,total) {
    //var simbolo = getSimboloMoneda();
    return SimboloFormatCurrency(simboloMoneda,total);
}

function SimboloFormatCurrency(simbolo, total)
{
    var neg = false;
    if (total < 0) {
        neg = true;
        total = Math.abs(total);
    }
    return (neg ? "-" + simbolo : simbolo) + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
}