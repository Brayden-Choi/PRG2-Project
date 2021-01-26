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

        /*public bool IsEligibleForReplacement()
        {

        }*/

        public void ReplaceToken(string serialNo, string collectionLocation)
        {

        }
    }
}
