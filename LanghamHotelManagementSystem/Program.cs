
using System;
using System.Collections.Generic;
using System.IO;

namespace HotelManagementSystem
{
    // Room class
    public class Room
    {
        public int RoomNo { get; set; }
        public bool IsAllocated { get; set; }
    }

    // Customer class
    public class Customer
    {
        public int CustomerNo { get; set; }
        public string CustomerName { get; set; }
    }

    // Room Allocation class
    public class RoomAllocation
    {
        public int AllocatedRoomNo { get; set; }
        public Customer AllocatedCustomer { get; set; }
    }

    class Program
    {
        static List<Room> listOfRooms = new List<Room>();
        static List<RoomAllocation> roomAllocations = new List<RoomAllocation>();
        static string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "HotelManagement.txt");

        static void Main(string[] args)
        {
            char choice;

            do
            {
                Console.Clear();
                Console.WriteLine("========== LANGHAM HOTEL MANAGEMENT ==========");
                Console.WriteLine("1. Add Rooms");
                Console.WriteLine("2. Display Rooms");
                Console.WriteLine("3. Allocate Room");
                Console.WriteLine("4. Deallocate Room");
                Console.WriteLine("5. Show Room Allocations");
                Console.WriteLine("6. Save Allocations to File");
                Console.WriteLine("7. Load Allocations from File");
                Console.WriteLine("8. Exit");
                Console.Write("Enter your choice: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddRooms();
                        break;
                    case "2":
                        DisplayRooms();
                        break;
                    case "3":
                        AllocateRoom();
                        break;
                    case "4":
                        DeallocateRoom();
                        break;
                    case "5":
                        ShowAllocations();
                        break;
                    case "6":
                        SaveToFile();
                        break;
                    case "7":
                        LoadFromFile();
                        break;
                    case "8":
                        Console.WriteLine("Exiting... Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }

                Console.Write("\nDo you want to continue? (Y/N): ");
                choice = Console.ReadKey().KeyChar;
                Console.WriteLine();
            } while (choice == 'Y' || choice == 'y');
        }

        static void AddRooms()
        {
            try
            {
                Console.Write("How many rooms do you want to add? ");
                int count = int.Parse(Console.ReadLine());

                for (int i = 0; i < count; i++)
                {
                    Console.Write("Enter Room Number: ");
                    int roomNo = int.Parse(Console.ReadLine());

                    listOfRooms.Add(new Room { RoomNo = roomNo, IsAllocated = false });
                }

                Console.WriteLine("Rooms added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding rooms: " + ex.Message);
            }
        }

        static void DisplayRooms()
        {
            Console.WriteLine("List of Rooms:");
            foreach (Room room in listOfRooms)
            {
                string status = room.IsAllocated ? "Allocated" : "Available";
                Console.WriteLine($"Room No: {room.RoomNo}, Status: {status}");
            }
        }

        static void AllocateRoom()
        {
            try
            {
                Console.Write("Enter Room Number to Allocate: ");
                int roomNo = int.Parse(Console.ReadLine());

                Room room = listOfRooms.Find(r => r.RoomNo == roomNo);

                if (room == null)
                {
                    Console.WriteLine("Room not found.");
                    return;
                }

                if (room.IsAllocated)
                {
                    Console.WriteLine("Room is already allocated.");
                    return;
                }

                Console.Write("Enter Customer Number: ");
                int custNo = int.Parse(Console.ReadLine());

                Console.Write("Enter Customer Name: ");
                string custName = Console.ReadLine();

                Customer customer = new Customer { CustomerNo = custNo, CustomerName = custName };
                RoomAllocation allocation = new RoomAllocation { AllocatedRoomNo = roomNo, AllocatedCustomer = customer };

                roomAllocations.Add(allocation);
                room.IsAllocated = true;

                Console.WriteLine("Room allocated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during allocation: " + ex.Message);
            }
        }

        static void DeallocateRoom()
        {
            try
            {
                Console.Write("Enter Room Number to Deallocate: ");
                int roomNo = int.Parse(Console.ReadLine());

                Room room = listOfRooms.Find(r => r.RoomNo == roomNo);

                if (room == null || !room.IsAllocated)
                {
                    Console.WriteLine("Room not found or already available.");
                    return;
                }

                roomAllocations.RemoveAll(ra => ra.AllocatedRoomNo == roomNo);
                room.IsAllocated = false;

                Console.WriteLine("Room deallocated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during deallocation: " + ex.Message);
            }
        }

        static void ShowAllocations()
        {
            Console.WriteLine("Room Allocation Details:");
            foreach (RoomAllocation ra in roomAllocations)
            {
                Console.WriteLine($"Room No: {ra.AllocatedRoomNo}, Customer No: {ra.AllocatedCustomer.CustomerNo}, Name: {ra.AllocatedCustomer.CustomerName}");
            }
        }

        static void SaveToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (RoomAllocation ra in roomAllocations)
                    {
                        writer.WriteLine($"{ra.AllocatedRoomNo},{ra.AllocatedCustomer.CustomerNo},{ra.AllocatedCustomer.CustomerName}");
                    }
                }

                Console.WriteLine("Data saved to file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving to file: " + ex.Message);
            }
        }

        static void LoadFromFile()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("No saved data found.");
                    return;
                }

                string[] lines = File.ReadAllLines(filePath);
                roomAllocations.Clear();

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');

                    int roomNo = int.Parse(parts[0]);
                    int custNo = int.Parse(parts[1]);
                    string custName = parts[2];

                    roomAllocations.Add(new RoomAllocation
                    {
                        AllocatedRoomNo = roomNo,
                        AllocatedCustomer = new Customer { CustomerNo = custNo, CustomerName = custName }
                    });

                    Room room = listOfRooms.Find(r => r.RoomNo == roomNo);
                    if (room != null)
                        room.IsAllocated = true;
                }

                Console.WriteLine("Data loaded from file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading from file: " + ex.Message);
            }
        }
    }
}

