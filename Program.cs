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
        const int MAX_ROOMS = 10;
        static int[] rooms = new int[MAX_ROOMS];
        static double[] dailyRates = new double[MAX_ROOMS];
        static string[] guestNames = new string[MAX_ROOMS];
        static int[] nights = new int[MAX_ROOMS];
        static bool[] isReserved = new bool[MAX_ROOMS];
        static int roomCount = 0;
        static int reservationCount = 0;
        static DateTime[] bookingDates = new DateTime[MAX_ROOMS];

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

            Console.WriteLine("                                          \t Cheers!!!..\n\t                                 Hotel Room Management System!..");
            Console.ReadLine();
            // For Buildind Menu of Choices.
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n Hotel Room Management");
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


                if (roomCount >= MAX_ROOMS) // Check if room limit is reached
                {
                    Console.WriteLine("Room limit reached. Cannot add more rooms.");
                    return;
                }
                Console.Write("Enter room number: ");
                int roomNumber = int.Parse(Console.ReadLine());
                Console.Write("Enter daily rate: ");
                double rate = double.Parse(Console.ReadLine());
                if (rate < 100) // Check if rate is valid
                {
                    Console.WriteLine("Rate must be >= 100.");
                    return;
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
            //======== View All Rooms ==========
            static void ViewAllRooms()
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("Hello For View All Rooms ");
                Console.WriteLine("=======================================");
                Console.ReadLine();


                for (int i = 0; i < roomCount; i++)
                {
                    Console.WriteLine("Room Number: " + rooms[i]);
                    if (isReserved[i]) // check if the room is reserved
                    {
                        Console.WriteLine("Status: Reserved");
                        Console.WriteLine("Guest Name: " + guestNames[i]);
                        Console.WriteLine("Nights: " + nights[i]);
                        Console.WriteLine("Booking Date: " + bookingDates[i]);
                    }
                    else
                    {
                        Console.WriteLine("Status: Available");
                    }
                    Console.WriteLine("Daily Rate: " + dailyRates[i]);

                    Console.WriteLine("===========================================================================================================");
                    Console.WriteLine("Rooms\tGuest Names\tDaily Rates\tNights\tReserved\t\tBooking Dates\t\tTotal Cost"); // To display the student details.
                    Console.WriteLine("===========================================================================================================");
                    for (int i = 0; i < roomCount; i++)
                    {
                        Console.WriteLine($"{rooms[i]}\t{guestNames[i]}\t\t{dailyRates[i]}\t\t{nights[i]}\t{isReserved[i]}\t\t{bookingDates[i]}\t\t{(nights[i] * dailyRates[i])}");
                    }

                }

            }
            //======== Reserve Room For Guest ==========
            static void ReserveRoomForGuest()
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("Hello For Reserve Room For Guest");
                Console.WriteLine("=======================================");
                Console.ReadLine();

                //• Input: Guest name, room number, and number of nights 
                //• Validations:
                //o Room must exist
                //o Room must not already be reserved
                //o Nights > 0
                //• Reserve the room and store guest name, nights, and booking date
                Console.Write("Enter guest name: ");
                string guestName = Console.ReadLine();
                Console.Write("Enter room number: ");
                int roomNumber = int.Parse(Console.ReadLine());
                Console.Write("Enter number of nights: ");
                int night = int.Parse(Console.ReadLine());
                if (night <= 0)
                {
                    Console.WriteLine("Nights must be greater than 0.");
                    return;
                }
                // Check if room exists and is not reserved
                int roomIndex = -1;
                for (int i = 0; i < roomCount; i++)
                {
                    if (rooms[i] == roomNumber)
                    {
                        roomIndex = i;
                        break;
                    }
                }
                if (roomIndex == -1)
                {
                    Console.WriteLine("Room does not exist.");
                    return;
                }
                if (isReserved[roomIndex])
                {
                    Console.WriteLine("Room is already reserved.");
                    return;
                }
                // Reserve the room
                isReserved[roomIndex] = true;
                guestNames[roomIndex] = guestName;
                nights[roomIndex] = night;
                bookingDates[roomIndex] = DateTime.Now;
                reservationCount++;
                Console.WriteLine("Room reserved successfully.");
                Console.WriteLine("Guest Name: " + guestName);
                Console.WriteLine("Room Number: " + roomNumber);
                Console.WriteLine("Number of Nights: " + night);
                Console.WriteLine("Booking Date: " + bookingDates[roomIndex]);
                Console.WriteLine("Total Cost: " + (night * dailyRates[roomIndex]));

            }
            //======== View All Reservations With Total Cost ==========
            static void ViewAllReservationsWithTotalCost()
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("Hello For View All Reservations With Total Cost");
                Console.WriteLine("=======================================");
                Console.ReadLine();


                for (int i = 0; i < roomCount; i++)
                {
                    if (isReserved[i])
                    {
                        Console.WriteLine("Guest Name: " + guestNames[i]);
                        Console.WriteLine("Room Number: " + rooms[i]);
                        Console.WriteLine("Nights: " + nights[i]);
                        Console.WriteLine("Rate: " + dailyRates[i]);
                        Console.WriteLine("Total Cost: " + (nights[i] * dailyRates[i]));
                        Console.WriteLine("Booking Date: " + bookingDates[i]);
                        Console.WriteLine();
                    }
                }
                //• If no reservations exist, print a message
                if (reservationCount == 0)
                {
                    Console.WriteLine("No reservations exist.");
                }
                else
                {
                    Console.WriteLine("Total Reservations: " + reservationCount);
                }
                Console.WriteLine("===========================================================================================================");
                Console.WriteLine("Rooms\tGuest Names\tDaily Rates\tNights\t\tBooking Dates\t\tTotal Cost"); // To display the student details.
                Console.WriteLine("===========================================================================================================");
                for (int i = 0; i < roomCount; i++)
                {
                    Console.WriteLine($"{rooms[i]}\t{guestNames[i]}\t{dailyRates[i]}\t{nights[i]}\t\t{bookingDates[i]}\t\t{(nights[i] * dailyRates[i])}");
                }

            }
            //======== Search Reservation By Guest Name ==========
            static void SearchReservationByGuestName()
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("Hello For Search Reservation By Guest Name");
                Console.WriteLine("=======================================");
                Console.ReadLine();


                Console.Write("Enter guest name: ");
                string guestName = Console.ReadLine();
                bool found = false;

                for (int i = 0; i < roomCount; i++)
                {
                    if (isReserved[i] && guestNames[i].Equals(guestName, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Guest Name: " + guestNames[i]);
                        Console.WriteLine("Room Number: " + rooms[i]);
                        Console.WriteLine("Nights: " + nights[i]);
                        Console.WriteLine("Rate: " + dailyRates[i]);
                        Console.WriteLine("Total Cost: " + (nights[i] * dailyRates[i]));
                        Console.WriteLine("Booking Date: " + bookingDates[i]);
                        found = true;
                    }
                }
                if (!found)
                {
                    Console.WriteLine("Not found");
                }
                else
                {
                    Console.WriteLine("Total Reservations: " + reservationCount);
                }
                Console.WriteLine("===========================================================================================================");
                Console.WriteLine("Rooms\tGuest Names\tDaily Rates\tNights\t\tBooking Dates\t\tTotal Cost"); // To display the student details.
                Console.WriteLine("===========================================================================================================");
                for (int i = 0; i < roomCount; i++)
                {
                    Console.WriteLine($"{rooms[i]}\t{guestNames[i]}\t{dailyRates[i]}\t{nights[i]}\t\t{bookingDates[i]}\t\t{(nights[i] * dailyRates[i])}");
                }
                
            }
            //======== Find The Highest Paying Guest ==========
            static void FindTheHighestPayingGuest()
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("Hello For Find The Highest Paying Guest");
                Console.WriteLine("=======================================");
                Console.ReadLine();

                double highestAmount = 0;
                string highestGuest = "";

                    for (int i = 0; i < roomCount; i++)
                    {
                        if (isReserved[i])
                        {
                            double totalCost = nights[i] * dailyRates[i];
                            if (totalCost > highestAmount)
                            {
                                highestAmount = totalCost;
                                highestGuest = guestNames[i];
                            }
                        }
                    }
                if (highestAmount > 0)
                {
                    Console.WriteLine("Highest Paying Guest: " + highestGuest);
                    Console.WriteLine("Total Amount: " + highestAmount);
                }
                else
                {
                    Console.WriteLine("No reservations exist.");
                }
                Console.WriteLine("===========================================================================================================");
                Console.WriteLine("Rooms\tGuest Names\tDaily Rates\tNights\t\tBooking Dates\t\tTotal Cost"); // To display the student details.
                Console.WriteLine("===========================================================================================================");
                for (int i = 0; i < roomCount; i++)
                {
                    Console.WriteLine($"{rooms[i]}\t{guestNames[i]}\t{dailyRates[i]}\t{nights[i]}\t\t{bookingDates[i]}\t\t{(nights[i] * dailyRates[i])}");
                }

            }
            //======== Cancel Reservation By Room Number ==========
            static void CancelReservationByRoomNumber()
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("Hello For Cancel Reservation By Room Number");
                Console.WriteLine("=======================================");
                Console.ReadLine();


                Console.Write("Enter room number: ");
                int roomNumber = int.Parse(Console.ReadLine());
                int roomIndex = -1;

                // loop to find the room index
                for (int i = 0; i < roomCount; i++)
                {
                    if (rooms[i] == roomNumber)
                    {
                        roomIndex = i;
                        break;
                    }
                }
                // Check if room exists
                if (roomIndex == -1)
                {
                    Console.WriteLine("Room does not exist.");
                    return;
                }
                // Check if room is reserved
                if (isReserved[roomIndex])
                {
                    isReserved[roomIndex] = false;
                    guestNames[roomIndex] = null;
                    nights[roomIndex] = 0;
                    reservationCount--;
                    Console.WriteLine("Reservation cancelled successfully.");
                }
                // If room is not reserved
                else
                {
                    Console.WriteLine("Room is not reserved.");
                }

                // Print all reservations
                Console.WriteLine("===========================================================================================================");
                Console.WriteLine("Rooms\tGuest Names\tDaily Rates\tNights\t\tBooking Dates\t\tTotal Cost"); // To display the student details.
                Console.WriteLine("===========================================================================================================");
                for (int i = 0; i < roomCount; i++)
                {
                    Console.WriteLine($"{rooms[i]}\t{guestNames[i]}\t{dailyRates[i]}\t{nights[i]}\t\t{bookingDates[i]}\t\t{(nights[i] * dailyRates[i])}");
                }
                // Print total reservations
                if (reservationCount == 0)
                {
                    Console.WriteLine("No reservations exist.");
                }
                else
                {
                    Console.WriteLine("Total Reservations: " + reservationCount);
                }

            }


        }
     
    }
}
