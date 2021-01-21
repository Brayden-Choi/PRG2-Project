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

            for (int i = 0; i < personList.Count; i++)
            {
                Console.WriteLine(personList[i].Name, personList[i].TravelEntryList);
            }

            while (true)
            {
                int option = DisplayMenu();
                if (option == 1)
                {

                }

                else if (option == 2)
                {

                }

                else if (option == 3)
                {

                }
           
                else if (option == 4)
                {

                }

                else if (option == 5)
                {

                }

                else if (option == 6)
                {

                }
                else if (option == 7)
                {

                }
           
                else if (option == 8)
                {

                }
           
                else if (option == 9)
                {

                }
            
                else if (option == 10)
                {

                }
            
                else if (option == 11)
                {

                }
               
                else if (option == 12)
                {

                }
                
                else if (option == 13)
                {

                }
               
                else if (option == 0)
                {
                    Console.WriteLine("Goodbye! Stay Safe!");
                    break;
                }

            }
        }

        static int DisplayMenu()
        {
            Console.WriteLine("---------------- M E N U--------------------");
            Console.WriteLine(" ");
            Console.WriteLine("-----------------GENERAL--------------------");
            Console.WriteLine("[1] Load Person and Business Location Data");
            Console.WriteLine("[2] Load SHN Facility Data");
            Console.WriteLine("[3] List All Visitors");
            Console.WriteLine("[4] List Person Details");
            Console.WriteLine(" ");
            Console.WriteLine("--------SafeEntry/TraceTogether Token-------");
            Console.WriteLine("[5] Assign/Replace TraceTogether Token");
            Console.WriteLine("[6] List All Business Locations");
            Console.WriteLine("[7] Edit Business Location Capacity");
            Console.WriteLine("[8] SafeEntry Check-in");
            Console.WriteLine("[9] SafeEntry Check-out");
            Console.WriteLine(" ");
            Console.WriteLine("-----------------TravelEntry----------------");
            Console.WriteLine("[10] List All SHN Facilities");
            Console.WriteLine("[11] Create Visitor");
            Console.WriteLine("[12] Create TravelEntry Record");
            Console.WriteLine("[13] Calculate SHN Charges");
            Console.WriteLine("---------------------------------------------");
            Console.Write("Enter your option: ");
            int option = Convert.ToInt32(Console.ReadLine());
            return option;
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
                    //Add a new Resident
                    string name = person[1];
                    string addr = person[2];
                    DateTime lastLeft = Convert.ToDateTime(person[3]);
                    Resident residentObj = new Resident(name, addr, lastLeft);


                    //If the resident has a token, add the token to the Resident Object
                    if (person[6] != "")
                    {
                        string serialNo = person[6];
                        string collectionLocation = person[7];
                        DateTime tokenExpiry = Convert.ToDateTime(person[8]);
                        residentObj.Token = new TraceTogetherToken(serialNo, collectionLocation, tokenExpiry);
                    }


                    //If there are travel details for the resident, add it to the Resident Object
                    if (person[9] != "")
                    {
                        string lastCountry = person[9];
                        string entryMode = person[10];
                        DateTime entryDate = Convert.ToDateTime(person[11]);
                        TravelEntry travelEntryObj = new TravelEntry(lastCountry, entryMode, entryDate);
                        travelEntryObj.ShnEndDate = Convert.ToDateTime(person[12]);
                        travelEntryObj.IsPaid = Convert.ToBoolean(person[13]);
                        residentObj.AddTravelEntry(travelEntryObj);


                        //If the resident is assigned a facility, add the facility to their travel entry
                        if (person[14] != "")
                        {
                            string facilityName = person[14];
                            SHNFacility facility = SearchFacility(facilityList, facilityName);
                            travelEntryObj.ShnStay = facility;
                        }
                    }

                    //Add the Resident Object to the Person List
                    personList.Add(residentObj);
                }

                if (person[0] == "visitor")
                {
                    //Add a new Visitor
                    string name = person[1];
                    string passportNo = person[4];
                    string nationality = person[5];
                    Visitor visitorObj = new Visitor(name, passportNo, nationality);


                    //If there are travel details for the visitor, add it to the Visitor Object
                    if (person[9] != "")
                    {
                        string lastCountry = person[9];
                        string entryMode = person[10];
                        DateTime entryDate = Convert.ToDateTime(person[11]);
                        TravelEntry travelEntryObj = new TravelEntry(lastCountry, entryMode, entryDate);
                        travelEntryObj.ShnEndDate = Convert.ToDateTime(person[12]);
                        travelEntryObj.IsPaid = Convert.ToBoolean(person[13]);
                        visitorObj.AddTravelEntry(travelEntryObj);


                        //If the visitor is assigned a facility, add the facility to their travel entry
                        if (person[14] != "")
                        {
                            string facilityName = person[14];
                            SHNFacility facility = SearchFacility(facilityList, facilityName);
                            travelEntryObj.ShnStay = facility;
                        }
                    }
                }
            }
        }

        static void CalSHNCharges(List<Person> personList)
        {
            foreach (Person p in personList)
            {
                if (p is Visitor)
                {
                    p.CalculateSHNCharges();
                }

                else
                {
                    p.CalculateSHNCharges();
                }
            }
        }

        static SHNFacility SearchFacility(List<SHNFacility> fList, string f)
        {
            for (int i = 0; i < fList.Count; i++)
            {
                if (f == fList[i].FacilityName)
                {
                    return fList[i];
                }
            }
            return null;
        }
    }
}
