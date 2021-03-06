//============================================================
// Student Number : S10205942, S10205045
// Student Name : Elsa Lee Ting, Brayden Choi Di Rong
// Module Group : T06
//============================================================

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

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
            List<Person> covidPList = new List<Person>();


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
                int option = DisplayMenu(); //Display the main menu

                //List all visitors
                if (option == 1)
                {
                    DisplayAllVisitors(personList);
                    Console.WriteLine();
                }

                //List person details
                else if (option == 2)
                {
                    while (true)
                    {
                        Console.Write("Enter your name: ");
                        string personName = Console.ReadLine();
                        Person searchedPerson = SearchPerson(personList, personName); //Search for person
                        if (searchedPerson != null)
                        {
                            Console.WriteLine();
                            Console.WriteLine("------------------------------------------- Person Details --------------------------------------------");
                            Console.WriteLine(searchedPerson.ToString());
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine("---------------------------------------- Travel Entry Details -----------------------------------------");
                            if (searchedPerson.TravelEntryList.Count == 0)
                            {
                                Console.WriteLine("There is no current record of your Travel Entry Details found.");
                            }
                            else
                            {
                                for (int i = 0; i < searchedPerson.TravelEntryList.Count; i++)
                                {
                                    Console.WriteLine(searchedPerson.TravelEntryList[i].ToString());
                                }
                            }
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine("----------------------------------------- Safe Entry Details ------------------------------------------");
                            if (searchedPerson.SafeEntryList.Count == 0)
                            {
                                Console.WriteLine("There is no current record of your Safe Entry Details found.");
                            }
                            else
                            {
                                for (int i = 0; i < searchedPerson.SafeEntryList.Count; i++)
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
                                    Console.WriteLine(r.Token.ToString());
                                }
                            }
                            Console.WriteLine();
                            Console.WriteLine();
                            break;
                        }
                        //Input validation for name input
                        else
                        {
                            Console.WriteLine("This person does not exist in our database. Please try another name.\nInput is case and space sensitive.");
                            Console.WriteLine();
                        }
                    }
                }

                //Assign and replace trace together token
                else if (option == 3)
                {
                    while (true)
                    {
                        Console.Write("Please enter your name: ");
                        string personName = Console.ReadLine();
                        Person searchedPerson = SearchPerson(personList, personName); //Searching for person
                        if (searchedPerson != null)
                        {
                            if (searchedPerson is Resident) //Checking if the person is a resident
                            {
                                Resident r = (Resident)searchedPerson;
                                if (r.Token == null)
                                {
                                    Console.Write("Please enter a new Serial Number (e.g: TXXXX): "); //User inputs a new S/N
                                    string sn = Console.ReadLine();
                                    for (int i = 0; i < tList.Count; i++)
                                    {
                                        Console.WriteLine(tList[i]); //Prints out the available collection locations
                                    }

                                    while (true)
                                    {
                                        Console.Write("Please select a collection location: ");
                                        string location = Console.ReadLine();
                                        String searchedCollection = SearchCollection(tList, location); //Searching for available collection location
                                        if (searchedCollection != null)
                                        {
                                            DateTime ed = DateTime.Now.AddMonths(6); //Add 6mths to current time for new expiration date
                                            TraceTogetherToken token = new TraceTogetherToken(sn, location, ed); //Set the token attributes
                                            r.Token = token; //Adds the new TraceTogetherToken object into the current resident
                                            Console.WriteLine("You have successfully claimed your TraceTogether Token.");
                                            break;
                                        }
                                        Console.WriteLine("Invalid Collection Point entered. Please try again.");
                                    }
                                }

                                else
                                {
                                    bool IsEligibleForReplacement = r.Token.IsEligibleForReplacement();
                                    if (IsEligibleForReplacement == true) //Checking for eligibility for replacement
                                    { 
                                        Console.WriteLine("Your TraceTogether Token is about to or has expired already. Please replace it now.");
                                        for (int i = 0; i < tList.Count; i++)
                                        {
                                            Console.WriteLine(tList[i]); //Prints out the available collection location
                                        }
                                        while (true)
                                        {
                                            Console.Write("Please select a collection location: ");
                                            string location = Console.ReadLine();
                                            String searchedCollection = SearchCollection(tList, location); //Search for collection location
                                            if (searchedCollection != null)
                                            {
                                                r.Token.ReplaceToken(r.Token.SerialNo, location); //Calls the method and sets the Token attributes
                                                Console.WriteLine("You have successfully replaced your TraceTogether Token.");
                                                break;
                                            }
                                            Console.WriteLine("Invalid Collection Point entered. Please try again.");
                                        }
                                    }

                                    else if (IsEligibleForReplacement == false) //Checking for eligibility for replacement
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
                        Console.WriteLine("This person does not exist in our database. Please try another name.\nInput is case and space sensitive.");
                        Console.WriteLine();
                    }
                }

                //List all business locations
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

                //Edit business location capacity
                else if (option == 5)
                {
                    while (true)
                    {
                        Console.Write("Please enter the name of the business location: ");
                        string location = Console.ReadLine();
                        BusinessLocation searchedLocation = SearchBusiness(bList, location); //Searching for business location
                        if (searchedLocation != null)
                        {
                            Console.Write("Please edit the maximum capacity: ");
                            int cap = Convert.ToInt32(Console.ReadLine()); //Setting user input as the new max capacity
                            searchedLocation.MaximumCapacity = cap;
                            Console.WriteLine("The new maximum capacity for {0} is {1}.", searchedLocation.BusinessName, cap);
                            break;
                        }
                        Console.WriteLine("Business location not found. Please try again.");
                    }
                }

                //SafeEntry check-in
                else if (option == 6)
                {
                    while (true)
                    {
                        Console.Write("Please enter your name: ");
                        string personName = Console.ReadLine();
                        Person searchedPerson = SearchPerson(personList, personName); //Searching for person
                        if (searchedPerson != null)
                        {
                            while (true)
                            {
                                ListBL(bList);
                                Console.Write("Select a business location to check into: ");
                                string location = Console.ReadLine();
                                BusinessLocation searchedLocation = SearchBusiness(bList, location); //Searching for business location
                                if (searchedLocation != null)
                                {
                                    while (true)
                                    {
                                        if (searchedLocation.IsFull() == false) //Checking if the business location is full or not
                                        {
                                            SafeEntry SE = new SafeEntry(DateTime.Now, searchedLocation);
                                            searchedLocation.VisitorsNow += 1;//Add a visitor to Visitor Now
                                            searchedPerson.AddSafeEntry(SE);
                                            Console.WriteLine("You have successfully checked into {0}.", searchedLocation.BusinessName);
                                            break;
                                        }
                                        Console.WriteLine("This business location is full. Please select another one.");
                                        break;
                                    }
                                    break;
                                }
                                Console.WriteLine("Invalid business location entered. Please try again.");
                            }
                            break;
                        }
                        Console.WriteLine("This person does not exist in our database. Please try another name.\nInput is case and space sensitive.");
                        Console.WriteLine();
                    }
                }

                //SafeEntry check-out
                else if (option == 7)
                {
                    while (true)
                    {
                        Console.Write("Please enter your name: ");
                        string personName = Console.ReadLine();
                        Person searchedPerson = SearchPerson(personList, personName); //Searching for provided business locations
                        if (searchedPerson != null)
                        {
                            uncheckedList.Clear(); //To reset the list to avoid duplication of safeEntry objects
                            for (int i = 0; i < searchedPerson.SafeEntryList.Count; i++)
                            {
                                DateTime empty = new DateTime(0001, 1, 1, 00, 00, 00); //This is the default datetime for checkout
                                if (searchedPerson.SafeEntryList[i].Checkout == empty) //Checking for no checkout
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
                                        Console.WriteLine("Record {0}: {1}", x, uncheckedList[x]);//Prints out the list containing all the unchecked records
                                    }
                                    Console.Write("Please select the record to check out from: ");
                                    int record = Convert.ToInt32(Console.ReadLine());
                                    if (0 < record || record < uncheckedList.Count)//Check for the count of the list
                                    {
                                        uncheckedList[record].PerformCheckOut(); //Sets the checkout datetime to the current datetime
                                        Console.WriteLine("You have successfully checked out from {0}.", uncheckedList[record].Location.BusinessName);
                                        uncheckedList[record].Location.VisitorsNow -= 1;
                                        break;
                                    }
                                    Console.WriteLine("Record not found. Please try again.");
                                }
                            }
                            break;
                        }
                        Console.WriteLine("This person does not exist in our database. Please try again.\nInput is case and space sensitive.");
                        Console.WriteLine();
                    }
                }

                //List all SHN Facilities
                else if (option == 8)
                {
                    Console.WriteLine("{0,-25}{1,-25}{2,-25}{3,-30}{4,-30}{5,-30}", "Facility Name", "Facility Capacity", "Facility Vacancy", "Dist. From Air Checkpoint", "Dist. From Sea Checkpoint", "Dist. From Land Checkpoint");
                    for (int i = 0; i < facilityList.Count; i++)
                    {
                        Console.WriteLine(facilityList[i].ToString());
                    }
                    Console.WriteLine();
                }

                //Create Visitor
                else if (option == 9)
                {
                    Person newVisitor = CreateVisitor(); //Call method to create a visitor
                    personList.Add(newVisitor);
                    Console.WriteLine("Visitor has been added!");
                    Console.WriteLine();
                }

                //Create TravelEntry Record
                else if (option == 10)
                {
                    bool runloop = true; //for main loop
                    while (runloop)
                    {
                        Console.Write("Enter your name: ");
                        string personName = Console.ReadLine();
                        Person searchedPerson = SearchPerson(personList, personName); //Search for the person
                        if (searchedPerson != null)
                        {
                            TravelEntry newTravelEntry = CreateTravelEntry(); //Create a new travel entry
                            searchedPerson.AddTravelEntry(newTravelEntry); //Add travelentry to the person
                            newTravelEntry.IsPaid = false;
                            newTravelEntry.CalculateSHNDuration(); //Call method to calculate SHN duration


                            while (runloop)
                            {
                                if (newTravelEntry.LastCountryOfEmbarkation == "New Zealand" || newTravelEntry.LastCountryOfEmbarkation == "Vietnam" || newTravelEntry.LastCountryOfEmbarkation == "Macao SAR") //For these countries, a facility is not needed
                                {
                                    Console.WriteLine("Travel Entry added.");
                                    runloop = false;
                                    break;
                                }
                                else //Facility is needed if the person has arrived from any other country
                                {
                                    while (true)
                                    {
                                        string facilityName = AssignFacility(); //Call method to assign a facility
                                        SHNFacility facility = SearchFacility(facilityList, facilityName);
                                        bool isAvail = facility.IsAvailable();
                                        if (isAvail) //Check if the facility is available
                                        {
                                            Console.WriteLine("Travel Entry added.");
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
                            }
                        }
                        else
                        {
                            Console.WriteLine("This person does not exist in our database. Please try another name.\nInput is case and space sensitive.");
                            Console.WriteLine();
                        }
                    }
                    Console.WriteLine();
                }

                //Calculate SHN Charges
                else if (option == 11)
                {
                    bool runloop = true;
                    while (runloop)
                    {
                        Console.Write("Enter your name: ");
                        string personName = Console.ReadLine();
                        Person searchedPerson = SearchPerson(personList, personName); //Search for the person
                        List<TravelEntry> unpaidTravelEntry = new List<TravelEntry>(); //List to contain the data of people that have unpaid travel entries
                        double totalcost = 0;
                        if (searchedPerson != null)
                        {
                            while (runloop)
                            {
                                for (int i = 0; i < searchedPerson.TravelEntryList.Count; i++)
                                {
                                    if (searchedPerson.TravelEntryList[i].IsPaid == false) //Check if the person has paid or not
                                    {
                                        totalcost = 0;
                                        unpaidTravelEntry.Add(searchedPerson.TravelEntryList[i]);
                                        if (searchedPerson is Resident) //Calculate for Residents
                                        {
                                            Resident r = (Resident)searchedPerson;
                                            totalcost += r.CalculateSHNCharges();
                                        }
                                        else if (searchedPerson is Visitor) //Calculate for Visitors
                                        {
                                            Visitor v = (Visitor)searchedPerson;
                                            totalcost += v.CalculateSHNCharges();
                                        }
                                        totalcost = totalcost * 1.07;
                                    }
                                }

                                if (totalcost == 0) //Charges have been paid
                                {
                                    Console.WriteLine("No SHN charges.");
                                    Console.WriteLine();
                                    runloop = false;
                                    break;
                                }

                                else if (totalcost > 0) //Charges have not been paid
                                {
                                    while (true)
                                    {
                                        Console.WriteLine("The total SHN charges for {0} is ${1:0.00}.", searchedPerson.Name, totalcost);
                                        Console.Write("Would you like to proceed with payment? (Y/N): "); //Check if the person would like to proceed with payment
                                        string reply = Console.ReadLine().ToLower();
                                        if (reply == "y")
                                        {
                                            Console.WriteLine("Payment completed.");
                                            Console.WriteLine();
                                            totalcost = 0;
                                            for (int i = 0; i < unpaidTravelEntry.Count; i++)
                                            {
                                                unpaidTravelEntry[i].IsPaid = true;
                                            }
                                            runloop = false;
                                            break;
                                        }
                                        else if (reply == "n")
                                        {
                                            Console.WriteLine("Transaction cancelled.");
                                            Console.WriteLine();
                                            for (int i = 0; i < unpaidTravelEntry.Count; i++)
                                            {
                                                unpaidTravelEntry[i].IsPaid = false;
                                            }
                                            runloop = false;
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid input. Please try again.");
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("This person does not exist in our database. Please try another name.\nInput is case and space sensitive.");
                            Console.WriteLine();
                        }
                    }
                }

                //Contact Tracing Reporting
                else if (option == 12)
                {
                    while (true)
                    {
                        Console.Write("Please enter a business location: ");
                        string location = Console.ReadLine();
                        BusinessLocation searchedLocation = SearchBusiness(bList, location); //Search for business location
                        if (searchedLocation != null)
                        {
                            Console.Write("Enter a time (dd/mm/yyyy) : ");
                            DateTime time = Convert.ToDateTime(Console.ReadLine()); //User input for given datetime
                            string path = @"ContactTracingReporting.csv"; //File will be created in the bin folder
                            if (!File.Exists(path))
                            {
                                using (StreamWriter sw = File.CreateText(path)) //Append the header
                                {
                                    sw.WriteLine(
                                        "Name" + "," + "CheckIn" + "," + "CheckOut" + "," + "Business Location");
                                }
                            }
                            else //If the file already exists, delete the existing data so that a new csv with new data can be generated
                            {
                                File.Delete(path);
                                using (StreamWriter sw = File.CreateText(path)) //Append the header
                                {
                                    sw.WriteLine(
                                        "Name" + "," + "CheckIn" + "," + "CheckOut" + "," + "Business Location");
                                }
                            }

                            for (int i = 0; i < personList.Count; i++) //Loop through personList
                            {
                                for (int x = 0; x < personList[i].SafeEntryList.Count; x++) //Loop through the SafeEntry lists of the person
                                {
                                    if (location == personList[i].SafeEntryList[x].Location.BusinessName)
                                    {
                                        if (time.Day == personList[i].SafeEntryList[x].Checkin.Day) //Checking if the day is the same as user input
                                        {
                                            string personName = personList[i].Name;
                                            DateTime checkIn = personList[i].SafeEntryList[x].Checkin;
                                            DateTime checkOut = personList[i].SafeEntryList[x].Checkout;
                                            BusinessLocation bL = personList[i].SafeEntryList[x].Location;

                                            using (StreamWriter sw = File.AppendText(path)) //Appends the data above to the file
                                            {
                                                sw.WriteLine("\n" + personName + "," + checkIn + "," + checkOut + "," +
                                                             bL);
                                            }
                                        }
                                    }
                                }
                            }

                            using (StreamReader sr = File.OpenText(path))
                            {
                                string s = "";
                                while ((s = sr.ReadLine()) != null)
                                {
                                    Console.WriteLine(s);
                                }
                            }
                            Console.WriteLine();
                            break;
                        }
                        Console.WriteLine("Invalid business location entered. Please try again.");
                    }
                }

                //SHN Status Reporting
                else if (option == 13)
                {
                    try
                    {
                        Console.WriteLine("");
                        Console.Write("Enter a date(dd/mm/yyyy): ");
                        DateTime reportDate = Convert.ToDateTime(Console.ReadLine());
                        string path = @"SHNStatusReport.csv"; //File will be created in the bin folder
                        if (!File.Exists(path)) //If the file does not exist, create a new one
                        {
                            using (StreamWriter sw = File.AppendText(path)) //Append the header
                            {
                                sw.WriteLine("Name, End Date, Facility");
                            }
                        }
                        else //If the file already exists, delete the existing data so that a new csv with new data can be generated
                        {
                            File.Delete(path);
                            using (StreamWriter sw = File.AppendText(path))//Append the header
                            {
                                sw.WriteLine("Name, End Date, Facility");
                            }
                        }
                        for (int i = 0; i < personList.Count; i++) //Loop through person list
                        {
                            for (int x = 0; x < personList[i].TravelEntryList.Count; x++) //Loop through travel entries of the person
                            {
                                if (personList[i].TravelEntryList[x].EntryDate.Day == reportDate.Day) //Check if the day input is the same as day of entry of the person
                                {
                                    if ((personList[i].TravelEntryList[x].ShnEndDate - personList[i].TravelEntryList[x].EntryDate).TotalDays == 14) //For people that stay in a facility
                                    {
                                        string personName = personList[i].Name;
                                        DateTime endDate = personList[i].TravelEntryList[x].ShnEndDate;
                                        string facility = personList[i].TravelEntryList[x].ShnStay.FacilityName;

                                        using (StreamWriter sw = File.AppendText(path))
                                        {
                                            sw.WriteLine("\n" + personName + "," + endDate + "," + facility);
                                        }

                                    }
                                    else if ((personList[i].TravelEntryList[x].ShnEndDate - personList[i].TravelEntryList[x].EntryDate).TotalDays == 7) //For people that stay in their own accomodation 
                                    {
                                        string personName = personList[i].Name;
                                        DateTime endDate = personList[i].TravelEntryList[x].ShnEndDate;
                                        string facility = "NIL";

                                        using (StreamWriter sw = File.AppendText(path))
                                        {
                                            sw.WriteLine("\n" + personName + "," + endDate + "," + facility);
                                        }
                                    }
                                    else if ((personList[i].TravelEntryList[x].ShnEndDate - personList[i].TravelEntryList[x].EntryDate).TotalDays == 0) //For people that have no SHN
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                        Console.WriteLine("Report has been generated.");
                        Console.WriteLine();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Input format was invalid. Please try again.");
                    }
                }

                //Declare Covid-19 status
                else if (option == 14)
                {
                    bool runloop = true;
                    while (runloop)
                    {
                        Console.Write("Enter your name: ");
                        string personName = Console.ReadLine();
                        Person searchedPerson = SearchPerson(personList, personName); //Search for the person
                        

                        if (searchedPerson != null)
                        {
                            while (true)
                            {
                                Console.Write("Did you test positive for Covid-19? (Y/N): ");
                                string reply = Console.ReadLine().ToLower();
                                if (reply == "y")
                                {
                                    covidPList.Add(searchedPerson); //Person has Covid-19
                                    Console.WriteLine("OK.");
                                    Console.WriteLine();
                                    runloop = false;
                                    break;
                                }
                                else if (reply == "n")
                                {
                                    covidPList.Remove(searchedPerson);
                                    Console.WriteLine("OK.");
                                    Console.WriteLine();
                                    runloop = false;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input. Please try again.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("This person does not exist in our database. Please try another name.\nInput is case and space sensitive.");
                            Console.WriteLine();
                        }
                    }
                }

                //List Covid-19 positive people
                else if (option == 15)
                {
                    if (covidPList.Count > 0)
                    {
                        Console.WriteLine("{0,-20}{1,-35}{2,-40}{3,-40}{4,-40}", "Name", "Last Country of Embarkation", "Entry Date", "End Date", "Facility");
                        for (int i = 0; i < covidPList.Count; i++)
                        {
                            if (covidPList[i].TravelEntryList.Count > 0)
                            {
                                for (int x = covidPList[i].TravelEntryList.Count - 1; x < covidPList[i].TravelEntryList.Count; x++)
                                {
                                    DisplayCovidPPeople(covidPList, i, x);
                                }
                            }
                            else if (covidPList[i].TravelEntryList.Count == 0)
                            {
                                Console.WriteLine("{0,-20}{1,-35}{2,-40}{3,-40}{4,-40}", covidPList[i].Name, "NIL", "NIL", "NIL", "NIL");
                            }
                        }
                    } 
                    else
                    {
                        Console.WriteLine("No records found.");
                    }
                    
                    Console.WriteLine();
                }
                else if (option == 0)
                {
                    Console.WriteLine("Goodbye! Stay Safe!");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
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
                    Console.WriteLine("--------------------- COVID-19 MONITORING SYSTEM ---------------------");
                    Console.WriteLine();
                    Console.WriteLine("----------------- General --------------------");
                    Console.WriteLine("[1]  List All Visitors");
                    Console.WriteLine("[2]  List Person Details");
                    Console.WriteLine(" ");
                    Console.WriteLine("-------- SafeEntry/TraceTogether Token -------");
                    Console.WriteLine("[3]  Assign/Replace TraceTogether Token");
                    Console.WriteLine("[4]  List All Business Locations");
                    Console.WriteLine("[5]  Edit Business Location Capacity");
                    Console.WriteLine("[6]  SafeEntry Check-in");
                    Console.WriteLine("[7]  SafeEntry Check-out");
                    Console.WriteLine();
                    Console.WriteLine("----------------- TravelEntry ----------------");
                    Console.WriteLine("[8]  List All SHN Facilities");
                    Console.WriteLine("[9]  Create Visitor");
                    Console.WriteLine("[10] Create TravelEntry Record");
                    Console.WriteLine("[11] Calculate SHN Charges");
                    Console.WriteLine();
                    Console.WriteLine("----------------- Additional Features ----------------");
                    Console.WriteLine("[12] Contact Tracing Reporting");
                    Console.WriteLine("[13] SHN Status Reporting");
                    Console.WriteLine("[14] Declare Covid-19 status");
                    Console.WriteLine("[15] List Covid-19 postive people");
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
                    Console.WriteLine();
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

        static Person SearchPerson(List<Person> pList, string p) //To Search for the person object
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

        static void DisplayCovidPPeople(List<Person> covidPList, int i, int x)
        {
            if ((covidPList[i].TravelEntryList[x].ShnEndDate - covidPList[i].TravelEntryList[x].EntryDate).TotalDays == 14) //For people that stay in a facility
            {
                Console.WriteLine("{0,-20}{1,-35}{2,-40}{3,-40}{4,-40}", covidPList[i].Name, covidPList[i].TravelEntryList[x].LastCountryOfEmbarkation, covidPList[i].TravelEntryList[x].EntryDate, covidPList[i].TravelEntryList[x].ShnEndDate, covidPList[i].TravelEntryList[x].ShnStay.FacilityName);
            }
            else if ((covidPList[i].TravelEntryList[x].ShnEndDate - covidPList[i].TravelEntryList[x].EntryDate).TotalDays == 7) //For people that stay in their own accommodation 
            {
                Console.WriteLine("{0,-20}{1,-35}{2,-40}{3,-40}{4,-40}", covidPList[i].Name, covidPList[i].TravelEntryList[x].LastCountryOfEmbarkation, covidPList[i].TravelEntryList[x].EntryDate, covidPList[i].TravelEntryList[x].ShnEndDate, "NIL");
            }
            else if ((covidPList[i].TravelEntryList[x].ShnEndDate - covidPList[i].TravelEntryList[x].EntryDate).TotalDays == 0) //For people that have no SHN
            {
                Console.WriteLine("{0,-20}{1,-35}{2,-40}{3,-40}{4,-40}", covidPList[i].Name, covidPList[i].TravelEntryList[x].LastCountryOfEmbarkation, covidPList[i].TravelEntryList[x].EntryDate, "NIL", "NIL"); //For people that have no SHN
            }
        }

        /*----------------TRAVEL ENTRY FUNCTIONS-------------------*/
        static List<SHNFacility> GetFacilityDetails(List<SHNFacility> facilityList) //Loading SHNFacility data
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

        static SHNFacility SearchFacility(List<SHNFacility> fList, string f) //To search for the facility
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
                string lastCountry = checkCountry();
                Console.WriteLine();

                string entryMode = AssignEntryMode();
                Console.WriteLine();

                DateTime entryDate = checkDate();

                TravelEntry newTravelEntry = new TravelEntry(lastCountry, entryMode, entryDate);
                return newTravelEntry;
                
            }
        }

        static void InitializeCountryList(List<string> countryList)
        {
            string[] countryLines = File.ReadAllLines("Countries.csv");
            for (int i = 1; i < countryLines.Length; i++)
            {
                countryList.Add(countryLines[i]);
            }
        }

        static string checkCountry()
        {
            //List of countries in the world
            List<string> countryList = new List<string>();
            InitializeCountryList(countryList);

            while (true)
            {
                Console.Write("Enter your last country of embarkation: ");
                string lastCountry = Console.ReadLine();

                if (countryList.Contains(lastCountry))
                {
                    return lastCountry;
                }
                else
                {
                    Console.WriteLine("Invalid country. Please try again.\nInput is case and space sensitive.");
                    Console.WriteLine();
                }
            }
        }

        static DateTime checkDate() //Input validation for date
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter your entry date(dd/mm/yyyy hh:mm:ss): ");
                    DateTime entryDate = Convert.ToDateTime(Console.ReadLine());
                    DateTime validStartDate = Convert.ToDateTime("1/1/2020");

                    if (entryDate >= validStartDate && entryDate <= DateTime.Now)
                    {
                        return entryDate;
                    }
                    else
                    {
                        Console.WriteLine("Invalid date. Please enter a date that falls within 1 Jan 2020 and today.");
                        Console.WriteLine();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input format. Please try again.");
                    Console.WriteLine();
                }
            }
        }

        static string AssignEntryMode() //Method for users to select their entry mode
        {
            Console.WriteLine("----- Entry Modes -----");
            Console.WriteLine("[1]  Air");
            Console.WriteLine("[2]  Land");
            Console.WriteLine("[3]  Sea");

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

        static void InitTlist(List<String> tList) //Adding the collection location
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

        static BusinessLocation SearchBusiness(List<BusinessLocation> bList, string b) //Search method for business location
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

        static  String SearchCollection(List<String> tList, string b) //Search method for collection location 
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
