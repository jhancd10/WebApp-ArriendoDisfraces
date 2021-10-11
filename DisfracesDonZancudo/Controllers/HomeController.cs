using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Logic;
using Core.Class;
using Core.Model;

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
                Servicios Servicios = new Servicios();
                resultado = Servicios.GetListadoArriendos();
            }
            catch (Exception ex) { }

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
                    resultado = contexto.disfrazs.Where(x => x.cantidad != 0).ToList();
                }
            }

            catch (Exception ex) { }

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

            catch (Exception ex) { }

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
            
            catch (Exception ex) { }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateArriendo(string rut, string nombres, string apellidos, int telefono, 
            int disfrazId, DateTime fechaArriendo, int diasArriendo, int tipoPagoId, string observacion, int cantidad)
        {
            Response resultado = new Response();
            try
            {
                Servicios Servicios = new Servicios();
                resultado = Servicios.Create(rut, nombres, apellidos, telefono, disfrazId, 
                    fechaArriendo, diasArriendo, tipoPagoId, observacion, cantidad);
            }
            catch (Exception ex) { }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FinalizarArriendo(int ServicioId)
        {
            Response resultado = new Response();
            try
            {
                Servicios Servicios = new Servicios();
                resultado = Servicios.FinalizarArriendo(ServicioId);
            }

            catch (Exception ex) { }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
    }
}