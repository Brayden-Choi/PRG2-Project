//============================================================
// Student Number : S10205942, S10205045
// Student Name : Elsa Lee Ting, Brayden Choi Di Rong
// Module Group : T06
//============================================================

using System;

namespace PRG2_T06_Team9
{
    class TraceTogetherToken
    {
        public string SerialNo { get; set; }
        public string CollectionLocation { get; set; }
        public DateTime ExpiryDate { get; set; }
        public TraceTogetherToken()
        {

        }

        public TraceTogetherToken(string serialNo, string collectionLocation, DateTime expiryDate)
        {
            SerialNo = serialNo;
            CollectionLocation = collectionLocation;
            ExpiryDate = expiryDate;
        }

        public bool IsEligibleForReplacement()
        {
            DateTime edDateTime = ExpiryDate;
            DateTime botspan = edDateTime.AddMonths(-1);

            if (edDateTime <= DateTime.Now || (DateTime.Now >= botspan && DateTime.Now <= edDateTime))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ReplaceToken(string serialNo, string collectionLocation)
        {
            SerialNo = serialNo;
            CollectionLocation = collectionLocation;
            ExpiryDate = DateTime.Now.AddMonths(6); ;
        }

        public override string ToString()
        {
            return string.Format("Serial No: {0,-15} Collection Location: {1,-15} Expiry Date: {2,-10}", SerialNo, CollectionLocation, ExpiryDate);
        }
    }
}
