function GetListado() {
    

    $.post('/Home/GetListado', null, function (data) {
        console.log(data);
        dtListado._fnClearTable();

        if (data.length == 0) {
            
            dtListado._fnReDraw();
            return;
        }
        
        $(data).each(function (i, item) {
            var addData = [];
            addData.push(item.ID);
            addData.push('<div  style="text-align:center">' + item.NOMBRECLIENTE + '</div>');
            addData.push('<div  style="text-align:center">' + item.TIPOCLIENTE + '</div>');
            addData.push('<div  style="text-align:center">' + item.NOMBREDISFRAZ + '</div>');
            addData.push('<div  style="text-align:center">' + item.TIPOPAGO + '</div>');
            addData.push('<div  style="text-align:center">' + item.TIPODISFRAZ + '</div>');
            addData.push('<div  style="text-align:center">' + item.CANTIDAD + '</div>');
            addData.push('<div  style="text-align:center">' + ConvertDateJSfromDateJSON(item.FECHAARRIENDO) + '</div>');
            addData.push('<div  style="text-align:center">' + ConvertDateJSfromDateJSON(item.FECHAFINALIZACION) + '</div>');

            if (item.ESTADO == true) {
                addData.push('<div  style="text-align:center"><button type="button" class="btn btn-warning btn-sm" data-toggle="modal" data-target="#mFinalizarServicio" id="' + item.ID + '"><span class="fas fa-redo" aria-hidden="true"></span></button></div>');
            } else { addData.push(''); }

            dtListado.fnAddData(addData);
        });

    }).fail(function () {
        dtListado._fnClearTable(); 
    });
}

function GetListadoDisfraz() {

    $.post('/Home/GetListadoDisfraz', null, function (data) {

        $('#selectedDisfraz').empty();
        $('#selectedDisfraz').append('<option value="0">---</option>');

        $(data).each(function (i, item) {
            $('#selectedDisfraz').append('<option value="' + item.id + '">' + item.nombre + '</option>');
        });

    }).fail(function (a, b, c) {
        console.log("Se ha presentado un error");
        $('#selectedDisfraz').empty();
        $('#selectedDisfraz').append('<option value="0">---</option>');

    });
}

function GetListadoTipoDisfraz() {

    $.post('/Home/GetListadoTipoDisfraz', null, function (data) {

        $('#selectedTipoDisfraz').empty();
        $('#selectedTipoDisfraz').append('<option value="0">---</option>');

        $(data).each(function (i, item) {
            $('#selectedTipoDisfraz').append('<option value="' + item.id + '">' + item.nombre + '</option>');
        });

    }).fail(function (a, b, c) {
        console.log("Se ha presentado un error");
        $('#selectedTipoDisfraz').empty();
        $('#selectedTipoDisfraz').append('<option value="0">---</option>');

    });
}

function GetListadoTipoPago() {

    $.post('/Home/GetListadoTipoPago', null, function (data) {

        $('#selectedTipoPago').empty();
        $('#selectedTipoPago').append('<option value="0">---</option>');

        $(data).each(function (i, item) {
            $('#selectedTipoPago').append('<option value="' + item.id + '">' + item.nombre + '</option>');
        });

    }).fail(function (a, b, c) {
        console.log("Se ha presentado un error");
        $('#selectedTipoPago').empty();
        $('#selectedTipoPago').append('<option value="0">---</option>');

    });
}

$('#btnModalAdicionar').click(function () {
    $('#mAdicionar').modal('show');
});

$('#btnAdicionar').click(function () {

    if ($("#rut").val() == "" || $("#nombres").val() == "" || $("#apellidos").val() == "" || $("#telefono").val() == "" || $("#diasArriendo").val() == "" || $("#cantidad").val() == "") {
        $('#lblTituloMensaje').text("Alerta");
        $('#lblMensaje').text("Se deben llenar todos los campos.");
        $('#mMensaje').modal('show');
        return;
    }

    if ($("#diasArriendo").val() > 60 ) {
        $('#lblTituloMensaje').text("Alerta");
        $('#lblMensaje').text("la cantidad de días de arriendo no puede ser mayor a 60 dias.");
        $('#mMensaje').modal('show');
        return;
    }

    var fechaArrend = $('input[name="fechaArriendo"]').data('daterangepicker').startDate.format('YYYY/MM/DD');

    if (moment(fechaArrend).isValid()) {
        if (moment().diff(fechaArrend, 'days') > 0) {

            var params = {
                rut: $('#rut').val(),
                nombres: $("#nombres").val(),
                apellidos: $('#apellidos').val(),
                telefono: $('#telefono').val(),
                disfrazId: $('#selectedDisfraz').val(),
                fechaArriendo: fechaArrend,
                diasArriendo: $('#diasArriendo').val(),
                tipoPagoId: $('#selectedTipoPago').val(),
                observacion: $('#observacion').val(),
                cantidad: $("#cantidad").val()
            };

            console.log(params);

            $.post('/Home/CreateArriendo', params, function (data) {

                if (!data.Status) {
                    $('#lblTituloMensaje').text("Alerta");
                    $('#lblMensaje').text(data.Message);
                    $('#mMensaje').modal('show');
                }

                else {
                    $('#mAdicionar').modal('hide');

                    $("#rut").val('');
                    $("#nombres").val('');
                    $("#apellidos").val('');
                    $("#telefono").val('');
                    $("#selectedDisfraz").val(0);
                    $('#diasArriendo').val('');
                    $('#selectedTipoPago').val(0);

                    GetListado();

                    $('#lblTituloMensaje').text("Alerta");
                    $('#lblMensaje').text(data.Message);
                    $('#mMensaje').modal('show');
                }

            }).fail(function () {
                $('#lblTituloMensaje').text("Alerta");
                $('#lblMensaje').text("Se ha presentado un error inesperado. Consulte con el administrador del sistema.");
                $('#mMensaje').modal('show');
            });
        }
        else {
            $('#lblTituloMensaje').text("Alerta");
            $('#lblMensaje').text("Ingrese una fecha de arrendamiento valida, menor a la fecha actual.");
            $('#mMensaje').modal('show');
        }
    }
    else {
        $('#lblTituloMensaje').text("Alerta");
        $('#lblMensaje').text("Ingrese una fecha de arrendamiento valida.");
        $('#mMensaje').modal('show');
    }
});

var servicioId;
$('#mFinalizarServicio').on('show.bs.modal', function (e) {
    var $modal = $(e.relatedTarget)
    id = e.relatedTarget.id;
    servicioId = id;
});

function FinalizarArriendo() {

    var params = {
        ServicioId: servicioId
    };

    PostAjax('/CentroDistribucion/FinalizarArriendo', params, function (data) {

        if (!data.Status) {
            $('#lblTituloMensaje').text("Alerta");
            $('#lblMensaje').text(data.Message);
            $('#mMensaje').modal('show');
        }

        else {
            $('#mFinalizarServicio').modal('hide');
            GetListado();

            $('#lblTituloMensaje').text("Alerta");
            $('#lblMensaje').text(data.Message);
            $('#mMensaje').modal('show');
        }

    }).fail(function () {
        $('#lblTituloMensaje').text("Alerta");
        $('#lblMensaje').text("Se ha presentado un error inesperado. Consulte con el administrador del sistema.");
        $('#mMensaje').modal('show');
    });
}