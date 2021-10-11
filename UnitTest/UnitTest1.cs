using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Core.Logic;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestServicio_ClienteNuevo()
        {
            Servicios Servicios = new Servicios();

            string rut = "197745746";
            string nombres = "Cesar";
            string apellidos = "Valle";
            int telefono = 31783608;
            int disfrazId = 4; //Jefe: Squid Game
            DateTime fechaArriendo = DateTime.Now;
            int diasArriendo = 7;
            int tipoPagoId = 2; //Tarjeta Debito 
            string observacion = "Cliente dice ser cumplido.";
            int cantidad = 1;

            var resultado = Servicios.Create(rut, nombres, apellidos, telefono, disfrazId,
                            fechaArriendo, diasArriendo, tipoPagoId, observacion, cantidad);

            Assert.IsTrue(resultado.Status);
        }

        [TestMethod]
        public void TestServicio_ClienteRegistrado_RolNormal()
        {
            Servicios Servicios = new Servicios();

            string rut = "197745746";
            string nombres = "Cesar";
            string apellidos = "Valle";
            int telefono = 31783608;
            int disfrazId = 4; //Jefe: Squid Game
            DateTime fechaArriendo = DateTime.Now;
            int diasArriendo = 7;
            int tipoPagoId = 2; //Tarjeta Debito 
            string observacion = "Cliente dice ser cumplido.";
            int cantidad = 2;

            var resultado = Servicios.Create(rut, nombres, apellidos, telefono, disfrazId,
                            fechaArriendo, diasArriendo, tipoPagoId, observacion, cantidad);

            Assert.IsFalse(resultado.Status);
        }

        [TestMethod]
        public void TestServicio_ServicioActivo()
        {
            Servicios Servicios = new Servicios();

            int servicioId = 1;
            var resultado = Servicios.FinalizarArriendo(servicioId);
            Assert.IsTrue(resultado.Status);
        }
    }
}
