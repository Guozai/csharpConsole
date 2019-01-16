using System;

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
        public Staff(string userID, string name, string email) : base(userID, name, email) { }
    }

    public class Student : Person
    {
        public Student(string userID, string name, string email) : base(userID, name, email) { }
    }

    public class Slot
    {
        public string RoomID { get; set; }
        public DateTime SlotDateTime { get; set; }
        public string StaffID { get; set; }
        public string StudentID { get; set; }

        public Slot(string roomID, DateTime slotDateTime, string staffID, string studentID)
        {
            RoomID = roomID;
            SlotDateTime = slotDateTime;
            StaffID = staffID;
            StudentID = studentID;
        }
    }
}
