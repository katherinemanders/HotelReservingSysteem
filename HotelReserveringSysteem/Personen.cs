using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReserveringSysteem
{
    public class Personen
    {
        private string voornaam;

        public string Voornaam
        {
            get { return voornaam; }
            set { voornaam = value; }
        }

        private string achternaam;

        public string Achternaam
        {
            get { return achternaam; }
            set { achternaam = value; }
        }

        private int leeftijd;

        public int Leeftijd
        {
            get { return leeftijd; }
            set { leeftijd = value; }
        }

        private string adres;

        public string Adres
        {
            get { return adres; }
            set { adres = value; }
        }

        private string woonplaats;

        public string Woonplaats
        {
            get { return woonplaats; }
            set { woonplaats = value; }
        }

        private string geslacht;

        public string Geslacht
        {
            get { return geslacht; }
            set { geslacht = value; }
        }

        public override string ToString()
        {
            string tempString = Voornaam + " " + Achternaam + ", " + Adres + ", " + Woonplaats + ", " + Leeftijd.ToString() + ", " + Geslacht;

            return tempString;
        }
    }
}
