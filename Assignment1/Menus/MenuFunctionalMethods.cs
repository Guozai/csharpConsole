using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Assignment1
{
    public class MenuFunctionalMethods
    {
        ///////////////////////// Staff Functional Methods ////////////////////////////////
        public void ListStaff(List<Staff> staffs)
        {
            foreach (var staff in staffs)
                Console.WriteLine("\t{0,-15}{1,-15}{2}", staff.UserID, staff.Name, staff.Email);
        }

        public void StaffAvailability(string staffID, string dateString, List<Slot> slots)
        {
            var date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            List<Slot> availabilities = slots.Where(x => x.SlotDateTime.Date == date && x.StaffID.Equals(staffID)).ToList();
            List<Slot> sortedList = availabilities.OrderBy(x => x.SlotDateTime).ToList();
            foreach (var slot in sortedList)
                Console.WriteLine("\t{0,-15}{1,-12:hh:mm}{2:hh:mm}", slot.RoomID, slot.SlotDateTime, slot.SlotDateTime.AddHours(1));
        }

        ///////////////////////// Student Functional Methods ////////////////////////////////
        public void ListStudent(List<Student> students)
        {
            foreach (var student in students)
                Console.WriteLine("\t{0,-15}{1,-15}{2}", student.UserID, student.Name, student.Email);
        }

        public bool MakeBooking(string name, string dateString, string time, string studentID, Database database)
        {
            // If the student hasn't booked on the day
            DateTime date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var count = database.Slots.Count(x => x.SlotDateTime.Date == date && x.StudentID == studentID);

            if (count == 0)
            {
                DateTime dateTime = DateTime.ParseExact(dateString + " " + time, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                var slots = database.Slots.Where(x => x.SlotDateTime == dateTime && x.RoomID.Equals(name)).ToList();
                if (slots.Any())
                {
                    foreach (var slot in slots)
                    {
                        if (slot.StudentID == null)
                        {
                            slot.StudentID = studentID;
                            // Update the slot in database
                            database.Update(slot);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool CancelBooking(string name, string dateString, string time, Database database)
        {
            DateTime dateTime = DateTime.ParseExact(dateString + " " + time, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
            var slots = database.Slots.Where(x => x.SlotDateTime == dateTime && x.RoomID.Equals(name)).ToList();
            if (slots.Any())
            {
                foreach (var slot in database.Slots)
                {
                    if (slot.SlotDateTime == dateTime && slot.RoomID.Equals(name))
                    {
                        slot.StudentID = null;
                        // Update the slot in databse
                        database.Update(slot);
                        return true;
                    }
                }
            }
            return false;
        }

        ///////////////////////// Slot Functional Methods ////////////////////////////////
        public void ListSlot(string input, List<Slot> databaseSlots)
        {
            DateTime date = DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var slots = databaseSlots.Where(x => x.SlotDateTime.Date == date).ToList();

            if (slots.Any())
            {
                foreach (var slot in slots)
                {
                    Console.Write("\t{0,-15}{1,-15:hh:mm}{2,-15:hh:mm}{3,-15}", slot.RoomID, slot.SlotDateTime,
                        slot.SlotDateTime.AddHours(1), slot.StaffID);
                    if (slot.StudentID == null)
                        Console.WriteLine("-");
                    else
                        Console.WriteLine(slot.StudentID);
                }
            }
            else
                Console.WriteLine("\t<no slots>");
        }

        public bool CreateSlot(string name, string date, string time, string staffID, Database database)
        {
            // Parse the DateTime
            DateTime dateTime = DateTime.ParseExact(date + " " + time, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);

            // Check the business rules
            int countStaff = 0;
            int countSlot = 0;
            if (database.Slots != null)
            {
                foreach (var slot in database.Slots)
                {
                    if (slot.StaffID.Equals(staffID))
                        countStaff++;
                    if (slot.RoomID.Equals(name))
                        countSlot++;
                }
            }
            if (countStaff < 4 && countSlot < 2)
            {
                // Create the Slot
                Slot slot = new Slot(name, dateTime, staffID);
                database.AddSlot(slot);
                // Add slot to database
                database.Add(slot);
                return true;
            }
            else
                return false;
        }

        public bool RemoveSlot(string date, string time, Database database)
        {
            // Parse the DateTime
            DateTime dateTime = DateTime.ParseExact(date + " " + time, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
            var slots = database.Slots.Where(x => x.SlotDateTime == dateTime).ToList();
            if (slots.Any())
            {
                foreach (var slot in slots)
                {
                    if (slot.StudentID == null)
                    {
                        database.Slots.Remove(slot);
                        // Delete slot from database
                        database.Delete(slot);
                        return true;
                    }
                }
            }
            return false;
        }

        ///////////////////////// Room Functional Methods ////////////////////////////////
        public void ListRoom(List<string> rooms)
        {
            if (rooms != null)
            {
                foreach (var room in rooms)
                    Console.WriteLine("\t" + room);
            }
        }

        public void RoomAvailability(string input, List<Slot> databaseSlots, List<string> rooms)
        {
            DateTime date = DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var slots = databaseSlots.Where(x => x.SlotDateTime.Date == date).ToList();
            if (slots.Any())
            {
                foreach (var room in rooms)
                {
                    var count = slots.Count(x => x.RoomID.Equals(room));
                    if (count < 2)
                        Console.WriteLine($"\t{room}");
                }
            }
        }
    }
}
