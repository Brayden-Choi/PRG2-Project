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
    class SafeEntry
    {
        public DateTime Checkin { get; set; }
        public DateTime Checkout { get; set; }
        public BusinessLocation Location { get; set; }

        public SafeEntry()
        {

        }

        public SafeEntry(DateTime checkin, DateTime checkout, BusinessLocation location)
        {
            Checkin = checkin;
            Checkout = checkout;
            Location = location;
        }

        public void PerformCheckOut()
        {
            
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
