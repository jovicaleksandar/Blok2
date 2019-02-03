using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace ServiceApp
{
    public class DBFunctions
    {
        public static string LoadMyAddress()
        {
            string adresa = "";
            string port = "";
            string ret = "";

            using (XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.BaseDirectory + "ServerAdrese.xml"))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement() && reader.Name.Equals("current"))
                    {
                        while (true)
                        {
                            if (reader.Name.Equals("address"))
                            {
                                reader.Read();
                                adresa = reader.Value;
                                break;
                            }
                            else
                            {
                                reader.Read();
                            }
                        }

                        while (true)
                        {
                            if (reader.Name.Equals("port"))
                            {
                                reader.Read();
                                port = reader.Value;
                                break;
                            }
                            else
                            {
                                reader.Read();
                            }
                        }

                        ret = adresa + ":" + port;
                    }
                }
            }


            return ret;

        }

        public static List<string> CitanjeKonfiguracije()
        {
            List<string> services = new List<string>();
            string adresa = "";
            string port = "";

            using (XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.BaseDirectory + "ServerAdrese.xml"))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement() && reader.Name.Equals("service"))
                    {
                        while (true)
                        {
                            if (reader.Name.Equals("address"))
                            {
                                reader.Read();
                                adresa = reader.Value;
                                break;
                            }
                            else
                            {
                                reader.Read();
                            }
                        }

                        while (true)
                        {
                            if (reader.Name.Equals("port"))
                            {
                                reader.Read();
                                port = reader.Value;
                                break;
                            }
                            else
                            {
                                reader.Read();
                            }
                        }

                        string service = adresa + ":" + port;
                        services.Add(service);
                    }
                }
            }
            return services;
        }


        public static byte[] MyDatabase()
        {
            if (File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "DataBase.xml").Equals(""))
            {
                return null;
            }
            else if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "DataBase.xml"))
            {
                byte[] ret = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "DataBase.xml");
                return ret;
            }
            else
                return null;
        }

        public static bool CompareDataBases(byte[] recivedDB, byte[] myDB)
        {
            bool retVal = false;

            if (recivedDB.SequenceEqual(myDB))
            {
                retVal = true;
            }
            else
            {
                DateTime recDBTime = DesierializeXml(recivedDB);
                DateTime myDBTime = LoadMyDataBaseTime();
                List<Entity> recList = RecivedList();

                if (recDBTime.CompareTo(myDBTime) > 0)
                    WriteInDatabase(recList, recDBTime);
            }

            return retVal;
        }

        public static void WriteInDatabase(List<Entity> entities, DateTime recDBTime)
        {
            string filename = AppDomain.CurrentDomain.BaseDirectory + "DataBase.xml";

            XmlWriter writer = null;

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ("\t");
                settings.OmitXmlDeclaration = true;

                writer = XmlWriter.Create(filename, settings);

                writer.WriteStartElement("Entities");

                foreach (Entity e in entities)
                {
                    writer.WriteStartElement("Entity");
                    writer.WriteElementString("Id", e.Id.ToString());
                    writer.WriteElementString("Region", e.Region);
                    writer.WriteElementString("City", e.City);
                    writer.WriteElementString("Year", e.Year.ToString());
                    writer.WriteStartElement("ConsumptionOfElectricity");

                    foreach (int i in e.ConsuptionOfElectricity)
                    {
                        writer.WriteElementString("MonthlyConsumption", i.ToString());
                    }
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }

                writer.WriteElementString("DateTime", recDBTime.ToString());
                writer.WriteEndElement();

            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        public static DateTime LoadMyDataBaseTime()
        {
            DateTime retVal = DateTime.Now;

            using (XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.BaseDirectory + "DataBase.xml"))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement() && reader.Name.Equals("DateTime"))
                    {
                        reader.Read();

                        DateTime dateValue = DateTime.Parse(reader.Value);

                        retVal = dateValue;
                    }
                }
            }

            return retVal;
        }

        public static DateTime LoadRecievedDataBaseTime()
        {
            DateTime retVal = DateTime.Now;

            using (XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.BaseDirectory + "RecivedDB.xml"))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement() && reader.Name.Equals("DateTime"))
                    {
                        reader.Read();

                        DateTime dateValue = DateTime.Parse(reader.Value);

                        retVal = dateValue;
                    }
                }
            }

            return retVal;
        }

        public static List<Entity> LoadDataBase()
        {
            List<Entity> ret = new List<Entity>();

            int id = 0;
            string region = "";
            string city = "";
            int year = 0;

            using (XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.BaseDirectory + "DataBase.xml"))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement() && reader.Name.Equals("Entity"))
                    {
                        while (true)
                        {
                            if (reader.Name.Equals("Id"))
                            {
                                reader.Read();
                                int.TryParse(reader.Value, out id);
                                break;
                            }
                            else
                            {
                                reader.Read();
                            }
                        }

                        while (true)
                        {

                            if (reader.Name.Equals("Region"))
                            {
                                reader.Read();
                                region = reader.Value;
                                break;
                            }
                            else
                            {
                                reader.Read();
                            }
                        }

                        while (true)
                        {
                            if (reader.Name.Equals("City"))
                            {
                                reader.Read();
                                city = reader.Value;
                                break;
                            }
                            else
                            {
                                reader.Read();
                            }
                        }

                        while (true)
                        {
                            if (reader.Name.Equals("Year"))
                            {
                                reader.Read();
                                int.TryParse(reader.Value, out year);
                                break;
                            }
                            else
                            {
                                reader.Read();
                            }
                        }

                        while (true)
                        {
                            if (reader.Name.Equals("ConsumptionOfElectricity"))
                            {
                                List<int> consu = new List<int>();
                                reader.Read();
                                reader.Read();
                                reader.Read();

                                while (reader.Name != "ConsumptionOfElectricity")
                                {
                                    consu.Add(int.Parse(reader.Value));

                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    if (consu.Count != 12)
                                        reader.Read();
                                }

                                Entity e = new Entity(id, region, city, year, consu);
                                ret.Add(e);
                                break;
                            }
                            else
                            {
                                reader.Read();
                            }
                        }


                    }
                }
            }
            return ret;
        }

        public static DateTime DesierializeXml(byte[] recivedDB)
        {
            XmlDocument doc = new XmlDocument();
            MemoryStream ms = new MemoryStream(recivedDB);
            doc.Load(ms);

            doc.Save("RecivedDB.xml");

            return LoadRecievedDataBaseTime();
        }


        public static List<Entity> RecivedList()
        {
            List<Entity> retVal = new List<Entity>();



            int id = 0;
            string region = "";
            string city = "";
            int year = 0;

            using (XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.BaseDirectory + "RecivedDB.xml"))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement() && reader.Name.Equals("Entity"))
                    {
                        while (true)
                        {
                            if (reader.Name.Equals("Id"))
                            {
                                reader.Read();
                                int.TryParse(reader.Value, out id);
                                break;
                            }
                            else
                            {
                                reader.Read();
                            }
                        }

                        while (true)
                        {

                            if (reader.Name.Equals("Region"))
                            {
                                reader.Read();
                                region = reader.Value;
                                break;
                            }
                            else
                            {
                                reader.Read();
                            }
                        }

                        while (true)
                        {
                            if (reader.Name.Equals("City"))
                            {
                                reader.Read();
                                city = reader.Value;
                                break;
                            }
                            else
                            {
                                reader.Read();
                            }
                        }

                        while (true)
                        {
                            if (reader.Name.Equals("Year"))
                            {
                                reader.Read();
                                int.TryParse(reader.Value, out year);
                                break;
                            }
                            else
                            {
                                reader.Read();
                            }
                        }

                        while (true)
                        {
                            if (reader.Name.Equals("ConsumptionOfElectricity"))
                            {
                                List<int> consu = new List<int>();
                                reader.Read();
                                reader.Read();
                                reader.Read();

                                while (reader.Name != "ConsumptionOfElectricity")
                                {
                                    consu.Add(int.Parse(reader.Value));

                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    if (consu.Count != 12)
                                        reader.Read();
                                }

                                Entity e = new Entity(id, region, city, year, consu);
                                retVal.Add(e);
                                break;
                            }
                            else
                            {
                                reader.Read();
                            }
                        }
                    }
                }
            }

            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "RecivedDB.xml");
            return retVal;
        }
    }
}
