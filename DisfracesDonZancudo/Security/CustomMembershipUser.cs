using DisfracesDonZancudo.Models.Export;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace DisfracesDonZancudo.Security
{
    public class CustomMembershipUser : MembershipUser
    {
        #region Properties

        public string Nombre { get; set; }

        public string Identificacion { get; set; }

        public int Id_Empleado { get; set; }

        public int UserRoleId { get; set; }

        public string UserRoleName { get; set; }

        public int UserEmpresaId { get; set; }

        public string UserEmpresaName { get; set; }

        public string UserEmpresaLogo { get; set; }

        public int UserEstadoId { get; set; }

        public string UserEstadoName { get; set; }

		#endregion

		public CustomMembershipUser(UserLogin user)
			: base("CustomMembershipProvider", user.Username, user.Id, user.Password, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            Nombre = user.Username;
			
        }

	

		
    }
}