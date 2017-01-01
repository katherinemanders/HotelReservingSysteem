using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelReserveringSysteem
{
    public class Kamer
    {
        private int kamerNR;
        private int personenPerKamer;

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

        #region methodes

        #endregion
    }
}