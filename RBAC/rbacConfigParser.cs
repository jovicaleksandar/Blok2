using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RBAC
{
    static class RBACConfigParser
    {
        private static readonly string path = AppDomain.CurrentDomain.BaseDirectory + "rbac_config.xml";

        public static List<string> GetPermissions(string group)
        {
            var list = new List<string>();
            using (var reader = XmlReader.Create(path))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement() && reader.Name.Equals(group))
                        while (true)
                        {
                            if(reader.Name.Equals("permission"))
                            {
                                reader.Read();
                                if(reader.Value != "\n")
                                {
                                    list.Add(reader.Value);
                                }
                            }
                            else if(reader.NodeType == XmlNodeType.EndElement && reader.Name == group)
                            {
                                break;
                            }
                            else
                            {
                                reader.Read();
                            }
                        }
                }
            }

            return list;
        }
    }
}
