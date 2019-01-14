using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Globalization;

namespace Assignment1
{
    public abstract class Person
    {
        public Person(string userID, string name, string email)
        {
            UserID = userID;
            Name = name;
            Email = email;
        }

        public string UserID { get; }
        public string Name { get; }
        public string Email { get; }
    }

    public class Staff : Person
    {
        public Staff(string userID, string name, string email) : base (userID, name, email) { }
    }

    public class Student : Person
    {
        public Student(string userID, string name, string email) : base (userID, name, email) { }
    }

    public class Slot
    {
        public string RoomID { get; set; }
        public string Time { get; set; }
        public string StaffID { get; set; }
        public string StudentID { get; set; }

        public Slot(string roomID, string time, string staffID)
        {
            RoomID = roomID;
            Time = time;
            StaffID = staffID;
        }

        public string AddHour(string time)
        {
            char[] seps = { ':' };
            string[] parts = time.Split(seps);
            if (int.TryParse(parts[0], out var hh))
                parts[0] = (hh + 1).ToString();
            return String.Join(":", parts);
        }
    }

    //public class Room
    //{
    //    public string RoomID { get; }

    //    public Room(string roomID)
    //    {
    //        RoomID = roomID;
    //    }
    //}

    public class Initializer
    {
        public List<Staff> Staffs { get; }
        public List<Student> Students { get; }
        public List<string> Rooms { get; }
        public List<Slot> Slots { get; private set; }

        public Initializer()
        {
            const string connectionString = "Server=wdt2019.australiasoutheast.cloudapp.azure.com;Database=s3177105;Uid=s3177105;Password=abc123";

            using(var connection = new SqlConnection(connectionString))
            {
                // Retrieve Staff List
                var command = connection.CreateCommand();
                command.CommandText = "select * from [User] where [UserID] like 'e%' and len([UserID]) = 6";

                var table = new DataTable();
                new SqlDataAdapter(command).Fill(table);

                Staffs = table.Select().Select(x =>
                    new Staff((string)x["UserID"], (string)x["Name"], (string)x["Email"])).ToList();

                table.Clear();
                // Retrieve Student List
                command.CommandText = "select * from [User] where [UserID] like 's%' and len([UserID]) = 8";
                new SqlDataAdapter(command).Fill(table);
                Students = table.Select().Select(x =>
                    new Student((string)x["UserID"], (string)x["Name"], (string)x["Email"])).ToList();

                table.Clear();
                // Retrieve Room List
                command.CommandText = "select * from Room";
                new SqlDataAdapter(command).Fill(table);
                Rooms = table.Select().Select(x => (string)x["RoomID"]).ToList();
                //Rooms = table.Select().Select(x => new Room((string)x["RoomID"])).ToList();
                //Rooms = new List<string>() { "A", "B", "C", "D"};
            }
        }

        public void AddSlot(Slot slot)
        {
            if (Slots == null)
                Slots = new List<Slot>();
            Slots.Add(slot);
        }
    }

    public class Menu
    {
        private Initializer initializer { get; } = new Initializer();

        public void Start()
        {
            Console.WriteLine(new string('-', 59));
            Console.WriteLine("Welcome to Appointment Scheduling and Reservation System");
            Console.WriteLine(new string('-', 59));
            MainMenu();
        }

        public void MainMenu()
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
                    ListRoom(initializer.Rooms);
                    MainMenu();
                    break;
                case "2":
                    ListSlot();
                    MainMenu();
                    break;
                case "3":
                    Console.WriteLine();
                    Console.WriteLine("Entering staff menu");
                    StaffMenu();
                    break;
                case "4":
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

        public void ListRoom(List<string> rooms)
        {
            Console.WriteLine();
            Console.WriteLine("--- List rooms ---");
            Console.WriteLine("\tRoom Name");
            if (rooms != null)
            {
                foreach (var room in rooms)
                    Console.WriteLine("\t" + room);
            }
        }

        public void ListSlot()
        {
            Console.WriteLine();
            Console.WriteLine("--- List slots ---");
            Console.Write("Enter date for slots (dd-mm-yyyy): ");
            var input = Console.ReadLine();

            // Retrieve the dd mm yyyy parts
            char[] seps = { '-', '-' };
            string[] parts = input.Split(seps);

            while (parts.Length != 3 || !int.TryParse(parts[0], out var dd) || !(dd >= 1 && dd <= 31)
                || !int.TryParse(parts[1], out var mm) || !(mm >= 1 && mm <= 12)
                || !int.TryParse(parts[2], out var yyyy))
            {
                Console.Write("Invalid input, either not a date or not in (dd-mm-yyyy) format: ");
                input = Console.ReadLine();
                parts = input.Split(seps);
            }

            //DateTime date = DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            Console.WriteLine();
            Console.WriteLine($"Slots on {input}:");
            Console.WriteLine("\t{0,-15}{1,-15}{2,-15}{3,-15}{4}", "Room name", "Start time", "End time", "Staff ID", "Bookings");

            if (input.Equals("30-01-2019") && initializer.Slots != null)
            {
                foreach ( var slot in initializer.Slots)
                {
                    Console.Write("\t{0,-15}{1,-15}{2,-15}{3,-15}", slot.RoomID, slot.Time, slot.AddHour(slot.Time), slot.StaffID);
                    if (slot.StudentID == null)
                        Console.WriteLine("-");
                    else
                        Console.WriteLine(slot.StudentID);
                }
            }
            else
                Console.WriteLine("\t<no slots>");

        }

        public void StaffMenu()
        {
            StaffMenuContent();

            var input = Console.ReadLine();
            while (input != "1" && input != "2" && input != "3" && input != "4" && input != "5")
            {
                Console.Write("Invalid input. Please input again: ");
                input = Console.ReadLine();
            }

            switch(input)
            {
                case "1":
                    ListStaff(initializer.Staffs);
                    StaffMenu();
                    break;
                case "2":
                    RoomAvailability();
                    StaffMenu();
                    break;
                case "3":
                    CreateSlot();
                    StaffMenu();
                    break;
                case "4":
                    RemoveSlot();
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

        private void ListStaff(List<Staff> staffs)
        {
            Console.WriteLine();
            Console.WriteLine("--- List staff ---");
            Console.WriteLine("\t{0,-15}{1,-15}{2}", "ID", "Name", "Email");
            foreach (var staff in staffs)
            {
                Console.WriteLine("\t{0,-15}{1,-15}{2}", staff.UserID, staff.Name, staff.Email);
            }
        }

        private void RoomAvailability()
        {
            Console.WriteLine();
            Console.WriteLine("--- Room availability ---");
            Console.Write("Enter date for room availability (dd-mm-yyyy): ");
            var input = Console.ReadLine();

            // Retrieve the dd mm yyyy parts
            char[] seps = { '-', '-' };
            string[] parts = input.Split(seps);

            while (parts.Length != 3 || !int.TryParse(parts[0], out var dd) || !(dd >= 1 && dd <= 31)
                || !int.TryParse(parts[1], out var mm) || !(mm >= 1 && mm <= 12)
                || !int.TryParse(parts[2], out var yyyy))
            {
                Console.Write("Invalid input, either not a date or not in (dd-mm-yyyy) format: ");
                input = Console.ReadLine();
                parts = input.Split(seps);
            }

            DateTime date = DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            Console.WriteLine();
            Console.WriteLine($"Rooms available on {input}:"); 
            Console.WriteLine("\tRoom name");

            if (initializer.Slots == null)
            {
                foreach ( var room in initializer.Rooms)
                {
                    Console.WriteLine($"\t{room}");
                }
            }
            //Console.Write("\tA\n\tB\n\tC\n\tD\n");
        }

        private void CreateSlot()
        {
            Console.WriteLine();
            Console.WriteLine("--- Create slot ---");
            Console.Write("Enter room name: ");
            var name = Console.ReadLine();

            while (!initializer.Rooms.Contains(name))
            {
                Console.Write("Invalid room name. Enter room name: ");
                name = Console.ReadLine();
            }

            Console.Write("Enter date for slot (dd-mm-yyyy): ");
            var date = Console.ReadLine();

            // Retrieve the dd mm yyyy parts
            char[] seps = { '-', '-' };
            string[] parts = date.Split(seps);
            while (parts.Length != 3 || !int.TryParse(parts[0], out var dd) || !(dd >= 1 && dd <= 31)
                || !int.TryParse(parts[1], out var mm) || !(mm >= 1 && mm <= 12)
                || !int.TryParse(parts[2], out var yyyy))
            {
                Console.Write("Invalid input, either not a date or not in (dd-mm-yyyy) format: ");
                date = Console.ReadLine();
                parts = date.Split(seps);
            }

            Console.Write("Enter time for slot (hh:mm): ");
            var time = Console.ReadLine();

            // Retrieve the hh mm parts
            char[] sepsTime = { ':' };
            string[] partsTime = time.Split(sepsTime);
            while (partsTime.Length != 2 || !int.TryParse(partsTime[0], out var hh) || !(hh >= 9 && hh <= 14)
                || !int.TryParse(partsTime[1], out var mm) || mm != 0)
            {
                Console.Write("Invalid input, must be (hh:mm) format and between 9:00 and 14:00: ");
                time = Console.ReadLine();
                partsTime = time.Split(sepsTime);
            }

            Console.Write("Enter staff ID: ");
            var staffID = Console.ReadLine();
            bool hasStaff = false;
            do
            {
                foreach ( var staff in initializer.Staffs)
                {
                    if (staff.UserID.Equals(staffID))
                    {
                        hasStaff = true;
                        break;
                    }
                }
                if (!hasStaff)
                {
                    Console.Write("Wrong Input. Enter staff ID: ");
                    staffID = Console.ReadLine();
                }
            } while (!hasStaff);
            //while (!staffID.StartsWith('e') || staffID.Length != 6)
            //{
            //    Console.Write("Wrong Input. Enter staff ID: ");
            //    staffID = Console.ReadLine();
            //}

            Console.WriteLine();
            // Parse the DateTime
            //DateTime dateTime = DateTime.ParseExact(date + " " + time, "dd-MM-yyyy hh:mm", CultureInfo.InvariantCulture);

            // Check the business rules
            int countStaff = 0;
            int countSlot = 0;
            if (initializer.Slots != null)
            {
                foreach ( var slot in initializer.Slots)
                {
                    if (slot.StaffID.Equals(staffID))
                        countStaff++;
                    if (slot.RoomID.Equals(name))
                        countSlot++;
                }
            }
            if (date.Equals("30-01-2019") && countStaff < 4 && countSlot < 2)
            {
                // Create the Slot
                initializer.AddSlot(new Slot(name, time, staffID));
                Console.WriteLine("Slot created successfully.");
            }
            else
                Console.WriteLine("Unable to create slot.");
        }

        private void RemoveSlot()
        {
            Console.WriteLine();
            Console.WriteLine("--- Remove slot ---");
            Console.Write("Enter room name: ");
            var name = Console.ReadLine();
            while (!initializer.Rooms.Contains(name))
            {
                Console.Write("Invalid room name. Enter room name: ");
                name = Console.ReadLine();
            }

            Console.Write("Enter date for slot (dd-mm-yyyy): ");
            var date = Console.ReadLine();

            // Retrieve the dd mm yyyy parts
            char[] seps = { '-', '-' };
            string[] parts = date.Split(seps);
            while (parts.Length != 3 || !int.TryParse(parts[0], out var dd) || !(dd >= 1 && dd <= 31)
                || !int.TryParse(parts[1], out var mm) || !(mm >= 1 && mm <= 12)
                || !int.TryParse(parts[2], out var yyyy))
            {
                Console.Write("Invalid input, either not a date or not in (dd-mm-yyyy) format: ");
                date = Console.ReadLine();
                parts = date.Split(seps);
            }

            Console.Write("Enter time for slot (hh:mm): ");
            var time = Console.ReadLine();

            // Retrieve the hh mm parts
            char[] sepsTime = { ':' };
            string[] partsTime = time.Split(sepsTime);
            while (partsTime.Length != 2 || !int.TryParse(partsTime[0], out var hh) || !(hh >= 0 && hh <= 23)
                || !int.TryParse(partsTime[1], out var mm) || mm != 0)
            {
                Console.Write("Invalid input, either not a time or not in (hh:mm) format: ");
                time = Console.ReadLine();
                partsTime = time.Split(sepsTime);
            }

            Console.WriteLine();
            if (date.Equals("30-01-2019"))
            {
                foreach ( var slot in initializer.Slots)
                {
                    if (slot.Time.Equals(time))
                    {
                        if (slot.StudentID == null)
                        {
                            initializer.Slots.Remove(slot);
                            Console.WriteLine("Slot removed successfully.");
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Unable to remove slot.");
                            return;
                        }
                    }
                }
            }
            Console.WriteLine("Slot doesn't exist.");
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
                    ListStudent(initializer.Students);
                    StudentMenu();
                    break;
                case "2":
                    StaffAvailability();
                    StudentMenu();
                    break;
                case "3":
                    MakeBooking();
                    StaffMenu();
                    break;
                case "4":
                    CancelBooking();
                    StaffMenu();
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

        private void ListStudent(List<Student> students)
        {
            Console.WriteLine();
            Console.WriteLine("--- List students ---");
            Console.WriteLine("\t{0,-15}{1,-15}{2}", "ID", "Name", "Email");
            foreach (var student in students)
            {
                Console.WriteLine("\t{0,-15}{1,-15}{2}", student.UserID, student.Name, student.Email);
            }
        }

        private void StaffAvailability()
        {
            Console.WriteLine();
            Console.WriteLine("--- Staff availability ---");
            Console.Write("Enter date for staff availability (dd-mm-yyyy): ");
            var date = Console.ReadLine();

            char[] seps = { '-', '-' };
            string[] parts = date.Split(seps);
            while (parts.Length != 3 || !int.TryParse(parts[0], out var dd) || !(dd >= 1 && dd <= 31)
                || !int.TryParse(parts[1], out var mm) || !(mm >= 1 && mm <= 12)
                || !int.TryParse(parts[2], out var yyyy))
            {
                Console.Write("Invalid input, either not a date or not in (dd-mm-yyyy) format: ");
                date = Console.ReadLine();
                parts = date.Split(seps);
            }

            Console.Write("Enter staff ID: ");
            var staffID = Console.ReadLine();

            bool hasStaff = false;
            do
            {
                foreach (var staff in initializer.Staffs)
                {
                    if (staff.UserID.Equals(staffID))
                    {
                        hasStaff = true;
                        break;
                    }
                }
                if (!hasStaff)
                {
                    Console.Write("Wrong Input. Enter staff ID: ");
                    staffID = Console.ReadLine();
                }
            } while (!hasStaff);

            Console.WriteLine();
            Console.WriteLine($"Staff {staffID} availability on {date}:");
            Console.WriteLine("\t{0,-15}{1,-12}{2}", "Room name", "Start time", "End time");
            List<Slot> availabilities = new List<Slot>();
            foreach ( var slot in initializer.Slots)
            {
                if (slot.StaffID.Equals(staffID))
                    availabilities.Add(slot);
            }
            List<Slot> sortedList = availabilities.OrderBy(x => x.Time).ToList();
            foreach ( var slot in sortedList)
                Console.WriteLine("\t{0,-15}{1,-12}{2}", slot.RoomID, slot.Time, slot.AddHour(slot.Time));
        }

        private void MakeBooking()
        {
            Console.WriteLine();
            Console.WriteLine("--- Make booking ---");
            Console.Write("Enter room name: ");
            var name = Console.ReadLine();

            while (!initializer.Rooms.Contains(name))
            {
                Console.Write("Invalid room name. Enter room name: ");
                name = Console.ReadLine();
            }

            Console.Write("Enter date for slot (dd-mm-yyyy): ");
            var date = Console.ReadLine();

            // Retrieve the dd mm yyyy parts
            char[] seps = { '-', '-' };
            string[] parts = date.Split(seps);
            while (parts.Length != 3 || !int.TryParse(parts[0], out var dd) || !(dd >= 1 && dd <= 31)
                || !int.TryParse(parts[1], out var mm) || !(mm >= 1 && mm <= 12)
                || !int.TryParse(parts[2], out var yyyy))
            {
                Console.Write("Invalid input, either not a date or not in (dd-mm-yyyy) format: ");
                date = Console.ReadLine();
                parts = date.Split(seps);
            }

            Console.Write("Enter time for slot (hh:mm): ");
            var time = Console.ReadLine();

            // Retrieve the hh mm parts
            char[] sepsTime = { ':' };
            string[] partsTime = time.Split(sepsTime);
            while (partsTime.Length != 2 || !int.TryParse(partsTime[0], out var hh) || !(hh >= 9 && hh <= 14)
                || !int.TryParse(partsTime[1], out var mm) || mm != 0)
            {
                Console.Write("Invalid input, must be (hh:mm) format and between 9:00 and 14:00: ");
                time = Console.ReadLine();
                partsTime = time.Split(sepsTime);
            }

            Console.Write("Enter student ID: ");
            var studentID = Console.ReadLine();
            bool hasStudent = false;
            do
            {
                foreach (var student in initializer.Students)
                {
                    if (student.UserID.Equals(studentID))
                    {
                        hasStudent = true;
                        break;
                    }
                }
                if (!hasStudent)
                {
                    Console.Write("Wrong Input. Enter student ID: ");
                    studentID = Console.ReadLine();
                }
            } while (!hasStudent);

            Console.WriteLine();

            // If the student hasn't booked on the day
            //foreach (var slot in initializer.Slots)
            //{
            //    if (slot.Time)
            //}
        }

        private void CancelBooking()
        {

        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            menu.Start();
        }
    }
}

// x[" "] is DBNull ? null : (string)x[" "]

// var slots = Slots.Where(x => x.StartTime.Date == date).ToList();
// if(!slots.Any())