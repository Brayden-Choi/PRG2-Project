//============================================================
// Student Number : S10205942, S10205045
// Student Name : Elsa Lee Ting, Brayden Choi Di Rong
// Module Group : T06
//============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace PRG2_T06_Team9
{
    class Visitor : Person
    {
        public string PassportNo { get; set; }
        public string Nationality { get; set; }

        public Visitor(string name, string passportNo, string nationality) : base(name)
        {
            PassportNo = passportNo;
            Nationality = nationality;
        }

        public override double CalculateSHNCharges()
        {
<<<<<<< HEAD
            //Checking for the last country of embarkation
=======
            double totalcost = 0;
>>>>>>> 41cb2aaae0e4e9dc16e3681390db287a65e02eb6

            for (int i = 0; i < TravelEntryList.Count; i++)
            {
                if (TravelEntryList[i].LastCountryOfEmbarkation == "New Zealand" || TravelEntryList[i].LastCountryOfEmbarkation == "Vietnam" || TravelEntryList[i].LastCountryOfEmbarkation == "Macao SAR")
                {
                    totalcost += 200 + 80;
                }
                else
                {
                    double transportationCost = TravelEntryList[i].ShnStay.CalculateTravelCost();
                    totalcost += transportationCost + 200 + 2000;
                }
            }
            return totalcost;
        }

        public override string ToString()
        {
            return PassportNo + Nationality;
        }
    }
}
