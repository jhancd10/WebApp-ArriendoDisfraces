using Core.Class;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Logic
{
    public class Servicios
    {
        public List<REPORTE_SERVICIOS> GetListadoArriendos()
        {
            List<REPORTE_SERVICIOS> resultado = new List<REPORTE_SERVICIOS>();
            try
            {
                using (DonZancudoEntities contexto = new DonZancudoEntities())
                {
                    contexto.Configuration.LazyLoadingEnabled = false;
                    contexto.Configuration.ProxyCreationEnabled = false;

                    resultado = (from serv in contexto.servicios.AsNoTracking()
                                 join client in contexto.clientes.AsNoTracking()
                                 on serv.cliente_id equals client.id
                                 join tipoclient in contexto.tipo_cliente.AsNoTracking()
                                 on client.tipo_cliente_id equals tipoclient.id
                                 join disf in contexto.disfrazs.AsNoTracking()
                                 on serv.disfraz_id equals disf.id
                                 join tipoDisf in contexto.tipo_disfraz.AsNoTracking()
                                 on disf.tipo_disfraz_id equals tipoDisf.id
                                 join tipoPag in contexto.tipo_pago.AsNoTracking()
                                 on serv.tipo_pago_id equals tipoPag.id
                                 select new
                                 {
                                     serv,
                                     client,
                                     tipoclient,
                                     disf,
                                     tipoDisf,
                                     tipoPag
                                 }).Select(x => new REPORTE_SERVICIOS()
                                 {
                                     ID = x.serv.id,
                                     NOMBRECLIENTE = x.client.nombres + " " + x.client.apellidos,
                                     TIPOCLIENTE = x.tipoclient.nombre,
                                     NOMBREDISFRAZ = x.disf.nombre,
                                     TIPOPAGO = x.tipoPag.nombre,
                                     TIPODISFRAZ = x.tipoDisf.nombre,
                                     FECHAARRIENDO = x.serv.fecha_arriendo,
                                     DIASARRIENDO = x.serv.dias_arriendo,
                                     CANTIDAD = x.serv.cantidad,
                                     ESTADO = x.serv.estado
                                 }).ToList();
                    
                    resultado.ForEach(item =>
                    {
                        item.FECHAFINALIZACION = item.FECHAARRIENDO.AddDays(item.DIASARRIENDO);
                    });
                    resultado = resultado.OrderByDescending(x => x.FECHAFINALIZACION).ToList();
                }
            }

            catch (Exception ex) { }

            return resultado;
        }

        public cliente GetClientexRut(string rut)
        {
            cliente cliente = null;
            try
            {
                using (DonZancudoEntities contexto = new DonZancudoEntities())
                {
                    cliente = contexto.clientes.Where(x => x.rut == rut).FirstOrDefault();
                }
            }

            catch (Exception ex) { }

            return cliente;
        }

        public List<servicio> GetArriendosActivosxCliente(int clienteId)
        {
            List<servicio> resultado = new List<servicio>();
            try
            {
                using (DonZancudoEntities contexto = new DonZancudoEntities())
                {
                    contexto.Configuration.LazyLoadingEnabled = false;
                    contexto.Configuration.ProxyCreationEnabled = false;

                    resultado = contexto.servicios.Where(x => x.cliente_id == clienteId && x.estado == true).ToList();
                }
            }

            catch (Exception ex) { }

            return resultado;
        }

        public Response ClientePuedeArrendar(int clienteId, int tipoClienteId, int cantidadNuevoServicio)
        {
            Response result = new Response();
            try
            {
                using (DonZancudoEntities contexto = new DonZancudoEntities())
                {
                    /* Consulta los Servicios activos del cliente */
                    List<servicio> serviciosCliente = GetArriendosActivosxCliente(clienteId);

                    /* Adicion del nuevo servicio */
                    int cantidad = serviciosCliente.Count + cantidadNuevoServicio;

                    tipo_cliente tipo_cliente = contexto.tipo_cliente.Where(x => x.id == tipoClienteId).FirstOrDefault();
                    if (cantidad > tipo_cliente.arriendos_permitidos)
                    {
                        if (tipo_cliente.arriendos_permitidos == 1)
                        {
                            result.Message = "El Cliente esta categorizado como '" + tipo_cliente.nombre + "'. " +
                                             "Solamente puede arrendar " + tipo_cliente.arriendos_permitidos + " disfraz.";
                        }
                        else
                        {
                            result.Message = "El Cliente esta categorizado como '" + tipo_cliente.nombre + "'. " + 
                                             "Solamente puede arrendar hasta " + tipo_cliente.arriendos_permitidos + 
                                             " disfraces simultaneos.";
                        }
                    }
                    else { result.Status = true; result.Message = ""; }
                }
            }

            catch (Exception ex) { }

            return result;
        }

        public Response UpgradeCategoryCliente(int clienteId)
        {
            Response result = new Response();
            try
            {
                using (DonZancudoEntities contexto = new DonZancudoEntities())
                {
                    cliente cliente = contexto.clientes.Where(x => x.id == clienteId).FirstOrDefault();
                    
                    /* Si el cliente esta categorizado como Normal o Premium, puede actualizarse. */
                    if (cliente.tipo_cliente_id != 3)
                    {
                        /* Si el clientes es categoria normal y cumple el requisito de 3 servicios totales puede ser premium */
                        if (cliente.tipo_cliente_id == 1 && cliente.total_servicios > 3) 
                        { 
                            cliente.tipo_cliente_id = 2;
                            contexto.SaveChanges();
                            result.Status = true;
                        }

                        /* Si el clientes es categoria premium y cumple el requisito de 13 servicios totales puede ser Empresarial */
                        else if (cliente.tipo_cliente_id == 2 && cliente.total_servicios > 13) 
                        { 
                            cliente.tipo_cliente_id = 3;
                            contexto.SaveChanges();
                            result.Status = true;
                        }
                    }
                }
            }

            catch (Exception ex) { }

            return result;
        }

        public bool UpdateTotalServiciosCliente(int clienteId)
        {
            bool result = false;
            try
            {
                using (DonZancudoEntities contexto = new DonZancudoEntities())
                {
                    cliente cliente = contexto.clientes.Where(x => x.id == clienteId).FirstOrDefault();
                    cliente.total_servicios = cliente.total_servicios + 1;
                    contexto.SaveChanges();
                    result = true;
                }
            }

            catch (Exception ex) { }

            return result;
        }

        public cliente CreateCliente(string rut, string nombres, string apellidos, int telefono)
        {
            cliente result = new cliente();
            try
            {
                using (DonZancudoEntities contexto = new DonZancudoEntities())
                {
                    cliente newcliente = new cliente()
                    {
                        rut = rut,
                        nombres = nombres,
                        apellidos = apellidos,
                        telefono = telefono,
                        total_servicios = 0,
                        tipo_cliente_id = 1 // (Tipo_Cliente_Id = 1) <=> "Cliente Normal"
                    };
                    contexto.clientes.Add(newcliente);
                    contexto.SaveChanges();

                    /* Consultamos el cliente insertado */
                    result = GetClientexRut(rut);
                }
            }

            catch (Exception ex) { }

            return result;
        }

        public disfraz GetDisfraz(int disfrazId)
        {
            disfraz disfraz = null;
            try
            {
                using (DonZancudoEntities contexto = new DonZancudoEntities())
                {
                    disfraz = contexto.disfrazs.Where(x => x.id == disfrazId).FirstOrDefault();
                }
            }

            catch (Exception ex) { }

            return disfraz;
        }

        public bool StockDisfraz(int disfrazId, int cantidadNuevoServicio)
        {
            bool result = false;
            try
            {
                using (DonZancudoEntities contexto = new DonZancudoEntities())
                {
                    disfraz disfraz = contexto.disfrazs.Where(x => x.id == disfrazId).FirstOrDefault();
                    if (disfraz != null)
                    {
                        if (disfraz.cantidad - cantidadNuevoServicio >= 0) { result = true; }
                    }
                }
            }

            catch (Exception ex) { }

            return result;
        }

        public bool UpdateStockDisfraz(int disfrazId, int cantidadNuevoServicio)
        {
            bool result = false;
            try
            {
                using (DonZancudoEntities contexto = new DonZancudoEntities())
                {
                    disfraz disfraz = contexto.disfrazs.Where(x => x.id == disfrazId).FirstOrDefault();
                    disfraz.cantidad = disfraz.cantidad - cantidadNuevoServicio;
                    contexto.SaveChanges();
                    result = true;
                }
            }

            catch (Exception ex) { }

            return result;
        }

        public bool CreateServicio(string observacion, DateTime fechaArriendo, int diasArriendo, int clienteId, 
            int disfrazId, int tipoPagoId, int cantidad)
        {
            bool result = false;
            try
            {
                using (DonZancudoEntities contexto = new DonZancudoEntities())
                {
                    servicio newServicio = new servicio()
                    {
                        observacion = observacion,
                        fecha_arriendo = fechaArriendo,
                        dias_arriendo = diasArriendo,
                        cliente_id = clienteId,
                        disfraz_id = disfrazId,
                        tipo_pago_id = tipoPagoId,
                        cantidad = cantidad,
                        estado = true
                    };
                    contexto.servicios.Add(newServicio);
                    contexto.SaveChanges();
                    result = true;
                }
            }

            catch (Exception ex) { }

            return result;
        }

        public Response Proceso(int clienteId, string observacion, DateTime fechaArriendo, int diasArriendo,
            int disfrazId, int tipoPagoId, int cantidad)
        {
            Response response = new Response();
            try
            {
                /* Consulta de Inventario del Disfraz */
                bool hayStock = StockDisfraz(disfrazId, cantidad);
                if (hayStock)
                {
                    /* Se actualiza el total de servicios del cliente */
                    bool updateServiciosCliente = UpdateTotalServiciosCliente(clienteId);
                    if (updateServiciosCliente)
                    {
                        /* Se actualiza el stock del disfraz */
                        bool updateStock = UpdateStockDisfraz(disfrazId, cantidad);
                        if (updateStock)
                        {
                            /* Creacion del servicio */
                            bool creacionServicio = CreateServicio(observacion, fechaArriendo, diasArriendo, clienteId, disfrazId, tipoPagoId, cantidad);
                            if (creacionServicio)
                            {
                                response.Message = "Operación Exitosa. ";
                                response.Status = true;

                                if (UpgradeCategoryCliente(clienteId).Status)
                                {
                                    response.Message += "El cliente fue promovido de categoria. Ahora se beneficiara de servicios simultaneos.";
                                }
                            }
                        }
                    }
                }

                else { response.Message = "No hay Stock disponible del disfraz."; }
            }

            catch (Exception ex) { }
            
            return response;
        }

        public Response Create(string rut, string nombres, string apellidos, int telefono, 
            int disfrazId, DateTime fechaArriendo, int diasArriendo, int tipoPagoId, string observacion, int cantidad)
        {
            Response response = new Response();
            try
            {
                /* Consulta si el cliente esta en la BD. */
                cliente clienteAct = GetClientexRut(rut);

                /* El cliente no existe */
                if (clienteAct == null)
                {
                    /* Creacion del cliente */
                    clienteAct = CreateCliente(rut, nombres, apellidos, telefono);

                    /* Consulta si el cliente puede realizar un nuevo servicio  */
                    Response consulta = ClientePuedeArrendar(clienteAct.id, clienteAct.tipo_cliente_id, cantidad);

                    if (!consulta.Status) { response.Message = consulta.Message; }

                    else
                    {
                        /* Metodo Maestro */
                        response = Proceso(clienteAct.id, observacion, fechaArriendo, diasArriendo, disfrazId, tipoPagoId, cantidad);
                    }
                }

                /* El cliente existe en BD */
                else
                {
                    /* Consulta si el cliente puede realizar un nuevo servicio  */
                    Response consulta = ClientePuedeArrendar(clienteAct.id, clienteAct.tipo_cliente_id, cantidad);

                    if (!consulta.Status) { response.Message = consulta.Message; }

                    else
                    {
                        /* Metodo Maestro */
                        response = Proceso(clienteAct.id, observacion, fechaArriendo, diasArriendo, disfrazId, tipoPagoId, cantidad);
                    }
                }
            }

            catch (Exception ex) { }

            return response;
        }

        public Response FinalizarArriendo(int servicioId)
        {
            Response response = new Response();
            response.Data = false;

            try
            {
                using (DonZancudoEntities contexto = new DonZancudoEntities())
                {
                    contexto.Configuration.LazyLoadingEnabled = false;
                    contexto.Configuration.ProxyCreationEnabled = false;

                    /* Se actualiza el servicio con estado False (Finalizado) */
                    servicio serv = contexto.servicios.Where(x => x.id == servicioId).FirstOrDefault();
                    serv.estado = false;
                    int disfrazId = serv.disfraz_id;
                    contexto.SaveChanges();

                    /* Se suma la cantidad devuelta al inventario */
                    var disfraz = contexto.disfrazs.Where(x => x.id == disfrazId).FirstOrDefault();
                    disfraz.cantidad = disfraz.cantidad + serv.cantidad;
                    contexto.SaveChanges();

                    var fechaInicio = serv.fecha_arriendo;
                    var diasArriendo = serv.dias_arriendo;
                    var fechaFinalizacion = fechaInicio.AddDays(diasArriendo);
                    
                    if (DateTime.Now.Date > fechaFinalizacion)
                    {
                        response.Message = "Se le cobrara una multa al cliente por pasarse de los dias arrendados. ";
                        response.Data = true;
                    }

                    response.Message += "Operación Exitosa.";
                    response.Status = true;
                }
            }

            catch (Exception ex) { }

            return response;
        }
    }
}
