using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceApp
{
    public class DataBase
    {
        public static bool Contains(int id)
        {
            List<Entity> entities = DBFunctions.LoadDataBase();

            foreach (Entity e in entities)
            {
                if (e.Id == id)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ContainsCity(string city)
        {
            List<Entity> entities = DBFunctions.LoadDataBase();

            foreach (Entity e in entities)
            {
                if (e.City == city)
                {
                    return true;
                }
            }

            return false;
        }
        public static Entity ReadFromBase(int id)
        {
            Entity retVal = new Entity();
            bool exists = false;
            List<Entity> entities = DBFunctions.LoadDataBase();

            foreach (Entity e in entities)
            {
                if (e.Id == id)
                {
                    retVal = e;
                    exists = true;
                }
            }

            if (!exists)
            {
                retVal.Id = -1;
            }

            return retVal;
        }
        public static bool AddToBase(int id, string region, string city, int year, List<int> consumption)
        {
            List<Entity> entities = DBFunctions.LoadDataBase();

            foreach (Entity e in entities)
            {
                if (e.Id == id)
                {
                    return false;
                }
            }

            Entity newEntity = new Entity(id, region, city, year, consumption);

            entities.Add(newEntity);

            DBFunctions.WriteInDatabase(entities, DateTime.UtcNow);
            return true;
        }
        public static double CalculateAvgValue(string city)
        {
            List<Entity> entities = DBFunctions.LoadDataBase();
            int sum = 0;
            double retVal = 0;


            foreach (Entity e in entities)
            {
                if (e.City == city)
                {
                    foreach (int m in e.ConsuptionOfElectricity)
                    {
                        sum += m;
                    }
                    break;
                }
            }

            retVal = sum / 12;


            return retVal;
        }
        public static bool EditBase(int monthNo, int monthlyConsumption, int idOfEntity)
        {
            List<Entity> entities = DBFunctions.LoadDataBase();
            bool edited = false;


            foreach (Entity e in entities)
            {
                if (e.Id == idOfEntity)
                {
                    e.ConsuptionOfElectricity[monthNo - 1] = monthlyConsumption;
                    edited = true;
                    break;
                }
            }

            if (edited)
            {
                DBFunctions.WriteInDatabase(entities, DateTime.UtcNow);
                return true;
            }
            else
                return false;
        }
        public static bool DeleteFromBase(int id)
        {
            List<Entity> entities = DBFunctions.LoadDataBase();
            bool deleted = false;


            foreach (Entity e in entities)
            {
                if (e.Id == id)
                {
                    entities.Remove(e);
                    deleted = true;
                    break;
                }
            }

            if (deleted)
            {
                DBFunctions.WriteInDatabase(entities, DateTime.UtcNow);
                return true;
            }
            else
                return false;

        }
    }
}
