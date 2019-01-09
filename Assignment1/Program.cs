using System;
using System.Collections;

namespace Assignment1
{
    public abstract class Person
    {
        private String UserID, Name, Email;
    }
    public class Staff : Person
    {

    }
    public class Student : Person
    {

    }
    public class Slot
    {
        private String RoomID;
        private DateTime dateTime;
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
