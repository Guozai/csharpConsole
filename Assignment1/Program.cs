using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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
        public Slot(string roomID, DateTime slotDateTime)
        {
            RoomID = roomID;
            SlotDateTime = slotDateTime;
        }

        public string RoomID { get; set; }
        public DateTime SlotDateTime { get; set; }
    }

    public class Initializer
    {
        public List<Staff> Staffs { get; }

        public Initializer()
        {
            const string connectionString = "Server=wdt2019.australiasoutheast.cloudapp.azure.com;Database=s3177105;Uid=s3177105;Password=abc123";

            using(var connection = new SqlConnection(connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "select * from [User]";

                var table = new DataTable();
                new SqlDataAdapter(command).Fill(table);

                Staffs = table.Select().Select(x =>
                    new Staff((string)x["UserID"], (string)x["Name"], (string)x["Email"])).ToList();
            }
        }

        public void Initialize()
        {
            //List<Staff> staffs = new List<Staff>();
            //staffs.Add(new Staff("e12345", "Matt", "e12345@rmit.edu.au"));
            //staffs.Add(new Staff("e56789", "Joe", "e56789@rmit.edu.au"));

            //List<Student> students = new List<Student>()
            //{
            //    new Student("s1234567", "Kevin", "s1234567@student.rmit.edu.au"),
            //    new Student("s4567890", "Oliver", "s4567890@student.rmit.edu.au")
            //};
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
                    ListRoom();
                    break;
                case "2":
                    ListSlot();
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

        public void ListRoom()
        {

        }

        public void ListSlot()
        {

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
                    break;
                case "3":
                    CreateSlot();
                    break;
                case "4":
                    RemoveSlot();
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

        }

        private void CreateSlot()
        { 

        }

        private void RemoveSlot()
        { 
        
        }

        public void StudentMenu()
        {

        }

        private void StudentMenuContent()
        {

        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Initializer initializer = new Initializer();
            initializer.Initialize();

            Menu menu = new Menu();
            menu.Start();
        }
    }
}
