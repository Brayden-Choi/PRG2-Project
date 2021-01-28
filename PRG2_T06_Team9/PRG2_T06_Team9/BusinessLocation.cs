﻿//============================================================
// Student Number : S10205942, S10205045
// Student Name : Elsa Lee Ting, Brayden Choi Di Rong
// Module Group : T06
//============================================================

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

        public BusinessLocation(string businessName, string branchCode, int maximumCapacity)
        {
            BusinessName = businessName;
            BranchCode = branchCode;
            MaximumCapacity = maximumCapacity;
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
<<<<<<< HEAD
            return string.Format("{0,-20}{1,-10}{2,-7}", BusinessName, BranchCode, MaximumCapacity);
=======
            return string.Format("{0,-20}{1,-8}{2,-8}", BusinessName, BranchCode, MaximumCapacity);
>>>>>>> 08aca775edb08c4ed0c11c0f0526370012cdef14
        }
    }
}
