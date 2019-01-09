using System;
using System.Collections;

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

    }
    public class Student : Person
    {

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
        void Initialize()
        {
            List<Staff> staffs = new List<Staff>();
        }

        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");
        }
    }
}
