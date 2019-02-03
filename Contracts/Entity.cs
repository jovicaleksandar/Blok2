using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contracts
{
    public class Entity
    {
        private int id;
        private string region;
        private string city;
        private int year;
        private List<int> consuptionOfElectricity;

        public Entity()
        {

        }

        public Entity(int id, string reg, string city, int year, List<int> cons)
        {
            Id = id;
            Region = reg;
            City = city;
            Year = year;
            ConsuptionOfElectricity = cons;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        public List<int> ConsuptionOfElectricity
        {
            get { return consuptionOfElectricity; }
            set { consuptionOfElectricity = value; }
        }


    }
}
