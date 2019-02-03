using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Security.Principal;
using System.Security.Cryptography.X509Certificates;
using Manager;

namespace RBAC
{
    public class CustomAuthorizationPolicy : IAuthorizationPolicy
    {
        private string id;
        private object locker = new object();

        public CustomAuthorizationPolicy()
        {
            this.id = Guid.NewGuid().ToString();
        }

        public string Id
        {
            get { return this.id; }
        }

        public ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            object list;

            if (!evaluationContext.Properties.TryGetValue("Identities", out list))
            {
                return false;
            }

            IList<IIdentity> identities = list as IList<IIdentity>;
            if (list == null || identities.Count <= 0)
            {
                return false;
            }
            string[] parts = new string[] { };
            parts = identities[0].Name.Split(',');
            string[] cn = new string[] { };
            cn = parts[0].Split('=');
            string[] ou = new string[] { };
            ou = parts[1].Split('=');
            string group = ou[1];
         
            evaluationContext.Properties["Principal"] = GetPrincipal(identities[0], group);
            return true;
        }

        protected virtual IPrincipal GetPrincipal(IIdentity identity, string grupa)
        {
            lock (locker)
            {
                IPrincipal principal = null;
                //lose kastuje
                WindowsIdentity windowsIdentity = identity as WindowsIdentity;

               if (identity != null)
               {
                    /// audit successfull authentication						
                    principal = new CustomPrincipal(identity, grupa );
               }

                return principal;
            }
        }
    }
}
