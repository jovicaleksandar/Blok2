using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace RBAC
{
    public class CustomPrincipal : IPrincipal
    {
        private IIdentity winId = null;
        private List<string> Roles = new List<string>();

        public IIdentity Identity
        {
            get { return this.winId; }
        }

        public CustomPrincipal(IIdentity winIdentity, string Certifikat)
        {
            this.winId = winIdentity;
            
            List<string> permissions = RBACConfigParser.GetPermissions(Certifikat);

            foreach (string p in permissions)
                Roles.Add(p);
        }

        public bool IsInRole(string role)
        {
            foreach (var group in Roles)
            {
                if (Roles.Contains(role))
                    return true;
            }

            return false;
        }
    }
}
