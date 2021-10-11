using Core.Model;
using DisfracesDonZancudo.Models.Export;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace DisfracesDonZancudo.Controllers
{
    public class SecurityController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            try
            {
                using (DonZancudoEntities contexto = new DonZancudoEntities())
                {
                    username = username.Trim();
                    var usr = contexto.usuarios.FirstOrDefault(x => x.username == username);

                    if (usr != null)
                    {
                        if (password.Equals(usr.password))
                        {
                            var serialUsr = new UserLogin()
                            {
                                Id = usr.id,
                                Username = usr.username,
                                Password = usr.password
                            };

                            if (ModelState.IsValid)
                            {
                                FormsAuthentication.RedirectFromLoginPage(serialUsr.Username, true);
                                var serializer = new JavaScriptSerializer();
                                string userData = serializer.Serialize(serialUsr);
                                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, serialUsr.Username, DateTime.Now, DateTime.Now.AddDays(30), true, userData, FormsAuthentication.FormsCookiePath);
                                //    Encrypt the ticket.
                                string encTicket = FormsAuthentication.Encrypt(ticket);
                                //  Create the cookie.
                                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                                return RedirectToAction("Index", "Home");
                            }
                            else { ModelState.AddModelError("Login", "Se ha producido un error inesperado. Vuelve a intentarlo."); }
                        }
                        else { ModelState.AddModelError("Login", "La contraseña que ingresaste es incorrecta. Vuelve a intentarlo."); }
                    }
                    else { ModelState.AddModelError("Login", "Ingrese un nombre de usuario válido. Vuelve a intentarlo."); }

                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Login", "No se puede iniciar sesión. Comprueba tu conexión de red.");
                return View();
            }
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Security", null);
        }
    }
}