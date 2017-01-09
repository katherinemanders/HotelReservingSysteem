using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Windows.Storage;

namespace HotelReserveringSysteem
{
    public class Kamer
    {
        private int kamerNR;
        private int personenPerKamer;
        private string[] bezettingDitJaar;
        private string[] aantalPersonenDitJaar;
        private string[] gegevensDitJaar;
        private string[] dinerOntbijtDitJaar;
        private string fileName;

        public int KamerNR
        {
            get
            {
                return kamerNR;
            }
            set
            {
                kamerNR = value;
            }
        }
        public int PersonenPerKamer
        {
            get
            {
                return personenPerKamer;
            }
            set
            {
                personenPerKamer = value;
            }
        }
        public string[] BezettingDitJaar
        {
            get
            {
                return bezettingDitJaar;
            }
            set
            {
                bezettingDitJaar = value;
            }
        }
        public string[] GegevensDitJaar
        {
            get
            {
                return gegevensDitJaar;
            }
            set
            {
                gegevensDitJaar = value;
            }
        }
        public string[] DinerOntbijtDitJaar
        {
            get
            {
                return dinerOntbijtDitJaar;
            }
            set
            {
                dinerOntbijtDitJaar = value;
            }
        }
        public string[] AantalPersonenDitJaar
        {
            get
            {
                return aantalPersonenDitJaar;
            }
            set
            {
                aantalPersonenDitJaar = value;
            }
        }

        public Kamer(int nummer)
        {
            PersonenPerKamer = 4;
            KamerNR = nummer;
            
            GetData();
        }

        #region methodes
        private async void GetData()
        {
            StorageFolder appFolder = ApplicationData.Current.LocalFolder;

            fileName = "Kamer" + KamerNR + DateTime.Now.Year + ".txt";

            StorageFile kamerBestand = await appFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            string text = await FileIO.ReadTextAsync(kamerBestand);

            if (text.Split(';').Count() < 3)
            {
                string tempString = string.Empty;

                for (int i = 1; i <= (DateTime.Now.Year % 4 == 0 ? 366 : 365); i++)
                {
                    //bezetting
                    tempString += "vrij,";
                }

                tempString = tempString.Remove(tempString.Length - 1, 1) + ";";

                for (int i = 1; i <= (DateTime.Now.Year % 4 == 0 ? 366 : 365); i++)
                {
                    //aantal personen
                    tempString += "0(";

                    for (int j = 0; j < PersonenPerKamer; j++)
                    {
                        //persoonsgegevens
                        tempString += "naam|adres|woonplaats|leeftijd|geslacht&";
                    }

                    tempString = tempString.Remove(tempString.Length - 1, 1) + ");";
                }

                for (int i = 1; i <= (DateTime.Now.Year % 4 == 0 ? 366 : 365); i++)
                {
                    //ontbijt/diner aantal personen
                    tempString += "0/0,";
                }

                tempString = tempString.Remove(tempString.Length - 1, 1);

                await FileIO.WriteTextAsync(kamerBestand, tempString);
            }

            GetBezettingDitJaarVanBestand();
        }

        private async void GetBezettingDitJaarVanBestand()
        {
            StorageFolder appFolder = ApplicationData.Current.LocalFolder;

            fileName = "Kamer" + KamerNR + DateTime.Now.Year + ".txt";

            StorageFile kamerBestand = await appFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            string text = await FileIO.ReadTextAsync(kamerBestand);

            BezettingDitJaar = text.Split(';')[0].Split(',');
            AantalPersonenDitJaar = text.Split(';')[1].Split('(');

            for (int i = 0; i < AantalPersonenDitJaar.Count(); i++)
            {
                AantalPersonenDitJaar[i] = AantalPersonenDitJaar[i].Substring(AantalPersonenDitJaar[i].Length - 1);
            }

            Debug.WriteLine("Data opgehaald voor: " + KamerNR);
        }

        public async void SetBezetting(int dag)
        {
            BezettingDitJaar[dag - 1] = "bezet";

            StorageFolder appFolder = ApplicationData.Current.LocalFolder;

            fileName = "Kamer" + KamerNR + DateTime.Now.Year + ".txt";

            StorageFile kamerBestand = await appFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            string text = await FileIO.ReadTextAsync(kamerBestand);

            string tempString = string.Join(",", BezettingDitJaar);

            await FileIO.WriteTextAsync(kamerBestand, tempString + ";" + text.Split(';')[1] + ";" + text.Split(';')[2]);
        }
        #endregion
    }
}