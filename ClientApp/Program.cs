using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using Manager;
using System.Threading;
using System.Security.Principal;

namespace ClientApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Unesite broj porta: ");
            int brojPorta = Int32.Parse(Console.ReadLine());

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string address = "net.tcp://localhost:" + brojPorta + "/WCFService";
            string srvCertCN = WindowsIdentity.GetCurrent().Name;
            srvCertCN = Formatter.ParseName(srvCertCN);
            Console.WriteLine(srvCertCN);
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, "serverSertifikat");
            EndpointAddress address2 = new EndpointAddress(new Uri(address),
                                      new X509CertificateEndpointIdentity(srvCert));
            //string grupa = CertManager.GetOu(StoreName.My, StoreLocation.LocalMachine, "klijentSertifikat");
            //Console.WriteLine("Ovo je grupa "+ grupa);
            int response;
            using (WCFClient proxy = new WCFClient(binding, address2))
            {
                do
                {
                    response = ClientMenu.Menu();
                    switch (response)
                    {
                        case 1:
                            Console.WriteLine("Enter Id(must be number):");
                            int id;
                            int.TryParse(Console.ReadLine(), out id);
                            Console.WriteLine("Enter region:");
                            string reg = Console.ReadLine();
                            Console.WriteLine("Enter city:");
                            string city = Console.ReadLine();
                            Console.WriteLine("Enter year:");
                            int year = int.Parse(Console.ReadLine());
                            List<int> consum = new List<int>();
                            int m = 0;
                            for (int i = 0; i < 12; i++)
                            {
                                Console.WriteLine("Enter consumption of electricity for month" + (i + 1));
                                int.TryParse(Console.ReadLine(), out m);
                                //m = int.Parse(Console.ReadLine());
                                consum.Add(m);
                            }
                            proxy.Add(id, reg, city, year, consum);
                            break;
                        case 2:
                            Console.WriteLine("Enter id of entity you want to read:");
                            int Id = int.Parse(Console.ReadLine());
                            proxy.Read(Id);
                            break;
                        case 3:
                            Console.WriteLine("Enter id of entity you want to edit:");
                            int idOfEntity = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter month number you want to edit consumption of electricity:");
                            int monthNo = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter new value for consumption of electricity:");
                            int consumption = int.Parse(Console.ReadLine());
                            proxy.Edit(monthNo, consumption, idOfEntity);
                            break;
                        case 4:
                            Console.WriteLine("Enter id of entity you want to delete:");
                            int IdE = int.Parse(Console.ReadLine());
                            proxy.Delete(IdE);
                            break;
                        case 5:
                            Console.WriteLine("Enter city name for which you want to calculate consumption of electricity:");
                            string c = Console.ReadLine();
                            proxy.Calculate(c);
                            break;
                        case 6:
                            Console.WriteLine("Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Wrong command. Try agian");
                            break;

                    }
                } while (response != 6);
            }
        }
    }
}
