using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace ServiceApp
{
    public class Update : IUpdate
    {
        public byte[] IntegrityUdate()
        {
            byte[] ret = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "DataBase.xml");

            return ret;
        }
    }
}
