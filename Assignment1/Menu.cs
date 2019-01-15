using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Assignment1
{
    public class Menu
    {
        private Initializer initializer { get; } = Initializer.Instance;

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

        private void ListRoom(List<string> rooms)
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

        private void ListSlot()
        {
            Console.WriteLine();
            Console.WriteLine("--- List slots ---");
            Console.WriteLine("Enter date for slots (dd-mm-yyyy): ");

            var input = EnterDate();

            Console.WriteLine();
            Console.WriteLine($"Slots on {input}:");
            Console.WriteLine("\t{0,-15}{1,-15}{2,-15}{3,-15}{4}", "Room name", "Start time", "End time", "Staff ID", "Bookings");

            DateTime date = DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var slots = initializer.Slots.Where(x => x.SlotDateTime.Date == date).ToList();
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

            var input = EnterDate();

            Console.WriteLine();
            Console.WriteLine($"Rooms available on {input}:");
            Console.WriteLine("\tRoom name");

            DateTime date = DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var slots = initializer.Slots.Where(x => x.SlotDateTime.Date == date).ToList();
            if (slots.Any())
            {
                foreach (var room in initializer.Rooms)
                {
                    var count = slots.Count(x => x.RoomID.Equals(room));
                    if (count < 2)
                        Console.WriteLine($"\t{room}");
                }
            }
        }

        private void CreateSlot()
        {
            Console.WriteLine();
            Console.WriteLine("--- Create slot ---");
            var name = EnterName();
            Console.Write("Enter date for slot (dd-mm-yyyy): ");
            var date = EnterDate();
            Console.Write("Enter time for slot (hh:mm): ");
            var time = EnterTime();
            var staffID = EnterStaffID();
            Console.WriteLine();

            // Parse the DateTime
            DateTime dateTime = DateTime.ParseExact(date + " " + time, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);

            // Check the business rules
            int countStaff = 0;
            int countSlot = 0;
            if (initializer.Slots != null)
            {
                foreach (var slot in initializer.Slots)
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
                initializer.AddSlot(slot);
                // Add slot to database
                initializer.Add(slot);
                Console.WriteLine("Slot created successfully.");
            }
            else
                Console.WriteLine("Unable to create slot.");
        }

        private void RemoveSlot()
        {
            Console.WriteLine();
            Console.WriteLine("--- Remove slot ---");
            var name = EnterName();
            Console.Write("Enter date for slot (dd-mm-yyyy): ");
            var date = EnterDate();
            Console.Write("Enter time for slot (hh:mm): ");
            var time = EnterTime();
            Console.WriteLine();

            // Parse the DateTime
            DateTime dateTime = DateTime.ParseExact(date + " " + time, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
            var slots = initializer.Slots.Where(x => x.SlotDateTime == dateTime).ToList();
            if (slots.Any())
            {
                foreach (var slot in slots)
                {
                    if (slot.StudentID == null)
                    {
                        initializer.Slots.Remove(slot);
                        // Delete slot from database
                        initializer.Delete(slot);
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
                    StudentMenu();
                    break;
                case "4":
                    CancelBooking();
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
            var date = EnterDate();
            var staffID = EnterStaffID();
            Console.WriteLine();
            Console.WriteLine($"Staff {staffID} availability on {date}:");
            Console.WriteLine("\t{0,-15}{1,-12}{2}", "Room name", "Start time", "End time");

            List<Slot> availabilities = new List<Slot>();
            foreach (var slot in initializer.Slots)
            {
                if (slot.StaffID.Equals(staffID))
                    availabilities.Add(slot);
            }
            List<Slot> sortedList = availabilities.OrderBy(x => x.SlotDateTime).ToList();
            foreach (var slot in sortedList)
                Console.WriteLine("\t{0,-15}{1,-12:hh:mm}{2:hh:mm}", slot.RoomID, slot.SlotDateTime, slot.SlotDateTime.AddHours(1));
        }

        private void MakeBooking()
        {
            Console.WriteLine();
            Console.WriteLine("--- Make booking ---");
            var name = EnterName();
            Console.Write("Enter date for slot (dd-mm-yyyy): ");
            var dateString = EnterDate();
            Console.Write("Enter time for slot (hh:mm): ");
            var time = EnterTime();
            var studentID = EnterStudentID();
            Console.WriteLine();

            // If the student hasn't booked on the day
            DateTime date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var count = initializer.Slots.Count(x => x.SlotDateTime.Date == date && x.StudentID == studentID);

            if (count == 0)
            {
                DateTime dateTime = DateTime.ParseExact(dateString + " " + time, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                var slots = initializer.Slots.Where(x => x.SlotDateTime == dateTime && x.RoomID.Equals(name)).ToList();
                if (slots.Any())
                {
                    foreach (var slot in slots)
                    {
                        if (slot.StudentID == null)
                        {
                            slot.StudentID = studentID;
                            // Update the slot in database
                            initializer.Update(slot);
                            Console.WriteLine("Slot booked successfully.");
                            return;
                        }
                    }
                }
            }
            Console.WriteLine("Unable to book slot.");
        }

        private void CancelBooking()
        {
            Console.WriteLine();
            Console.WriteLine("--- Cancel booking ---");
            var name = EnterName();
            Console.Write("Enter date for slot (dd-mm-yyyy): ");
            var dateString = EnterDate();
            Console.Write("Enter time for slot (hh:mm): ");
            var time = EnterTime();

            DateTime dateTime = DateTime.ParseExact(dateString + " " + time, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
            var slots = initializer.Slots.Where(x => x.SlotDateTime == dateTime && x.RoomID.Equals(name)).ToList();
            if (slots.Any())
            {
                foreach (var slot in initializer.Slots)
                {
                    if (slot.SlotDateTime == dateTime && slot.RoomID.Equals(name))
                    {
                        slot.StudentID = null;
                        // Update the slot in databse
                        initializer.Update(slot);
                        Console.WriteLine("Slot cancelled successfully.");
                    }
                }

            }
            Console.WriteLine("Unable to cancel slot.");
        }

        /// <summary>
        /// Generic Menu Methods
        /// </summary>
        /// <returns>string</returns>
        private string EnterName()
        {
            Console.Write("Enter room name: ");
            var name = Console.ReadLine();

            while (!initializer.Rooms.Contains(name))
            {
                Console.Write("Invalid room name. Enter room name: ");
                name = Console.ReadLine();
            }

            return name;
        }

        private string EnterDate()
        {
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

            return input;
        }

        private string EnterTime()
        {
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

            return time;
        }

        private string EnterStaffID()
        {
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

            return staffID;
        }

        private string EnterStudentID()
        {
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

            return studentID;
        }
    }
}
