using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading;
using System.Xml;
using Contracts;
using Manager;
using RBAC;

namespace ServiceApp
{
    public class WCFService : IWCFService
    {
        public Entity Read(int id)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            var permission = Permissions.Read.ToString().ToLower();

            if (principal.IsInRole(permission))
            {
                Console.WriteLine("Read() successfully executed.");

                Entity e = DataBase.ReadFromBase(id);
                
                return e;
            }
            else
            {
                Console.WriteLine("Read() unsuccessfully executed.");
                Entity e = null;
                return e;
            }
        }

        public int Edit(int monthNo, int monthlyConsumption, int idOfEntity)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string a = principal.Identity.Name;
            var permission = Permissions.Edit.ToString().ToLower();

            if (principal.IsInRole(permission))
            {
                if(DataBase.Contains(idOfEntity))
                {
                    Console.WriteLine("Edit() successfully executed.");
                    DataBase.EditBase(monthNo, monthlyConsumption, idOfEntity);
                    Logger log = new Logger();
                    log.WriteToEventLog("NAS", a, idOfEntity, "Edited in DataBase!");
                    return 1;
                }
                else
                {
                    Console.WriteLine("Entity with id: " + idOfEntity + " does not exist!");
                    return -1;
                }
            }
            else
            {
                Console.WriteLine("Edit() unsuccessfully executed.");
                return 0;
            }
        }

        public int Add(int id, string region, string city, int year, List<int> consumption)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string a = principal.Identity.Name;
            var permission = Permissions.Add.ToString().ToLower();

            if (principal.IsInRole(permission))
            {
                if(!DataBase.Contains(id))
                {
                    Console.WriteLine("Add() successfully executed.");
                    DataBase.AddToBase(id, region, city, year, consumption);
                    Logger log = new Logger();
                    log.WriteToEventLog("NAS", a, id, "Added to DataBase!");
                    return 1;
                }
                else
                {
                    Console.WriteLine("Entity with id: " + id + " alredy exist!");
                    return -1;
                }
            }
            else
            {
                Console.WriteLine("Add() unsuccessfully executed.");
                return 0;
            }
        }

        public int Delete(int id)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string a = principal.Identity.Name;
            var permission = Permissions.Delete.ToString().ToLower();

            if (principal.IsInRole(permission))
            {
                if(DataBase.Contains(id))
                {
                    Console.WriteLine("Delete() successfully executed.");
                    DataBase.DeleteFromBase(id);
                    Logger log = new Logger();
                    log.WriteToEventLog("NAS", a, id, "Deleted from DataBase!");
                    return 1;
                }
                else
                {
                    Console.WriteLine("Entity with id: " + id + " does not exist!");
                    return -1;
                }
            }
            else
            {
                Console.WriteLine("Delete() unsuccessfully executed.");
                return 0;
            }
        }
        public double Calculate(string city)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            double retVal = 0;

            var permission = Permissions.Calculate.ToString().ToLower();

            if (principal.IsInRole(permission))
            {
                if (DataBase.ContainsCity(city))
                {
                    Console.WriteLine("Calculate() successfully executed.");
                    retVal = DataBase.CalculateAvgValue(city);
                    return retVal;
                }
                else
                {
                    Console.WriteLine("City: " + city + " doesn't exist!");
                    return -1;
                }
            }
            else
            {
                Console.WriteLine("Calculate() unsuccessfully executed.");
                return -2;
            }
        }
    }
}
