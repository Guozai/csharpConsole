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
            try
            {
                var date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                List<Slot> availabilities = slots.Where(x => x.SlotDateTime.Date == date && x.StaffID.Equals(staffID)).ToList();
                List<Slot> sortedList = availabilities.OrderBy(x => x.SlotDateTime).ToList();
                foreach (var slot in sortedList)
                    Console.WriteLine("\t{0,-15}{1,-12:HH:mm}{2:HH:mm}", slot.RoomID, slot.SlotDateTime, slot.SlotDateTime.AddHours(1));
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine("Null Reference Exception: {0}", nre.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
        }

        ///////////////////////// Student Functional Methods ////////////////////////////////
        public void ListStudent(List<Student> students)
        {
            foreach (var student in students)
                Console.WriteLine("\t{0,-15}{1,-15}{2}", student.UserID, student.Name, student.Email);
        }

        public bool MakeBooking(string name, string dateString, string time, string studentID, Database database)
        {
            try
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
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine("Null Reference Exception: {0}", nre.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            return false;
        }

        public bool CancelBooking(string name, string dateString, string time, Database database)
        {
            try
            {
                DateTime dateTime = DateTime.ParseExact(dateString + " " + time, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                var slots = database.Slots.Where(x => x.SlotDateTime == dateTime && x.RoomID.Equals(name)).ToList();
                if (slots.Any())
                {
                    foreach (var slot in slots)
                    {
                        slot.StudentID = null;
                        // Update the slot in databse
                        database.Update(slot);
                        return true;
                    }
                }
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine("Null Reference Exception: {0}", nre.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            return false;
        }

        ///////////////////////// Slot Functional Methods ////////////////////////////////
        public void ListSlot(string input, List<Slot> databaseSlots)
        {
            try
            {
                DateTime date = DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var slots = databaseSlots.Where(x => x.SlotDateTime.Date == date).OrderBy(x => x.RoomID).ToList();

                if (slots.Any())
                {
                    foreach (var slot in slots)
                    {
                        Console.Write("\t{0,-15}{1,-15:HH:mm}{2,-15:HH:mm}{3,-15}", slot.RoomID, slot.SlotDateTime,
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
            catch (NullReferenceException nre)
            {
                Console.WriteLine("Null Reference Exception: {0}", nre.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
        }

        public bool CreateSlot(string name, string dateString, string time, string staffID, Database database)
        {
            try
            {
                // Parse the DateTime
                var date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var dateTime = DateTime.ParseExact(dateString + " " + time, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);

                // Check the business rules
                var countStaff = database.Slots.Count(x => x.SlotDateTime.Date == date && x.StaffID.Equals(staffID));
                var countSlot = database.Slots.Count(x => x.SlotDateTime.Date == date && x.RoomID.Equals(name));

                if (countStaff < 4 && countSlot < 2)
                {
                    // If slot doesn't exist
                    var count = database.Slots.Count(x => x.RoomID.Equals(name) && x.SlotDateTime == dateTime);
                    if (count == 0)
                    {
                        // Create the Slot
                        Slot slot = new Slot(name, dateTime, staffID, null);
                        database.AddSlot(slot);
                        // Add slot to database
                        database.Add(slot);
                        return true;
                    }
                }
            }
            catch(NullReferenceException nre)
            {
                Console.WriteLine("Null Reference Exception: {0}", nre.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            return false;
        }

        public bool RemoveSlot(string date, string time, Database database)
        {
            try
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
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine("Null Reference Exception: {0}", nre.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
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
            try
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
            catch (NullReferenceException nre)
            {
                Console.WriteLine("Null Reference Exception: {0}", nre.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
        }
    }
}
