using System;
using System.Collections.Generic;

namespace Assignment1
{
    public class MenuGenericMethods
    {
        public string EnterName(List<string> rooms)
        {
            Console.Write("Enter room name: ");
            var name = Console.ReadLine();

            while (!rooms.Contains(name))
            {
                Console.Write("Invalid room name. Enter room name: ");
                name = Console.ReadLine();
            }

            return name;
        }

        public string EnterDate()
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

        public string EnterTime()
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

        public string EnterStaffID(List<Staff> staffs)
        {
            Console.Write("Enter staff ID: ");
            var staffID = Console.ReadLine();
            bool hasStaff = false;
            do
            {
                foreach (var staff in staffs)
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

        public string EnterStudentID(List<Student> students)
        {
            Console.Write("Enter student ID: ");
            var studentID = Console.ReadLine();
            bool hasStudent = false;
            do
            {
                foreach (var student in students)
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
