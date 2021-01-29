//============================================================
// Student Number : S10205942, S10205045
// Student Name : Elsa Lee Ting, Brayden Choi Di Rong
// Module Group : T06
//============================================================

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;

namespace PRG2_T06_Team9
{
    class Program
    {
        static void Main(string[] args)
        {
            List<SHNFacility> fList = new List<SHNFacility>();
            List<BusinessLocation> bList = new List<BusinessLocation>();
            List<string> tList = new List<string>();
            List<SafeEntry> uncheckedList = new List<SafeEntry>();
            List<SHNFacility> facilityList = GetFacilityDetails(fList);
            for (int i = 0; i < facilityList.Count; i++)
            {
                facilityList[i].FacilityVacancy = facilityList[i].FacilityCapacity;
            }
            List<Person> personList = new List<Person>();
            InitPersonList(personList, facilityList);
            ListBL(bList);
            InitTlist(tList);

            while (true)
            {
                int option = DisplayMenu();
                if (option == 1)
                {
                    DisplayAllVisitors(personList);
                    Console.WriteLine();
                }

                else if (option == 2)
                {
                    while (true)
                    {
                        Console.Write("Enter your name: ");
                        string personName = Console.ReadLine();
                        Person searchedPerson = SearchPerson(personList, personName);
                        if (searchedPerson != null)
                        {
                            Console.WriteLine();
                            Console.WriteLine("------------------------------------------- Person Details --------------------------------------------");
                            Console.WriteLine(searchedPerson.ToString());
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine("---------------------------------------- Travel Entry Details -----------------------------------------");
                            for (int i = 0; i < searchedPerson.TravelEntryList.Count; i++)
                            {
                                if (searchedPerson.TravelEntryList is null)
                                {
                                    Console.WriteLine("There is no current record of your Travel Entry Details found.");
                                }
                                else
                                {
                                    Console.WriteLine(searchedPerson.TravelEntryList[i].ToString());
                                }
                            }
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine("----------------------------------------- Safe Entry Details ------------------------------------------");
                            for (int i = 0; i < searchedPerson.SafeEntryList.Count; i++)
                            {
                                if (searchedPerson.SafeEntryList is null)
                                {
                                    Console.WriteLine("There is no current record of your Safe Entry Details found.");
                                }

                                else
                                {
                                    Console.WriteLine(searchedPerson.SafeEntryList[i].ToString());
                                }
                            }
                            Console.WriteLine();
                            Console.WriteLine();
                            if (searchedPerson is Resident)
                            {
                                Resident r = (Resident)searchedPerson;
                                Console.WriteLine("-------------------------------------------- Token Details --------------------------------------------");
                                if (r.Token is null)
                                {
                                    Console.WriteLine("There is no current record of your TraceTogether Token found.");
                                }
                                else
                                {
                                    Console.WriteLine("Serial No: {0,-15}Expiry Date: {1,-30}Collection Location: {2,-15}", r.Token.SerialNo, r.Token.ExpiryDate, r.Token.CollectionLocation);
                                }
                            }
                            Console.WriteLine();
                            Console.WriteLine();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("This person does not exist in our database. Please try another name.");
                            Console.WriteLine();
                        }
                    }
                }

                else if (option == 3)
                {
                    while (true)
                    {
                        Console.Write("Please enter your name: ");
                        string personName = Console.ReadLine();
                        Person searchedPerson = SearchPerson(personList, personName);
                        if (searchedPerson != null)
                        {
                            if (searchedPerson is Resident)
                            {
                                Resident r = (Resident) searchedPerson;
                                if (r.Token == null)
                                {
                                    Console.Write("Please enter a new Serial Number (e.g: TXXXX): ");
                                    string sn = Console.ReadLine();
                                    for (int i = 0; i < tList.Count; i++)
                                    {
                                        Console.WriteLine(tList[i]);
                                    }

                                    while (true)
                                    {
                                        Console.Write("Please select a collection location: ");
                                        string location = Console.ReadLine();
                                        String searchedCollection = SearchCollection(tList, location);
                                        if (searchedCollection != null) 
                                        {
                                            DateTime ed = DateTime.Now.AddMonths(6);
                                            TraceTogetherToken token = new TraceTogetherToken(sn, location, ed);
                                            r.Token = token;
                                            Console.WriteLine("You have successfully claimed your TraceTogether Token.");
                                            break;
                                        }
                                        Console.WriteLine("Invalid Collection Point entered. Please try again.");
                                    }
                                }

                                else
                                {
                                    DateTime edDateTime = r.Token.ExpiryDate;
                                    DateTime botspan = edDateTime.AddMonths(-1);
                                    if (botspan >= DateTime.Now || DateTime.Now <= edDateTime)
                                    {
                                        Console.WriteLine("Your token's expiry date is coming soon. Please replace it now.");
                                        for (int i = 0; i < tList.Count; i++)
                                        {
                                            Console.WriteLine(tList[i]);
                                        }

                                        while (true)
                                        {
                                            Console.Write("Please select a collection location: ");
                                            string location = Console.ReadLine();
                                            String searchedCollection = SearchCollection(tList, location);
                                            if (searchedCollection != null)
                                            {
                                                DateTime ed = DateTime.Now.AddMonths(6);
                                                TraceTogetherToken token = new TraceTogetherToken(r.Token.SerialNo, location, ed);
                                                r.Token = token;
                                                Console.WriteLine("You have successfully replaced your TraceTogether Token.");
                                                break;
                                            }
                                            Console.WriteLine("Invalid Collection Point entered. Please try again.");
                                        }
                                    }

                                    else if (edDateTime < DateTime.Now)
                                    {
                                        Console.WriteLine("Your TraceTogether Token has expired already. Please replace it now.");
                                        for (int i = 0; i < tList.Count; i++)
                                        {
                                            Console.WriteLine(tList[i]);
                                        }

                                        while (true)
                                        {
                                            Console.Write("Please select a collection location: ");
                                            string location = Console.ReadLine();
                                            String searchedCollection = SearchCollection(tList, location);
                                            if (searchedCollection != null)
                                            {
                                                DateTime ed = DateTime.Now.AddMonths(6);
                                                TraceTogetherToken token = new TraceTogetherToken(r.Token.SerialNo, location, ed);
                                                r.Token = token;
                                                Console.WriteLine("You have successfully replaced your TraceTogether Token.");
                                                break;
                                            }
                                            Console.WriteLine("Invalid Collection Point entered. Please try again.");
                                        }
                                    }
                                    else if (botspan >= edDateTime)
                                    {
                                        Console.WriteLine("Your token has not expired yet and is not up for replacement.");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Sorry, you are not eligible for a TraceTogether Token as you are not a resident.");
                            }
                            break;
                        }
                        Console.WriteLine("This person does not exist in our database. Please try another name.");
                    }
                }
                
                else if (option == 4)
                {
                    Console.WriteLine("------------------------ Business Locations ------------------------");
                    Console.WriteLine();
                    Console.WriteLine("{0,-30}{1,-20}{2,-20}", "Business Name", "Branch Code", "Maximum Capacity");
                    for (int i = 0; i < bList.Count; i++)
                    {
                        Console.WriteLine(bList[i].ToString());
                    }
                    Console.WriteLine();
                }
                
                else if (option == 5)
                {
                    while (true)
                    {   Console.Write("Please enter the name of the business location: ");
                        string location = Console.ReadLine();
                        BusinessLocation searchedLocation = SearchBusiness(bList, location);
                        if (searchedLocation != null)
                        {
                            Console.Write("Please edit the maximum capacity: ");
                            int cap = Convert.ToInt32(Console.ReadLine());
                            searchedLocation.MaximumCapacity = cap;
                            Console.WriteLine("The new maximum capacity for {0} is {1}.", searchedLocation.BusinessName, cap);
                            break;
                        }
                        Console.WriteLine("Business location not found. Please try again.");
                    }   
                }

                else if (option == 6)
                {
                    while (true)
                    {
                        Console.Write("Please enter your name: ");
                        string personName = Console.ReadLine();
                        Person searchedPerson = SearchPerson(personList, personName);
                        if (searchedPerson != null)
                        {
                            while (true)
                            {
                                ListBL(bList);
                                Console.Write("Select a business location to check into: ");
                                string location = Console.ReadLine();
                                BusinessLocation searchedLocation = SearchBusiness(bList, location);
                                if (searchedLocation != null)
                                {
                                    while (true)
                                    {
                                        if (searchedLocation.IsFull() == false)
                                        {
                                            SafeEntry SE = new SafeEntry(DateTime.Now, searchedLocation);
                                            searchedLocation.VisitorsNow += 1;
                                            searchedPerson.AddSafeEntry(SE);
                                            Console.WriteLine("You have successfully checked into {0}.",
                                                searchedLocation.BusinessName);
                                            break;
                                        }
                                        Console.WriteLine("This business location is full. Please select another one.");
                                    }
                                    break;
                                }
                                Console.WriteLine("Invalid business location entered. Please try again.");
                            }
                            break;
                        }
                        Console.WriteLine("This person does not exist in our database. Please try another name.");
                        
                    }
                }

                else if (option == 7)
                {
                    while (true)
                    {
                        Console.Write("Please enter your name: ");
                        string personName = Console.ReadLine();
                        Person searchedPerson = SearchPerson(personList, personName);
                        if (searchedPerson != null)
                        {
                            uncheckedList.Clear(); 
                            for (int i = 0; i < searchedPerson.SafeEntryList.Count; i++)
                            {
                                DateTime empty = new DateTime(0001, 1, 1, 00, 00, 00);
                                if (searchedPerson.SafeEntryList[i].Checkout == empty)
                                {
                                    uncheckedList.Add(searchedPerson.SafeEntryList[i]);
                                }
                            }
                            if (uncheckedList.Count == 0)
                            {
                                Console.WriteLine("You have checked out from all the SafeEntry Records.");
                            }
                            else
                            {
                                while (true)
                                {
                                    for (int x = 0; x < uncheckedList.Count; x++)
                                    {
                                        Console.WriteLine("Record {0}: {1}", x, uncheckedList[x]);
                                    }
                                    Console.Write("Please select the record to check out from: ");
                                    int record = Convert.ToInt32(Console.ReadLine());
                                    if (0 < record || record < uncheckedList.Count)
                                    {
                                        uncheckedList[record].Checkout = DateTime.Now;
                                        Console.WriteLine("You have successfully checked out from {0}.", uncheckedList[record].Location.BusinessName);
                                        uncheckedList.Remove(uncheckedList[record]);
                                        break;
                                    }
                                    Console.WriteLine("Record not found. Please try again.");
                                }
                            }
                            break;
                        }
                        Console.WriteLine("This person does not exist in our database. Please try again.");
                    }
                }

                else if (option == 8)
                {
                    Console.WriteLine("{0,-25}{1,-25}{2,-25}{3,-30}{4,-30}{5,-30}", "Facility Name", "Facility Capacity", "Facility Vacancy", "Dist. From Air Checkpoint", "Dist. From Sea Checkpoint", "Dist. From Land Checkpoint");
                    for (int i = 0; i < facilityList.Count; i++)
                    {
                        Console.WriteLine(facilityList[i].ToString());
                    }
                    Console.WriteLine();
                }

                else if (option == 9)
                {
                    Person newVisitor = CreateVisitor();
                    personList.Add(newVisitor);
                    Console.WriteLine("Visitor has been added!");
                    Console.WriteLine();
                }

                else if (option == 10)
                {
                    bool runloop = true;
                    while (runloop)
                    {
                        Console.Write("Enter your name: ");
                        string personName = Console.ReadLine();
                        Person searchedPerson = SearchPerson(personList, personName);
                        if (searchedPerson != null)
                        {
                            TravelEntry newTravelEntry = CreateTravelEntry();
                            searchedPerson.AddTravelEntry(newTravelEntry);
                            newTravelEntry.CalculateSHNDuration();
                            

                            while (runloop)
                            {
                                Console.Write("Would you like to add a SHN Facility? (Y/N): ");
                                string reply = Console.ReadLine().ToLower();
                                if (reply == "y")
                                {
                                    while (true)
                                    {
                                        string facilityName = AssignFacility();
                                        SHNFacility facility = SearchFacility(facilityList, facilityName);
                                        bool isAvail = facility.IsAvailable();
                                        if (isAvail)
                                        {
                                            newTravelEntry.AssignSHNFacility(facility);
                                            facility.FacilityVacancy -= 1;
                                            runloop = false;
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Facility has no vacant slots. Please choose another one.");
                                            Console.WriteLine();
                                        }
                                    }
                                }
                                else if (reply == "n")
                                {
                                    Console.WriteLine("Travel Entry Added.");
                                    runloop = false;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid option. Please try again.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("This person does not exist in our database. Please try another name.");
                        }
                    }
                    Console.WriteLine();
                }

                else if (option == 11)
                {
                    Console.Write("Enter your name: ");
                    string personName = Console.ReadLine();
                    Person searchedPerson = SearchPerson(personList, personName);
                    double totalcost = 0;
                    List<TravelEntry> unpaidTravelEntry = new List<TravelEntry>();
                    if (searchedPerson != null)
                    {
                        for (int i = 0; i < searchedPerson.TravelEntryList.Count; i++)
                        {
                            if (searchedPerson.TravelEntryList[i].IsPaid == false && searchedPerson.TravelEntryList[i].ShnEndDate < DateTime.Now)
                            {
                                unpaidTravelEntry.Add(searchedPerson.TravelEntryList[i]);
                                if (searchedPerson is Resident)
                                {
                                    Resident r = (Resident)searchedPerson;
                                    totalcost += r.CalculateSHNCharges();
                                }
                                else if (searchedPerson is Visitor)
                                {
                                    Visitor v = (Visitor)searchedPerson;
                                    totalcost += v.CalculateSHNCharges();
                                }
                                totalcost = totalcost * 1.07;
                            }
                            else if (searchedPerson.TravelEntryList[i].IsPaid == true)
                            {
                                totalcost = 0;
                                Console.WriteLine("No SHN charges.");
                            }
                        }


                        for (int i = 0; i < unpaidTravelEntry.Count; i++)
                        {
                            Console.WriteLine("The total SHN charges for {0} is ${1:0.00}.", searchedPerson.Name, totalcost);
                            Console.Write("Would you like to proceed with payment? (Y/N): ");
                            string reply = Console.ReadLine().ToLower();
                            if (reply == "y")
                            {
                                Console.WriteLine("Payment completed.");
                                searchedPerson.TravelEntryList[i].IsPaid = true;
                                break;
                            }
                            if (reply == "n")
                            {
                                Console.WriteLine("Transaction cancelled.");
                                Console.WriteLine();
                                searchedPerson.TravelEntryList[i].IsPaid = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("This person does not exist in our database. Please try another name.");
                    }
                }

                else if (option == 0)
                {
                    Console.WriteLine("Goodbye! Stay Safe!");
                    break;
                }
            }
        }

        /*----------------GENERAL FUNCTIONS-------------------*/

        static int DisplayMenu()
        {
            int option = 0;
            while (true)
            {
                try
                {
                    Console.WriteLine("|---------------- COVID-19 MONITORING SYSTEM --------------------|");
                    Console.WriteLine();
                    Console.WriteLine("----------------- General --------------------");
                    Console.WriteLine("[1] List All Visitors");
                    Console.WriteLine("[2] List Person Details");
                    Console.WriteLine(" ");
                    Console.WriteLine("-------- SafeEntry/TraceTogether Token -------");
                    Console.WriteLine("[3] Assign/Replace TraceTogether Token");
                    Console.WriteLine("[4] List All Business Locations");
                    Console.WriteLine("[5] Edit Business Location Capacity");
                    Console.WriteLine("[6] SafeEntry Check-in");
                    Console.WriteLine("[7] SafeEntry Check-out");
                    Console.WriteLine();
                    Console.WriteLine("----------------- TravelEntry ----------------");
                    Console.WriteLine("[8] List All SHN Facilities");
                    Console.WriteLine("[9] Create Visitor");
                    Console.WriteLine("[10] Create TravelEntry Record");
                    Console.WriteLine("[11] Calculate SHN Charges");
                    Console.WriteLine();
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("[0] Exit");
                    Console.Write("Enter your option: ");
                    option = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();
                    break;
                }

                catch (FormatException e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    Console.WriteLine("Please try again.");
                }
            }
            return option;
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
                            travelEntryObj.AssignSHNFacility(facility);
                            travelEntryObj.ShnStay.FacilityVacancy -= 1;
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
                            travelEntryObj.AssignSHNFacility(facility);
                            travelEntryObj.ShnStay.FacilityVacancy -= 1;
                        }
                    }

                    //Add the Visitor Object to the Person List
                    personList.Add(visitorObj);
                }
            }
        }

        static void DisplayAllVisitors(List<Person> pList)
        {
            Console.WriteLine("-------------------------------- Visitors --------------------------------");
            Console.WriteLine();
            foreach (Person p in pList)
            {
                if (p is Visitor)
                {
                    Visitor v = (Visitor)p;
                    Console.WriteLine(v.ToString());
                }
            }
        }

        static Person SearchPerson(List<Person> pList, string p)
        {
            for (int i = 0; i < pList.Count; i++)
            {
                if (p == pList[i].Name)
                {
                    return pList[i];
                }
            }
            return null;
        }


        /*----------------TRAVEL ENTRY FUNCTIONS-------------------*/
        static List<SHNFacility> GetFacilityDetails(List<SHNFacility> facilityList)
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
            return facilityList;
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

        static Person CreateVisitor()
        {
            Console.Write("Enter your name: ");
            string visitorName = Console.ReadLine();

            Console.Write("Enter your passport number: ");
            string passsportNo = Console.ReadLine();

            Console.Write("Enter your nationality: ");
            string nationality = Console.ReadLine();

            Person newVisitor = new Visitor(visitorName, passsportNo, nationality);
            return newVisitor;
        }

        static TravelEntry CreateTravelEntry()
        {
            while (true)
            {
                Console.Write("Enter your last country of embarkation: ");
                string lastCountry = Console.ReadLine();
                Console.WriteLine();
                string entryMode = AssignEntryMode();
                Console.Write("Enter your entry date(dd/mm/yyyy hh:mm:ss): ");
                DateTime entryDate = Convert.ToDateTime(Console.ReadLine());

                TravelEntry newTravelEntry = new TravelEntry(lastCountry, entryMode, entryDate);
                return newTravelEntry;
                
            }
        }

/*        static int DisplayEntryMode()
        {
            int option = 0;
            while (true)
            {
                try
                {
                    Console.WriteLine("[1] Air");
                    Console.WriteLine("[2] Land");
                    Console.WriteLine("[3] Sea");

                    Console.Write("Select your entry mode: ");
                    option = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();
                    break;
                }

                catch (FormatException e)
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    Console.WriteLine();
                }
            }
            return option;
        }*/

        static string AssignEntryMode()
        {
            Console.WriteLine("[1] Air");
            Console.WriteLine("[2] Land");
            Console.WriteLine("[3] Sea");

            while (true)
            {
                try
                {
                    Console.Write("Select your entry mode: ");
                    int option = Convert.ToInt32(Console.ReadLine());

                    if (option == 1)
                    {
                        return "Air";
                    }
                    else if (option == 2)
                    {
                        return "Land";
                    }
                    else if (option == 3)
                    {
                        return "Sea";
                    }
                    else
                    {
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.WriteLine();
                    }
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.WriteLine();
                }
            }
        }

        static string AssignFacility()
        {
            Console.WriteLine("------------ Facilities ---------------");
            Console.WriteLine("[1] A'Resort");
            Console.WriteLine("[2] Yozel");
            Console.WriteLine("[3] Mandarin Orchid");
            Console.WriteLine("[4] Small Hostel");

            while (true)
            {
                try
                {
                    Console.Write("Select a facility: ");
                    int option = Convert.ToInt32(Console.ReadLine());

                    if (option == 1)
                    {
                        return "A'Resort";
                    }
                    else if (option == 2)
                    {
                        return "Yozel";
                    }
                    else if (option == 3)
                    {
                        return "Mandarin Orchid";
                    }
                    else if (option == 4)
                    {
                        return "Small Hostel";
                    }
                    else
                    {
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.WriteLine();
                    }
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.WriteLine();
                }
            }
        }
        /*----------------SAFE ENTRY FUNCTIONS-------------------*/

        static void ListBL(List<BusinessLocation> bList)
        {
            string[] businessLines = File.ReadAllLines("BusinessLocation.csv");
            for (int i = 1; i < businessLines.Length; i++)
            {
                string[] location = businessLines[i].Split(',');
                BusinessLocation bL = new BusinessLocation(location[0], location[1], Convert.ToInt32(location[2]));
                bList.Add(bL);
            }
        }

        static void InitTlist(List<String> tList)
        {
            tList.Add("Canberra CC");
            tList.Add("Bukit CC");
            tList.Add("Clementi CC"); 
            tList.Add("Eunos CC");
            tList.Add("Dover CC");
            tList.Add("Raffles CC");
            tList.Add("Woodlands CC");
            tList.Add("Yishun CC");
        }

        static BusinessLocation SearchBusiness(List<BusinessLocation> bList, string b)
        {
            for (int i = 0; i < bList.Count; i++)
            {
                if (b == bList[i].BusinessName)
                {
                    return bList[i];
                }
            }
            return null;
        }

        static  String SearchCollection(List<String> tList, string b)
        {
            for (int i = 0; i < tList.Count; i++)
            {
                if (b == tList[i])
                {
                    return tList[i];
                }
            }
            return null;
        }
        
    }
}
