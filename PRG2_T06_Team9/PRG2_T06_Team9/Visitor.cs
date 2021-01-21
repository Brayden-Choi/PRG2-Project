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
            double totalcost = 0;

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
