using System;

namespace Assignment1
{
    public class Menu
    {
        private Database database { get; } = Database.Instance;
        // Facade
        private Facade facade = new Facade();

        public void Start()
        {
            Console.WriteLine(new string('-', 59));
            Console.WriteLine("Welcome to Appointment Scheduling and Reservation System");
            Console.WriteLine(new string('-', 59));
            MainMenu();
        }

        private void MainMenu()
        {
            MainMenuContent();

            var input = Console.ReadLine();
            while (input != "1" && input != "2" && input != "3" && input != "4" && input != "5")
            {
                Console.Write("Invalid input. Please input again: ");
                input = Console.ReadLine();
            }

            switch (input)
            {
                case "1":
                    facade.ListRoom(database.Rooms);
                    MainMenu();
                    break;
                case "2":
                    facade.ListSlot(database.Slots);
                    MainMenu();
                    break;
                case "3":
                    Console.WriteLine();
                    Console.WriteLine("Entering staff menu");
                    StaffMenu();
                    break;
                case "4":
                    Console.WriteLine();
                    Console.WriteLine("Entering student menu");
                    StudentMenu();
                    break;
                case "5":
                default:
                    break;
            }
        }

        private void MainMenuContent()
        {
            Console.WriteLine();
            Console.WriteLine(new string('-', 59));
            Console.WriteLine("Main menu:");
            Console.WriteLine("        1. List rooms");
            Console.WriteLine("        2. List slots");
            Console.WriteLine("        3. Staff menu");
            Console.WriteLine("        4. Student menu");
            Console.WriteLine("        5. Exit");
            Console.WriteLine();
            Console.Write("Enter option: ");
        }

        private void StaffMenu()
        {
            StaffMenuContent();

            var input = Console.ReadLine();
            while (input != "1" && input != "2" && input != "3" && input != "4" && input != "5")
            {
                Console.Write("Invalid input. Please input again: ");
                input = Console.ReadLine();
            }

            switch (input)
            {
                case "1":
                    facade.ListStaff(database.Staffs);
                    StaffMenu();
                    break;
                case "2":
                    facade.RoomAvailability(database.Slots, database.Rooms);
                    StaffMenu();
                    break;
                case "3":
                    facade.CreateSlot(database);
                    StaffMenu();
                    break;
                case "4":
                    facade.RemoveSlot(database);
                    StaffMenu();
                    break;
                case "5":
                    Console.WriteLine();
                    Console.WriteLine("Exiting staff menu.");
                    MainMenu();
                    break;
                default:
                    break;
            }
        }

        private void StaffMenuContent()
        {
            Console.WriteLine();
            Console.WriteLine(new string('-', 59));
            Console.WriteLine("Staff menu:");
            Console.WriteLine("        1. List staff");
            Console.WriteLine("        2. Room availability");
            Console.WriteLine("        3. Create slot");
            Console.WriteLine("        4. Remove slot");
            Console.WriteLine("        5. Exit");
            Console.WriteLine();
            Console.Write("Enter option: ");
        }

        public void StudentMenu()
        {
            StudentMenuContent();

            var input = Console.ReadLine();
            while (input != "1" && input != "2" && input != "3" && input != "4" && input != "5")
            {
                Console.Write("Invalid input. Please input again: ");
                input = Console.ReadLine();
            }

            switch (input)
            {
                case "1":
                    facade.ListStudent(database.Students);
                    StudentMenu();
                    break;
                case "2":
                    facade.StaffAvailability(database.Staffs, database.Slots);
                    StudentMenu();
                    break;
                case "3":
                    facade.MakeBooking(database);
                    StudentMenu();
                    break;
                case "4":
                    facade.CancelBooking(database);
                    StudentMenu();
                    break;
                case "5":
                    Console.WriteLine();
                    Console.WriteLine("Exiting student menu.");
                    MainMenu();
                    break;
                default:
                    break;
            }
        }

        private void StudentMenuContent()
        {
            Console.WriteLine();
            Console.WriteLine(new string('-', 59));
            Console.WriteLine("Student menu:");
            Console.WriteLine("        1. List students");
            Console.WriteLine("        2. Staff availability");
            Console.WriteLine("        3. Make booking");
            Console.WriteLine("        4. Cancel booking");
            Console.WriteLine("        5. Exit");
            Console.WriteLine();
            Console.Write("Enter option: ");
        }
    }
}
