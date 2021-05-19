using DisfracesDonZancudo.Models.Export;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace DisfracesDonZancudo.Security
{
	public class UserSerializer
	{


		public UserLogin UserRole(HttpRequestBase request,string userData) {

			HttpCookie authCookie = request.Cookies[FormsAuthentication.FormsCookieName];
			UserLogin userPrincipal = new UserLogin();
			if (authCookie != null)
			{
				// Get the forms authentication ticket.
				FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
				var identity = new System.Security.Principal.GenericIdentity(authTicket.Name, "Forms");
				// Get the custom user data encrypted in the ticket.			

				// Deserialize the json data and set it on the custom principal.
				var serializer = new JavaScriptSerializer();
				userPrincipal = (UserLogin)serializer.Deserialize(userData, typeof(Models.Export.UserLogin));
				// Set the context user.	
				return userPrincipal;

			}
			return new UserLogin();

		}
}
}