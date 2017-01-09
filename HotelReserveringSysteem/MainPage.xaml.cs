using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HotelReserveringSysteem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Kamer[] kamers;
        int dayNumber = 1;

        public MainPage()
        {
            this.InitializeComponent();

            kamers = new Kamer[312];

            int j = 0;

            for (int i = 1; i <= 313; i++)
            {
                if (i != 13)
                {
                    kamers[j] = new Kamer(i);
                    j++;
                }
            }

            textKamerBezetting.Text = "Dag in " + DateTime.Now.Year;
        }

        private void buttonBookRoom_Click(object sender, RoutedEventArgs e)
        {
            kamers[0].SetBezetting(5);
        }

        private void textBoxDayNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(textBoxDayNumber.Text, out dayNumber);

            if (dayNumber > 0 && dayNumber <= kamers[0].BezettingDitJaar.Count())
            {
                RefreshBezetting();
            }
        }

        private void RefreshBezetting()
        {
            listViewBezetting.Items.Clear();

            foreach (Kamer kamer in kamers)
            {
                listViewBezetting.Items.Add(kamer.KamerNR + " - " + kamer.BezettingDitJaar[dayNumber - 1]);
            }
        }

        private void listViewBezetting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBoxReserveringInfo.Text = "Kamer nummer: " + kamers[listViewBezetting.SelectedIndex].KamerNR + "\n" +
                "Aantal personen: " + /*kamers[listViewBezetting.SelectedIndex].AantalPersonenDitJaar[dayNumber - 1] +*/ "/" + kamers[listViewBezetting.SelectedIndex].PersonenPerKamer + "\n" +
                "";
        }
    }
}
