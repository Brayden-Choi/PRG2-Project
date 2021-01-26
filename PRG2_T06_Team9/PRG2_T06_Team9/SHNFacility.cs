//============================================================
// Student Number : S10205942, S10205045
// Student Name : Elsa Lee Ting, Brayden Choi Di Rong
// Module Group : T06
//============================================================

using System;

namespace PRG2_T06_Team9
{
    class SHNFacility
    {
        public string FacilityName { get; set; }
        public int FacilityCapacity { get; set; }
        public int FacilityVacancy { get; set; }
        public double DistFromAirCheckpoint { get; set; }
        public double DistFromSeaCheckpoint { get; set; }
        public double DistFromLandCheckpoint { get; set; }

        public SHNFacility()
        {

        }

        public SHNFacility(string facilityName, int facilityCapacity, double distFromAirCheckpoint, double distFromSeaCheckpoint, double distFromLandCheckpoint)
        {
            FacilityName = facilityName;
            FacilityCapacity = facilityCapacity;
            DistFromAirCheckpoint = distFromAirCheckpoint;
            DistFromSeaCheckpoint = distFromSeaCheckpoint;
            DistFromLandCheckpoint = distFromLandCheckpoint;
        }

        public double CalculateTravelCost(string entryMode, DateTime entryDate)
        {
            double baseFare = 0;
            if (entryMode == "Air")
            {
                baseFare += 50 + DistFromAirCheckpoint * 0.22;
            }
            else if (entryMode == "Land")
            {
                baseFare += 50 + DistFromLandCheckpoint * 0.22;
            }
            else if (entryMode == "Sea")
            {
                baseFare += 50 + DistFromSeaCheckpoint * 0.22;
            }

            //Calculate surcharge
            var morning_start = new TimeSpan(6, 0, 0);
            var morning_end = new TimeSpan(8, 59, 0);
            var evening_start = new TimeSpan(18, 0, 0);
            var evening_end = new TimeSpan(23, 59, 0);
            var midnight_start = new TimeSpan(24, 0, 0);
            var midnight_end = new TimeSpan(5, 59, 0);

            if ((entryDate.TimeOfDay >= morning_start && entryDate.TimeOfDay <= morning_end) || (entryDate.TimeOfDay >= evening_start && entryDate.TimeOfDay <= evening_end))
            {
                baseFare += baseFare * 0.25;
            }
            else if (entryDate.TimeOfDay >= midnight_start && entryDate.TimeOfDay <= midnight_end)
            {
                baseFare += baseFare * 0.50;
            }

            return baseFare;
        }

        public bool IsAvailable()
        {
            bool isAvail = true;
            if (FacilityVacancy == 0)
            {
                isAvail = false;
            }
            return isAvail;
        }

        public override string ToString()
        {
            return string.Format("{0,-25}{1,-25}{2,-25}{3,-30}{4,-30}{5,-30}", FacilityName, FacilityCapacity, FacilityVacancy, DistFromAirCheckpoint, DistFromSeaCheckpoint, DistFromLandCheckpoint);
        }
    }
}
