using System;
using System.Collections.Generic;

// Facade implementation using https://www.dofactory.com/net/facade-design-pattern
namespace Assignment1
{
    public class Facade
    {
        private MenuGenericMethods genericMethods = new MenuGenericMethods();
        private MenuFunctionalMethods functionalMethods = new MenuFunctionalMethods();

        public void ListRoom(List<string> rooms) 
        {
            Console.WriteLine();
            Console.WriteLine("--- List rooms ---");
            Console.WriteLine("\tRoom Name");
            functionalMethods.ListRoom(rooms);
        }

        public void ListSlot(List<Slot> slots)
        {
            Console.WriteLine();
            Console.WriteLine("--- List slots ---");
            Console.Write("Enter date for slots (dd-mm-yyyy): ");

            var input = genericMethods.EnterDate();

            Console.WriteLine();
            Console.WriteLine($"Slots on {input}:");
            Console.WriteLine("\t{0,-15}{1,-15}{2,-15}{3,-15}{4}", "Room name", "Start time", "End time", "Staff ID", "Bookings");

            functionalMethods.ListSlot(input, slots);
        }

        public void ListStaff(List<Staff> staffs)
        {
            Console.WriteLine();
            Console.WriteLine("--- List staff ---");
            Console.WriteLine("\t{0,-15}{1,-15}{2}", "ID", "Name", "Email");
            functionalMethods.ListStaff(staffs);
        }

        public void RoomAvailability(List<Slot> slots, List<string> rooms)
        {
            Console.WriteLine();
            Console.WriteLine("--- Room availability ---");
            Console.Write("Enter date for room availability (dd-mm-yyyy): ");

            var input = genericMethods.EnterDate();

            Console.WriteLine();
            Console.WriteLine($"Rooms available on {input}:");
            Console.WriteLine("\tRoom name");

            functionalMethods.RoomAvailability(input, slots, rooms);
        }

        public void CreateSlot(Database database)
        {
            Console.WriteLine();
            Console.WriteLine("--- Create slot ---");
            var name = genericMethods.EnterName(database.Rooms);
            Console.Write("Enter date for slot (dd-mm-yyyy): ");
            var date = genericMethods.EnterDate();
            Console.Write("Enter time for slot (hh:mm): ");
            var time = genericMethods.EnterTime();
            var staffID = genericMethods.EnterStaffID(database.Staffs);
            Console.WriteLine();

            if (functionalMethods.CreateSlot(name, date, time, staffID, database))
                Console.WriteLine("Slot created successfully.");
            else
                Console.WriteLine("Unable to create slot.");
        }

        public void RemoveSlot(Database database)
        {
            Console.WriteLine();
            Console.WriteLine("--- Remove slot ---");
            var name = genericMethods.EnterName(database.Rooms);
            Console.Write("Enter date for slot (dd-mm-yyyy): ");
            var date = genericMethods.EnterDate();
            Console.Write("Enter time for slot (hh:mm): ");
            var time = genericMethods.EnterTime();
            Console.WriteLine();

            if (functionalMethods.RemoveSlot(date, time, database))
                Console.WriteLine("Slot removed successfully.");
            else
                Console.WriteLine("Unable to remove slot.");
        }

        public void ListStudent(List<Student> students)
        {
            Console.WriteLine();
            Console.WriteLine("--- List students ---");
            Console.WriteLine("\t{0,-15}{1,-15}{2}", "ID", "Name", "Email");
            functionalMethods.ListStudent(students);
        }

        public void StaffAvailability(List<Staff> staffs, List<Slot> slots)
        {
            Console.WriteLine();
            Console.WriteLine("--- Staff availability ---");
            Console.Write("Enter date for staff availability (dd-mm-yyyy): ");
            var date = genericMethods.EnterDate();
            var staffID = genericMethods.EnterStaffID(staffs);
            Console.WriteLine();
            Console.WriteLine($"Staff {staffID} availability on {date}:");
            Console.WriteLine("\t{0,-15}{1,-12}{2}", "Room name", "Start time", "End time");

            functionalMethods.StaffAvailability(staffID, date, slots);
        }

        public void MakeBooking(Database database)
        {
            Console.WriteLine();
            Console.WriteLine("--- Make booking ---");
            var name = genericMethods.EnterName(database.Rooms);
            Console.Write("Enter date for slot (dd-mm-yyyy): ");
            var date = genericMethods.EnterDate();
            Console.Write("Enter time for slot (hh:mm): ");
            var time = genericMethods.EnterTime();
            var studentID = genericMethods.EnterStudentID(database.Students);
            Console.WriteLine();

            if (functionalMethods.MakeBooking(name, date, time, studentID, database))
                Console.WriteLine("Slot booked successfully.");
            else
                Console.WriteLine("Unable to book slot.");
        }

        public void CancelBooking(Database database)
        {
            Console.WriteLine();
            Console.WriteLine("--- Cancel booking ---");
            var name = genericMethods.EnterName(database.Rooms);
            Console.Write("Enter date for slot (dd-mm-yyyy): ");
            var date = genericMethods.EnterDate();
            Console.Write("Enter time for slot (hh:mm): ");
            var time = genericMethods.EnterTime();

            if (functionalMethods.CancelBooking(name, date, time, database))
                Console.WriteLine("Slot cancelled successfully.");
            else
                Console.WriteLine("Unable to cancel slot.");
        }
    }
}
