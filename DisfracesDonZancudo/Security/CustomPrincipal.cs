using System;
using System.Security.Principal;
using System.Web.Security;


namespace DisfracesDonZancudo.Security
{
    [Serializable]
    public class CustomPrincipal : RolePrincipal
    {
		public CustomPrincipal(CustomIdentity identity, string encryptedTicket) : base(identity, encryptedTicket)
		{
			Identity = identity;
		}
		#region Implementation of IPrincipal

		/// <summary>
		/// Determines whether the current principal belongs to the specified role.
		/// </summary>
		/// <returns>
		/// true if the current principal is a member of the specified role; otherwise, false.
		/// </returns>
		/// <param name="role">The name of the role for which to check membership. </param>
		public override bool IsInRole(string role)
        {
            return Identity is CustomIdentity &&
                   string.Compare(role, ((CustomIdentity)Identity).UserRoleName, StringComparison.CurrentCultureIgnoreCase) == 0;
        }

        /// <summary>
        /// Gets the identity of the current principal.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Security.Principal.IIdentity"/> object associated with the current principal.
        /// </returns>
        public override IIdentity Identity { get;  }

        #endregion

        public CustomIdentity CustomIdentity { get { return (CustomIdentity)Identity; } }

        
    }
}