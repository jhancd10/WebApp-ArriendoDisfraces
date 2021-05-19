using DisfracesDonZancudo.Models;
using DisfracesDonZancudo.Models.Export;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisfracesDonZancudo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetListado()
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
                                 join disf in contexto.disfrazs.AsNoTracking()
                                 on serv.disfraz_id equals disf.id
                                 join tipoDisf in contexto.tipo_disfraz.AsNoTracking()
                                 on disf.tipo_disfraz_Id equals tipoDisf.id
                                 join tipoPag in contexto.tipo_pago.AsNoTracking()
                                 on serv.tipo_pago_id equals tipoPag.id
                                 select new
                                 {
                                     serv,
                                     client,
                                     disf,
                                     tipoDisf,
                                     tipoPag
                                 }).Select(x => new REPORTE_SERVICIOS() 
                                 {
                                     ID = x.serv.id,
                                     NOMBRECLIENTE = x.client.nombres + " " + x.client.apellidos,
                                     NOMBREDISFRAZ = x.disf.nombre,
                                     TIPOPAGO = x.tipoPag.nombre,
                                     TIPODISFRAZ = x.tipoDisf.nombre,
                                     FECHAARRIENDO = x.serv.fecha_arriendo,
                                     DIASARRIENDO = x.serv.dias_arriendo
                                 }).ToList();
                    
                    resultado.ForEach(item =>
                    {
                        item.FECHAFINALIZACION = item.FECHAARRIENDO.AddDays(item.DIASARRIENDO);
                    });
                    resultado = resultado.OrderByDescending(x => x.FECHAFINALIZACION).ToList();
                }
            }
            catch (Exception ex)
            {
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetListadoDisfraz()
        {
            List<disfraz> resultado = new List<disfraz>();
            try
            {
                using (DonZancudoEntities contexto = new DonZancudoEntities())
                {
                    contexto.Configuration.LazyLoadingEnabled = false;
                    contexto.Configuration.ProxyCreationEnabled = false;
                    resultado = (from row in contexto.disfrazs select row).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetListadoTipoDisfraz()
        {
            List<tipo_disfraz> resultado = new List<tipo_disfraz>();
            try
            {
                using (DonZancudoEntities contexto = new DonZancudoEntities())
                {
                    contexto.Configuration.LazyLoadingEnabled = false;
                    contexto.Configuration.ProxyCreationEnabled = false;
                    resultado = (from row in contexto.tipo_disfraz select row).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetListadoTipoPago()
        {
            List<tipo_pago> resultado = new List<tipo_pago>();
            try
            {
                using (DonZancudoEntities contexto = new DonZancudoEntities())
                {
                    contexto.Configuration.LazyLoadingEnabled = false;
                    contexto.Configuration.ProxyCreationEnabled = false;
                    resultado = (from row in contexto.tipo_pago select row).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(string rut, string nombres, string apellidos, int telefono, int disfrazId, DateTime fechaArriendo, 
            int diasArriendo, int tipoPagoId, string observacion)
        {
            int resultado = -1;
            try
            {
                using (DonZancudoEntities contexto = new DonZancudoEntities())
                {
                    contexto.Configuration.LazyLoadingEnabled = false;
                    contexto.Configuration.ProxyCreationEnabled = false;

                    int clienteId = 0;

                    cliente clienteAct = contexto.clientes.Where(x => x.rut == rut).FirstOrDefault();
                    if (clienteAct == null)
                    {
                        cliente newcliente = new cliente()
                        {
                            rut = rut,
                            nombres = nombres,
                            apellidos = apellidos,
                            telefono = telefono
                        };
                        contexto.clientes.Add(newcliente);
                        contexto.SaveChanges();

                        clienteId = contexto.clientes.Max(x => x.id);
                    }
                    else {
                        clienteId = clienteAct.id;
                    }

                    servicio newServicio = new servicio()
                    {
                        observacion = observacion,
                        fecha_arriendo = fechaArriendo,
                        dias_arriendo = diasArriendo,
                        cliente_id = clienteId,
                        disfraz_id = disfrazId,
                        tipo_pago_id = tipoPagoId
                    };
                    contexto.servicios.Add(newServicio);
                    contexto.SaveChanges();
                    resultado = 0;
                }
            }
            catch (Exception ex)
            {
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
    }
}