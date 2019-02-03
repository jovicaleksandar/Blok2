using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Contracts;
using System.Xml;
using Manager;
using System.Security.Principal;
using System.Security.Cryptography.X509Certificates;

namespace ClientApp
{
    public class WCFClient : ChannelFactory<IWCFService>, IWCFService, IDisposable
    {
        IWCFService factory;
        public WCFClient(NetTcpBinding binding, EndpointAddress address)
                    : base(binding, address)
        {
            /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            //Console.WriteLine("CltCert: " + cltCertCN);


            //this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            //this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }

        #region Read()

        public Entity Read(int id)
        {
            Entity retVal = null;
            bool allowed = false;
            try
            {
                retVal = factory.Read(id);
                if (retVal != null)
                    allowed = true;

                if (allowed)
                {
                    Console.WriteLine("Read() allowed.");

                    if (retVal.Id == -1)
                    {
                        Console.WriteLine("Entity with that id: " + id + " does not exist!");
                    }
                    else
                    {
                        Console.WriteLine("ID:" + retVal.Id);
                        Console.WriteLine("Region:" + retVal.Region);
                        Console.WriteLine("City:" + retVal.City);
                        Console.WriteLine("Year:" + retVal.Year);
                        Console.WriteLine("Consumption of electricity:");

                        for (int i = 0; i < retVal.ConsuptionOfElectricity.Count; i++)
                        {
                            Console.WriteLine("Month " + (i + 1) + ": " + retVal.ConsuptionOfElectricity[i]);
                        }

                    }
                }
                else
                    Console.WriteLine("Read() not allowed.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Read(). {0}", e.Message);
            }

            return retVal;
        }

        #endregion

        #region Edit()

        public int Edit(int monthNo, int monthlyConsumption, int idOfEntity)
        {
            int allowed = 0;
            try
            {
                allowed = factory.Edit(monthNo, monthlyConsumption, idOfEntity);

                if (allowed == 1)
                {
                    Console.WriteLine("Edit() allowed.");
                    Console.WriteLine("Entity with id: " + idOfEntity + " changed monthly consumption for month number " + monthNo + " to " + monthlyConsumption);
                }
                else if(allowed == -1)
                {
                    Console.WriteLine("Entity with id: "+ idOfEntity+" does not exist!");
                }
                else
                    Console.WriteLine("Edit() not allowed.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Edit(). {0}", e.Message);
            }

            return allowed;
        }

        #endregion

        #region Add()

        public int Add(int id, string region, string city, int year, List<int> consumption)
        {
            int allowed = 0;
            try
            {
                allowed = factory.Add(id, region, city, year, consumption);

                if (allowed==1)
                {
                    Console.WriteLine("Add() allowed.");
                    Console.WriteLine("Entity (" + id + "," + region + "," + city + "," + year + ") successfully added");
                }
                else if(allowed == -1)
                {
                    Console.WriteLine("Entity with id: " + id + " alredy exist!");
                    return -1;
                }
                else
                {
                    Console.WriteLine("Add() not allowed.");
                    return 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Add(). {0}", e.Message);
            }

            return allowed;
        }
        #endregion

        #region Calculate
        public double Calculate(string city)
        {
            double retVal = -1;
            try
            {
                retVal = factory.Calculate(city);
                if (retVal == -1)
                {
                    Console.WriteLine("City: " + city + " doesn't exist!");
                }
                else if (retVal == -2)
                { 
                    Console.WriteLine("Calculate() not allowed.");
                }
                else
                {
                    Console.WriteLine("Calculate() allowed.");
                    Console.WriteLine("Calculation result: " + retVal);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Calculate(). {0}", e.Message);
            }

            return retVal;
        }
        #endregion

        #region Delete
        public int Delete(int id)
        {
            int allowed = 0;
            try
            {
                allowed = factory.Delete(id);

                if (allowed == 1)
                {
                    Console.WriteLine("Delete() allowed.");
                    Console.WriteLine("Entity with id: " + id + " successfully deleted");
                }
                else if(allowed == -1)
                {
                    Console.WriteLine("Entity with id: " + id + " does not exist!");
                    return -1;
                }
                else
                {
                    Console.WriteLine("Delete() not allowed.");
                    return 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Delete(). {0}", e.Message);
            }

            return allowed;
        }

        #endregion

      

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }
    }
}
