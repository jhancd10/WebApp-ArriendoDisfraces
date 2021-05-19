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
            addData.push('<div  style="text-align:center">' + item.NOMBREDISFRAZ + '</div>');
            addData.push('<div  style="text-align:center">' + item.TIPOPAGO + '</div>');
            addData.push('<div  style="text-align:center">' + item.TIPODISFRAZ + '</div>');
            addData.push('<div  style="text-align:center">' + ConvertDateJSfromDateJSON(item.FECHAARRIENDO) + '</div>');
            addData.push('<div  style="text-align:center">' + ConvertDateJSfromDateJSON(item.FECHAFINALIZACION) + '</div>');
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

    }).always(function () {
        
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

    }).always(function () {
        
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

    }).always(function () {
        
    });
}

$('#btnModalAdicionar').click(function () {
    $('#mAdicionar').modal('show');
});

$('#btnAdicionar').click(function () {

    if ($("#rut").val() == "" || $("#nombres").val() == "" || $("#apellidos").val() == "" || $("#telefono").val() == "" || $("#diasArriendo").val() == "") {
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
                observacion: $('#observacion').val()
            };

            console.log(params);

            $.post('/Home/Create', params, function (data) {

                $('#mAdicionar').modal('hide');

                if (data == -1) {
                    $('#lblTituloMensaje').text("Alerta");
                    $('#lblMensaje').text("Se ha presentado un problema al adicionar el registro. Consulte con el administrador del sistema.");
                    $('#mMensaje').modal('show');
                }
                else {

                    if (data == -2) {
                        $('#lblTituloMensaje').text("Alerta");
                        $('#lblMensaje').text("El RUT del cliente no cumple con el formato aceptado. Intentelo nuevamente.");
                        $('#mMensaje').modal('show');
                    }

                    else {
                        $('#lblTituloMensaje').text("Mensaje");
                        $('#lblMensaje').text("El registro se ha añadido exitosamente.");
                        $('#mMensaje').modal('show');

                        $('#mAdicionar').modal('hide');

                        $("#rut").val('');
                        $("#nombres").val('');
                        $("#apellidos").val('');
                        $("#telefono").val('');
                        $("#selectedDisfraz").val(0);
                        $('#diasArriendo').val('');
                        $('#selectedTipoPago').val(0);

                        GetListado();
                    }
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