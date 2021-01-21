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
    class TravelEntry
    {
        public string LastCountryOfEmbarkation { get; set; }
        public string EntryMode { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ShnEndDate { get; set; }
        public SHNFacility ShnStay { get; set; }
        public bool IsPaid { get; set; }

        public TravelEntry()
        {

        }

        public TravelEntry(string lastCountryOfEmbarkation, string entryMode, DateTime entryDate)
        {
            LastCountryOfEmbarkation = lastCountryOfEmbarkation;
            EntryMode = entryMode;
            EntryDate = entryDate;
        }

        public void AssignSHNFacility(SHNFacility shnFacility)
        {
            ShnStay = shnFacility;
        }

        public void CalculateSHNDuration()
        {
            
        }

        public override string ToString()
        {
            return LastCountryOfEmbarkation + EntryMode + EntryDate;
        }
    }
}
