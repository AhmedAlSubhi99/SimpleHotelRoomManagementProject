using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using System.Threading.Channels;
using System.Runtime.ConstrainedExecution;

namespace SimpleHotelRoomManagementProject
{
    internal class Program
    {
        const int MR = 10; // Max number of rooms
        static int[] rooms = new int[MR]; // Array to store room numbers
        static double[] dailyRates = new double[MR]; // Array to store daily rates
        static string[] guestNames = new string[MR]; // Array to store guest names
        static int[] nights = new int[MR]; // Array to store number of nights
        static bool[] isReserved = new bool[MR]; // Array to store reservation status
        static int roomCount = 0; // Count of rooms
        static int reservationCount = 0; // Count of reservations
        static DateTime[] bookingDates = new DateTime[MR]; // Array to store booking dates

        static void Main(string[] args)
        {
            //System Operations
            //1.Add a new room(Room Number, Daily Rate)
            //2.View all rooms(Available and Reserved)
            //3.Reserve a room for a guest (Guest Name, Room Number, Nights) 
            //4.View all reservations with total cost
            //5.Search reservation by guest name(case -insensitive) 
            //6.Find the highest-paying guest
            //7.Cancel a reservation by room number
            //8.Exit the system

            Console.WriteLine("                                          \t Cheers!!!..\n\t                                 (Hotel Room Management System!..)");
            Console.ReadLine();
            // For Buildind Menu of Choices.
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n ======= Hotel Room Management =======");
                Console.WriteLine("1. Add a new room");
                Console.WriteLine("2. View all rooms");
                Console.WriteLine("3. Reserve a room for a guest ");
                Console.WriteLine("4. View all reservations with total cost");
                Console.WriteLine("5. Search reservation by guest name");
                Console.WriteLine("6. Find the highest-paying guest");
                Console.WriteLine("7. Cancel a reservation by room number");
                Console.WriteLine("8. Exit");

                Console.Write("Enter your choice: ");
                int choice = int.Parse(Console.ReadLine());
                // To perform the operation based on the choice.
                switch (choice)
                {
                    case 1: AddNewRoom(); break;
                    case 2: ViewAllRooms(); break;
                    case 3: ReserveRoomForGuest(); break;
                    case 4: ViewAllReservationsWithTotalCost(); break;
                    case 5: SearchReservationByGuestName(); break;
                    case 6: FindTheHighestPayingGuest(); break;
                    case 7: CancelReservationByRoomNumber(); break;
                    case 8: return;
                    default: Console.WriteLine("Invalid choice! Try again."); break;
                }
                Console.WriteLine("\tPress any key to continue...");
                Console.ReadLine();
            }

            //======== Add New Room ==========
            static void AddNewRoom()
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("Hello For Adding New Rooms cheers!!(-_-) ");
                Console.WriteLine("=======================================");
                Console.ReadLine();

                try
                {
                    do
                    {
                        if (roomCount >= MR) // Check if room limit is reached
                        {
                            Console.WriteLine("Room limit reached. Cannot add more rooms.");
                            return;
                        }
                    }
                    while (roomCount >= MR); // if the room count is greater than the max room

                    Console.Write("Enter room number: ");
                    int roomNumber = int.Parse(Console.ReadLine());
                    Console.Write("Enter daily rate: ");
                    double rate = double.Parse(Console.ReadLine());


                   if (rate < 100) // Check if rate is valid
                   {
                                Console.WriteLine("Rate must be greater than 100.");
                                return; // exit the method
                   }

                        // Check if room number is unique
                        for (int i = 0; i < roomCount; i++) // loop to check if the room number already exists
                        {
                            if (rooms[i] == roomNumber) // if the room number already exists
                            {
                                Console.WriteLine("Room number already exists.");
                                return;
                            }
                        }


                    // Add room to the system
                    rooms[roomCount] = roomNumber; // store the room number
                    dailyRates[roomCount] = rate; // store the daily rate
                    isReserved[roomCount] = false; // set the room as not reserved
                    roomCount++; // increment the room count
                    Console.WriteLine("Room added successfully.");
                    Console.WriteLine("Room Number: " + roomNumber);
                    Console.WriteLine("Daily Rate: " + rate);
                } 
                catch (Exception ex) 
                { 
                    Console.WriteLine("Cant add room due to" , ex.Message);
                
                }
            }

            //======== View All Rooms ==========
            static void ViewAllRooms()
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("Hello For View All Rooms ");
                Console.WriteLine("=======================================");
                Console.ReadLine();

                try
                {
                    for (int i = 0; i < roomCount; i++) // loop to find the room index
                    {
                        Console.WriteLine("Room Number: " + rooms[i]); // display the room number
                        if (isReserved[i]) // check if the room is reserved
                        {
                            Console.WriteLine("Status: Reserved");
                            Console.WriteLine("Guest Name: " + guestNames[i]);
                            Console.WriteLine("Rate: " + dailyRates[i]);
                            Console.WriteLine("Nights: " + nights[i]);
                            Console.WriteLine("Booking Date: " + bookingDates[i]);
                            Console.WriteLine("Total Cost: " + (nights[i] * dailyRates[i]));
                        }
                        else
                        {
                            Console.WriteLine("Status: Available");
                        }
                    }

                    // Print all rooms with their details

                    Console.WriteLine("===========================================================================================================");
                    Console.WriteLine("Rooms\tGuest Names\tDaily Rates\tNights\t\tStatus\t\tBooking Dates\t\tTotal Cost"); // To display the student details.
                    Console.WriteLine("===========================================================================================================");

                    // Loop through the rooms and display their details
                    for (int i = 0; i < roomCount; i++)
                    {
                        Console.WriteLine($"{rooms[i]}\t{guestNames[i]}\t\t{dailyRates[i]}\t\t{nights[i]}\t{(isReserved[i] ? "Reserved" : "Available")}\t\t{bookingDates[i]}\t\t{(nights[i] * dailyRates[i])}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cant View rooms due to:", ex.Message);
                }
            }

            //======== Reserve Room For Guest ==========
            static void ReserveRoomForGuest()
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("Hello For Reserve Room For Guest!! ^.^ ");
                Console.WriteLine("=======================================");
                Console.ReadLine();

                try
                {
                    if (roomCount == 0) // Check if there are rooms available
                    {
                        Console.WriteLine("No rooms available.");
                        return;
                    }

                    Console.Write("Enter guest name: ");
                    string guestName = Console.ReadLine();

                    Console.Write("Enter room number: ");
                    int roomNumber = int.Parse(Console.ReadLine());

                    Console.Write("Enter number of nights: ");
                    int night = int.Parse(Console.ReadLine());

                    // Check if nights are valid
                    do
                    {
                        if (night < 0) // 
                        {
                            Console.WriteLine("Nights must be greater than 0.");
                            return;
                        }
                    } while (night < 0);

                    // Check if room exists and is not reserved
                    int roomIndex = -1; // set the room index to -1
                    for (int i = 0; i < roomCount; i++) // loop to find the room index
                    {
                        if (rooms[i] == roomNumber) // Check if the room is reserved
                        {
                            roomIndex = i; // store the room index on i
                            break;
                        }
                    }

                    // Check if room exists
                    if (roomIndex == -1) // if the room index is not found
                    {
                        Console.WriteLine("Room does not exist.");
                        return; // exit the method
                    }

                    // Check if room is reserved
                    if (isReserved[roomIndex]) // if the room is already reserved
                    {
                        Console.WriteLine("Room is already reserved !!!!.");
                        return; // exit the method
                    }


                    // Reserve the room
                    isReserved[roomIndex] = true; // set the room as reserved
                    guestNames[roomIndex] = guestName; // store the guest name
                    nights[roomIndex] = night; // store the number of nights
                    bookingDates[roomIndex] = DateTime.Now; // store the booking date
                    reservationCount++; // increment the reservation count
                    Console.WriteLine("Room reserved successfully.");
                    Console.WriteLine("Guest Name: " + guestName);
                    Console.WriteLine("Room Number: " + roomNumber);
                    Console.WriteLine("Number of Nights: " + night);
                    Console.WriteLine("Booking Date: " + bookingDates[roomIndex]);
                    Console.WriteLine("Total Cost: " + (night * dailyRates[roomIndex]));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cant reserve room due to:", ex.Message);
                }

            }

            //======== View All Reservations With Total Cost ==========
            static void ViewAllReservationsWithTotalCost()
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("Hello For View All Reservations With Total Cost");
                Console.WriteLine("=======================================");
                Console.ReadLine();
                try
                {
                    for (int i = 0; i < roomCount; i++) // loop in the rooms 
                    {
                        if (isReserved[i] == true) // if the room is reserved
                        {
                            // Display reservation details
                            Console.WriteLine("Status: Reserved");
                            Console.WriteLine("Guest Name: " + guestNames[i]);
                            Console.WriteLine("Room Number: " + rooms[i]);
                            Console.WriteLine("Nights: " + nights[i]);
                            Console.WriteLine("Rate: " + dailyRates[i]);
                            Console.WriteLine("Total Cost: " + (nights[i] * dailyRates[i]));
                            Console.WriteLine("Booking Date: " + bookingDates[i]);
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("Status: Available");
                        }

                    }

                    Console.WriteLine("===========================================================================================================");
                    Console.WriteLine("Rooms\tGuest Names\tDaily Rates\tNights\t\tStatus\t\tBooking Dates\t\tTotal Cost"); // To display the Room details.
                    Console.WriteLine("===========================================================================================================");
                    for (int i = 0; i < roomCount; i++)
                    {
                        Console.WriteLine($"{rooms[i]}\t{guestNames[i]}\t\t{dailyRates[i]}\t\t{nights[i]}\t{(isReserved[i] ? "Reserved" : "Available")}\t\t{bookingDates[i]}\t\t{(nights[i] * dailyRates[i])}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cant view reservations due to:", ex.Message);
                }

            }

            //======== Search Reservation By Guest Name ==========
            static void SearchReservationByGuestName()
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("Hello For Search Reservation By Guest Name");
                Console.WriteLine("=======================================");
                Console.ReadLine();

                try
                {
                    Console.Write("Enter guest name: ");
                    string guestName = Console.ReadLine();
                    bool found = false;

                    for (int i = 0; i < roomCount; i++) // loop to find room index
                    {
                        if (guestNames[i] == guestName) // 
                        {
                            // Display reservation details
                            Console.WriteLine("Status: Reserved");
                            Console.WriteLine("Room Number: " + rooms[i]);
                            Console.WriteLine("Nights: " + nights[i]);
                            Console.WriteLine("Rate: " + dailyRates[i]);
                            Console.WriteLine("Total Cost: " + (nights[i] * dailyRates[i]));
                            Console.WriteLine("Booking Date: " + bookingDates[i]);
                            found = true;
                        }
                        else
                        {
                            Console.WriteLine("Status: Available");
                        }

                    }

                    // check if not found 
                    if (!found)
                    {
                        Console.WriteLine("Not found");
                    }

                    Console.WriteLine("===========================================================================================================");
                    Console.WriteLine("Rooms\tGuest Names\tDaily Rates\tNights\t\tStatus\t\tBooking Dates\t\tTotal Cost"); // To display the Room details.
                    Console.WriteLine("===========================================================================================================");
                    for (int i = 0; i < roomCount; i++)
                    {
                        Console.WriteLine($"{rooms[i]}\t{guestNames[i]}\t\t{dailyRates[i]}\t\t{nights[i]}\t{(isReserved[i] ? "Reserved" : "Available")}\t\t{bookingDates[i]}\t\t{(nights[i] * dailyRates[i])}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cant search reservation due to:", ex.Message);
                }

            }

            //======== Find The Highest Paying Guest ==========
            static void FindTheHighestPayingGuest()
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("Hello For Find The Highest Paying Guest");
                Console.WriteLine("=======================================");
                Console.ReadLine();

                try
                {
                    //To track of the highest pay and the guest who made it
                    double highestAmount = 0;  // to store the highest amount
                    string highestGuest = "";  // to store the highest guest name

                    for (int i = 0; i < roomCount; i++) // Loop through all rooms.
                    {
                        if (isReserved[i]) // Check if the room is reserved.
                        {
                            double totalCost = nights[i] * dailyRates[i]; // Calculate the total cost.

                            if (totalCost > highestAmount) // Check if total cost is higher than highest amount.
                            {
                                highestAmount = totalCost; // Update the highest amount.
                                highestGuest = guestNames[i]; // Update the highest guest name inside Guset Names array.
                            }
                        }
                    }


                    // Print the highest paying guest and total amount

                    if (highestAmount > 0) // if the highest amount is greater than 0
                    {
                        Console.WriteLine("===========================================================================================================");
                        Console.WriteLine("Highest Paying Guest: " + highestGuest);
                        Console.WriteLine("Total Amount: " + highestAmount);
                    }
                    else
                    {
                        Console.WriteLine("No reservations exist.");
                    }

                    Console.WriteLine("===========================================================================================================");
                    Console.WriteLine("Rooms\tGuest Names\tDaily Rates\tNights\t\tStatus\t\tBooking Dates\t\tTotal Cost"); // To display the Room details.
                    Console.WriteLine("===========================================================================================================");
                    for (int i = 0; i < roomCount; i++)
                    {
                        Console.WriteLine($"{rooms[i]}\t{guestNames[i]}\t\t{dailyRates[i]}\t\t{nights[i]}\t{(isReserved[i] ? "Reserved" : "Available")}\t\t{bookingDates[i]}\t\t{(nights[i] * dailyRates[i])}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cant find the highest paying guest due to:", ex.Message);
                }

            }

            //======== Cancel Reservation By Room Number ==========
            static void CancelReservationByRoomNumber()
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("Hello For Cancel Reservation By Room Number");
                Console.WriteLine("=======================================");
                Console.ReadLine();
                try
                {
                    Console.Write("Enter room number: ");
                    int roomNumber = int.Parse(Console.ReadLine());
                    int roomIndex = -1;

                    // loop to find the room index
                    for (int i = 0; i < roomCount; i++)
                    {
                        if (rooms[i] == roomNumber) // check if the room number is match
                        {
                            roomIndex = i; // store the room index on i
                            break;
                        }
                    }

                    // Check if room exists
                    if (roomIndex == -1) // if the room index is not found
                    {
                        Console.WriteLine("Room does not exist.");
                        return; // exit the method
                    }

                    if (isReserved[roomIndex]) // check if the room is reserved
                    {
                        // Cancel the reservation
                        isReserved[roomIndex] = false; // set the room as not reserved
                        guestNames[roomIndex] = null; // clear the guest name
                        nights[roomIndex] = 0; // clear the number of nights
                        bookingDates[roomIndex] = DateTime.MinValue; // clear the booking date
                        reservationCount--; // decrement the reservation count
                        Console.WriteLine("Reservation cancelled successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Room is not reserved.");
                    }

                    Console.WriteLine("===========================================================================================================");
                    Console.WriteLine("Rooms\tGuest Names\tDaily Rates\tNights\t\tStatus\t\tBooking Dates\t\tTotal Cost"); // To display the Room details.
                    Console.WriteLine("===========================================================================================================");
                    for (int i = 0; i < roomCount; i++)
                    {
                        Console.WriteLine($"{rooms[i]}\t{guestNames[i]}\t\t{dailyRates[i]}\t\t{nights[i]}\t{(isReserved[i] ? "Reserved" : "Available")}\t\t{bookingDates[i]}\t\t{(nights[i] * dailyRates[i])}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cant cancel reservation due to:", ex.Message);
                }

            }

        }
     
    }
}
