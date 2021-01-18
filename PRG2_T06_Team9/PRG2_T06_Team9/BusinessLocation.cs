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
    class BusinessLocation
    {
        public string BusinessName { get; set; }
        public string BranchCode { get; set; }
        public int MaximumCapacity { get; set; }
        public int VisitorsNow { get; set; }

        public BusinessLocation()
        {

        }

        public BusinessLocation(string businessName, string branchCode, int maximumCapacity, int visitorsNow)
        {
            BusinessName = businessName;
            BranchCode = branchCode;
            MaximumCapacity = maximumCapacity;
            VisitorsNow = visitorsNow;
        }

        public bool IsFull()
        {
            if (VisitorsNow == MaximumCapacity)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
