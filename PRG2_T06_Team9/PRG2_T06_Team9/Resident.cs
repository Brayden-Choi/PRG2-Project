//============================================================
// Student Number : S10205942, S10205045
// Student Name : Elsa Lee Ting, Brayden Choi Di Rong
// Module Group : T06
//============================================================

using System;

namespace PRG2_T06_Team9
{
    class Resident : Person
    {
        public string Address { get; set; }
        public DateTime LastLeftCountry { get; set; }
        public TraceTogetherToken Token { get; set; }

        public Resident(string name, string address, DateTime lastLeftCountry) : base(name)
        {
            Address = address;
            LastLeftCountry = lastLeftCountry;
        }

        public override double CalculateSHNCharges()
        {
            double totalcost = 0;

            for (int i = 0; i < TravelEntryList.Count; i++)
            {
                if (TravelEntryList[i].IsPaid == false)
                {
                    if (TravelEntryList[i].LastCountryOfEmbarkation == "New Zealand" || TravelEntryList[i].LastCountryOfEmbarkation == "Vietnam")
                    {
                        totalcost += 200;
                    }
                    else if (TravelEntryList[i].LastCountryOfEmbarkation == "Macao SAR")
                    {
                        totalcost += 200 + 20;
                    }
                    else
                    {
                        totalcost += 1000 + 200 + 20;
                    }
                }
            }
            return totalcost;
        }

        public override string ToString()
        {
            return base.ToString() + string.Format("Address: {0,-15}", Address);
        }
    }
}
