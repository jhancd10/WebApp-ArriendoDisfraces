using System;
using System.Security.Principal;
using System.Web.Security;


namespace DisfracesDonZancudo.Security
{
    [Serializable]
    public class CustomIdentity : IIdentity
    {
        #region Properties

        public IIdentity Identity { get; set; }

        public int UserRoleId { get; set; }

        public string Nombre { get; set; }

        public string Identificacion { get; set; }

        public int Id_Empleado { get; set; }

        public string UserRoleName { get; set; }

        public int UserEmpresaId { get; set; }

        public string UserEmpresaName { get; set; }

        public string UserEmpresaLogo { get; set; }

        public int UserEstadoId { get; set; }

        public string UserEstadoName { get; set; }

        #endregion

        #region Implementation of IIdentity

        /// <summary>
        /// Gets the name of the current user.
        /// </summary>
        /// <returns>
        /// The name of the user on whose behalf the code is running.
        /// </returns>
        public string Name
        {
            get { return Identity.Name; }
        }

        /// <summary>
        /// Gets the type of authentication used.
        /// </summary>
        /// <returns>
        /// The type of authentication used to identify the user.
        /// </returns>
        public string AuthenticationType
        {
            get { return Identity.AuthenticationType; }
        }

        /// <summary>
        /// Gets a value that indicates whether the user has been authenticated.
        /// </summary>
        /// <returns>
        /// true if the user was authenticated; otherwise, false.
        /// </returns>
        public bool IsAuthenticated { get { return Identity.IsAuthenticated; } }

        #endregion

        #region Constructor

        public CustomIdentity(IIdentity identity)
        {
            Identity = identity;

            var customMembershipUser = (CustomMembershipUser)Membership.GetUser(identity.Name);
            if (customMembershipUser != null)
            {
                Nombre = customMembershipUser.UserName;

            }
        }

        #endregion
    }
}