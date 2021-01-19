//============================================================
// Student Number : S10205942, S10205045
// Student Name : Elsa Lee Ting, Brayden Choi Di Rong
// Module Group : T06
//============================================================

using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PRG2_T06_Team9
{
    class Program
    {
        static void Main(string[] args)
        {
            List<SHNFacility> facilityList = new List<SHNFacility>();
            GetFacilityDetails(facilityList);
            List<Person> personList = new List<Person>();
            InitPersonList(personList, facilityList);
            for (int i = 0; i < facilityList.Count; i++)
            {
                Console.WriteLine(facilityList[i]);
            }
        }

        static void GetFacilityDetails(List<SHNFacility> facilityList)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://covidmonitoringapiprg2.azurewebsites.net");
                Task<HttpResponseMessage> responseTask = client.GetAsync("/facility");
                responseTask.Wait();
                HttpResponseMessage result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    Task<string> readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    string data = readTask.Result;
                    facilityList = JsonConvert.DeserializeObject<List<SHNFacility>>(data);
                }
            }
        }

        static void InitPersonList(List<Person> personList, List<SHNFacility> facilityList)
        {
            string[] personLines = File.ReadAllLines("Person.csv");

            for (int i = 1; i < personLines.Length; i++)
            {
                string[] person = personLines[i].Split(',');

                if (person[0] == "resident")
                {
                    string name = person[1];
                    string addr = person[2];
                    DateTime lastLeft = Convert.ToDateTime(person[3]);
                    string serialNo = person[6];
                    string collectionLocation = person[7];
                    string lastCountry = person[9];
                    string entryMode = person[10];
                    DateTime entryDate = Convert.ToDateTime(person[11]);
                    Resident residentObj = new Resident(name, addr, lastLeft);
                    TravelEntry travelEntryObj = new TravelEntry(lastCountry, entryMode, entryDate);
                    residentObj.AddTravelEntry(travelEntryObj);
                    travelEntryObj.ShnEndDate = Convert.ToDateTime(person[12]);
                    travelEntryObj.IsPaid = Convert.ToBoolean(person[13]);

                    if (person[8] != "")
                    {
                        DateTime tokenExpiry = Convert.ToDateTime(person[8]);
                        residentObj.Token = new TraceTogetherToken(serialNo, collectionLocation, tokenExpiry);
                    }

                    personList.Add(residentObj);
                }

                if (person[0] == "visitor")
                {
                    string name = person[1];
                    string passportNo = person[4];
                    string nationality = person[5];
                    string lastCountry = person[9];
                    string entryMode = person[10];
                    DateTime entryDate = Convert.ToDateTime(person[11]);
                    DateTime endDate = Convert.ToDateTime(person[12]);
                    bool travelPaid = Convert.ToBoolean(person[13]);
                    string facilityName = person[14];
                    Visitor visitorObj = new Visitor(name, passportNo, nationality);
                    TravelEntry travelEntryObj = new TravelEntry(lastCountry, entryMode, entryDate);
                    visitorObj.AddTravelEntry(travelEntryObj);
                    personList.Add(visitorObj);
                }
            }
        }
    }
}
