//============================================================
// Student Number : S10205942, S10205045
// Student Name : Elsa Lee Ting, Brayden Choi Di Rong
// Module Group : T06
//============================================================

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
                if (TravelEntryList[i].IsPaid == false)
                {
                    if (TravelEntryList[i].LastCountryOfEmbarkation == "New Zealand" || TravelEntryList[i].LastCountryOfEmbarkation == "Vietnam" || TravelEntryList[i].LastCountryOfEmbarkation == "Macao SAR")
                    {
                        totalcost += 200 + 80;
                    }
                    else
                    {
                        double transportationCost = TravelEntryList[i].ShnStay.CalculateTravelCost(TravelEntryList[i].EntryMode, TravelEntryList[i].EntryDate);
                        totalcost += transportationCost + 200 + 2000;
                    }
                }
                
            }
            return totalcost;
        }

        public override string ToString()
        {
            return base.ToString() + string.Format("Passport No: {0,-17}Nationality: {1,-15}", PassportNo, Nationality);
        }
    }
}
