using System;
using System.Collections.Generic;

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
    public class Program
    {
        public static void Initialize()
        {
            List<Staff> staffs = new List<Staff>();
            staffs.Add(new Staff("e12345", "Matt", "e12345@rmit.edu.au"));
            staffs.Add(new Staff("e56789", "Joe", "e56789@rmit.edu.au"));
        }

        static void Main(string[] args)
        {
            Initialize();

            Console.WriteLine("Hello World!");
        }
    }
}
