//============================================================
// Student Number : S10205942, S10205045
// Student Name : Elsa Lee Ting, Brayden Choi Di Rong
// Module Group : T06
//============================================================

using System;

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

        public SafeEntry(DateTime checkin, BusinessLocation location)
        {
            Checkin = checkin;
            Location = location;
        }

        public void PerformCheckOut()
        {
            Checkout = DateTime.Now;
        }

        public override string ToString()
        {
            return string.Format("Check-in: {0,-15} Check-out: {1,-10} Location: {2,-10}", Checkin, Checkout, Location);
        }
    }
}
