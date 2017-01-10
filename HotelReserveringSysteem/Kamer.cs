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
        private List<Personen[]> personenDitJaar;
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
        public List<Personen[]> PersonenDitJaar
        {
            get
            {
                return personenDitJaar;
            }
            set
            {
                personenDitJaar = value;
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
                        tempString += "naam|adres|woonplaats|0|geslacht&";
                    }

                    tempString = tempString.Remove(tempString.Length - 1, 1) + ")";
                }

                tempString += ";";

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
            PersonenDitJaar = new List<Personen[]>();

            foreach (string personen in text.Split(';')[1].Split(')'))
            {
                if (personen.IndexOf('(') >= 0 && personen.IndexOf('&') >= 0)
                {
                    personen.Substring(personen.IndexOf('('), personen.LastIndexOf('&'));
                    Personen[] tempPersonen = new Personen[personen.Split('&').Length];

                    for (int i = 0; i < tempPersonen.Length; i++)
                    {
                        tempPersonen[i] = new Personen();

                        if (personen.Split('|')[0].Contains(' '))
                        {
                            tempPersonen[i].Voornaam = personen.Split('|')[0].Split(' ')[0];
                            tempPersonen[i].Achternaam = personen.Split('|')[0].Split(' ')[1];
                        }
                        else
                        {
                            tempPersonen[i].Voornaam = "voornaam";
                            tempPersonen[i].Achternaam = "achternaam";
                        }
                        tempPersonen[i].Adres = personen.Split('|')[1];
                        tempPersonen[i].Woonplaats = personen.Split('|')[2];
                        tempPersonen[i].Leeftijd = int.Parse(personen.Split('|')[3]);
                        tempPersonen[i].Geslacht = personen.Split('|')[4];
                    }

                    PersonenDitJaar.Add(tempPersonen);
                }
            }

            for (int i = 0; i < AantalPersonenDitJaar.Count() - 1; i++)
            {
                AantalPersonenDitJaar[i] = AantalPersonenDitJaar[i].Substring(AantalPersonenDitJaar[i].Length - 1);
            }

            Debug.WriteLine("Data opgehaald voor: " + KamerNR);
        }

        public async void SetBezetting(int dag, List<Personen> personen)
        {
            BezettingDitJaar[dag - 1] = "bezet";
            for (int i = 0; i < personen.Count(); i++)
            {
                PersonenDitJaar[dag - 1] = personen.ToArray();//TODO: dit werkt nog niet... de personen lijst klopt wel maar wordt niet overgezet naar de personenditjaar lijst
            }

            StorageFolder appFolder = ApplicationData.Current.LocalFolder;

            fileName = "Kamer" + KamerNR + DateTime.Now.Year + ".txt";

            StorageFile kamerBestand = await appFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            string text = await FileIO.ReadTextAsync(kamerBestand);

            string tempString = string.Join(",", BezettingDitJaar);

            string tempStringPersonen = "";

            foreach (Personen[] pers in PersonenDitJaar)
            {
                tempStringPersonen += pers.Length.ToString() + "(";

                foreach (Personen per in pers)
                {
                    tempStringPersonen += per.Voornaam + " " + per.Achternaam + "|";
                    tempStringPersonen += per.Adres + "|";
                    tempStringPersonen += per.Woonplaats + "|";
                    tempStringPersonen += per.Leeftijd.ToString() + "|";
                    tempStringPersonen += per.Geslacht + "&";
                }

                tempStringPersonen += ")";
            }

            await FileIO.WriteTextAsync(kamerBestand, tempString + ";" + text.Split(';')[1] + ";" + text.Split(';')[2]);
        }
        #endregion
    }
}