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
    class Person
    {
        public string Name { get; set; }
        public List<SafeEntry> SafeEntryList { get; set; }
        public List<TravelEntry> TravelEntryList { get; set; }

        public Person()
        {

        }

        public Person(string name)
        {
            Name = name;
            TravelEntryList = new List<TravelEntry>();
            SafeEntryList = new List<SafeEntry>();
        }

        public void AddTravelEntry(TravelEntry travelDetails)
        {
            TravelEntryList.Add(travelDetails);
        }

        public void AddTSafeEntry(SafeEntry safeEntryDetails)
        {
            SafeEntryList.Add(safeEntryDetails);
        }

        public virtual double CalculateSHNCharges()
        {
            return 1.0;
        }

        public override string ToString()
        {
            return string.Format("Name: {0,-14}", Name); 
        }
    }
}
