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

        }

        public override string ToString()
        {
            return string.Format("Check-in: {0,-30}Check-out: {1,-30}Location: {2,-20}", Checkin, Checkout, Location);
        }
    }
}
