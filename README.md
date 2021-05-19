# WebApp-ArriendoDisfraces
Aplicación Web en C#.NET y EntityFramework que permita registrar los arriendos de los disfraces realizados en la tienda “Disfraces Don Zancudo”.

## Descripción:
Los arriendos de los disfraces en la tienda “Disfraces Don Zancudo” se han duplicado en el último mes, sin embargo no son capaces de registrar tantas ventas ya que la afluencia de público es demasiado para el actual sistema de registro. Es por este motivo que se le ha contactado y propuesto la realización de un sistema el cual pueda registrar los arriendos y estos queden asociados a los clientes.
Los usuarios del sistema corresponderán al personal que manejará el sistema en la tienda de disfraces, por lo que para ingresar al sistema estos deberán proporcionar sus credenciales (Nombre Usuario y Password).

* Para poder realizar el arriendo de un disfraz es necesario registrar El RUT del cliente, nombres, apellidos y teléfono. 
* El Teléfono debe tener 9 dígitos y el rut debe tener formato 99999999-X, los nombres y apellidos deben tener un máximo de 25 caracteres.
* En el caso del disfraz se debe ingresar el nombre, la fecha de arriendo, los días por los que estará arrendado el disfraz, se debe seleccionar el tipo de pago y finalmente seleccionar el tipo de disfraz.
* Todos los datos son de carácter obligatorio y la fecha debe ser menor a la fecha actual  y la cantidad de días de arriendo no puede ser mayor a 60.

-------------------------------------------------------------------------------------------------------------------------------------------------------

## Reporte:
Se requiere mostrar una tabla para el administrador con la siguiente información:
- Nombre Cliente: Corresponderá al nombre completo del cliente.
- Nombre Disfraz.
- Tipo Pago: Nombre tipo pago
- Tipo Disfraz: Nombre tipo del disfraz.
- Fecha de arriendo: la fecha de arriendo del disfraz.
- Fecha de termino arriendo: la fecha de arriendo mas los días de arriendo. 

**Las fechas deben tener el formato dd-MM-yyyy**
