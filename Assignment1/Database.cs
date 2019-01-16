using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

// http://csharpindepth.com/Articles/General/Singleton.aspx
namespace Assignment1
{
    public sealed class Database
    {
        public List<Staff> Staffs { get; }
        public List<Student> Students { get; }
        public List<string> Rooms { get; }
        public List<Slot> Slots { get; private set; }

        private static readonly Lazy<Database> lazy =
        new Lazy<Database>(() => new Database());

        public static Database Instance { get { return lazy.Value; } }

        private Database()
        {
            using (var connection = new SqlConnection(Program.ConnectionString))
            {
                // Retrieve Staff List
                var command = connection.CreateCommand();
                command.CommandText = "select * from [User] where [UserID] like 'e%' and len([UserID]) = 6";

                var table = new DataTable();
                new SqlDataAdapter(command).Fill(table);

                Staffs = table.Select().Select(x =>
                    new Staff((string)x["UserID"], (string)x["Name"], (string)x["Email"])).ToList();

                table.Clear();
                // Retrieve Student List
                command.CommandText = "select * from [User] where [UserID] like 's%' and len([UserID]) = 8";
                new SqlDataAdapter(command).Fill(table);
                Students = table.Select().Select(x =>
                    new Student((string)x["UserID"], (string)x["Name"], (string)x["Email"])).ToList();

                table.Clear();
                // Retrieve Room List
                command.CommandText = "select * from Room";
                new SqlDataAdapter(command).Fill(table);
                Rooms = table.Select().Select(x => (string)x["RoomID"]).ToList();
                //Rooms = table.Select().Select(x => new Room((string)x["RoomID"])).ToList();
                //Rooms = new List<string>() { "A", "B", "C", "D"};

                table.Clear();
                // Retrieve Slot List
                command.CommandText = "select * from Slot";
                new SqlDataAdapter(command).Fill(table);
                if (table != null)
                    Slots = table.Select().Select(x =>
                        new Slot((string)x["RoomID"], (DateTime)x["StartTime"], (string)x["StaffID"])).ToList();
                else
                    Slots = new List<Slot>();
            }
        }

        public void AddSlot(Slot slot)
        {
            if (Slots == null)
                Slots = new List<Slot>();
            Slots.Add(slot);
        }

        public void Add(Slot slot)
        {
            using (var connection = new SqlConnection(Program.ConnectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    "insert into Slot values (@roomID, @startTime, @staffID, null)";
                command.Parameters.AddWithValue("roomID", slot.RoomID);
                command.Parameters.AddWithValue("startTime", slot.SlotDateTime);
                command.Parameters.AddWithValue("staffID", slot.StaffID);
                //command.Parameters.AddWithValue("studentID", slot.StudentID);

                command.ExecuteNonQuery();
            }
        }

        public void Delete(Slot slot)
        {
            using (var connection = new SqlConnection(Program.ConnectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    "delete from Slot where RoomID = @roomID and StartTime = @startTime";
                command.Parameters.AddWithValue("roomID", slot.RoomID);
                command.Parameters.AddWithValue("startTime", slot.SlotDateTime);

                command.ExecuteNonQuery();
            }
        }

        public void Update(Slot slot)
        {
            using (var connection = new SqlConnection(Program.ConnectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    "update Slot set BookedInStudentID = @studentID where RoomID = @roomID and StartTime = @startTime";
                command.Parameters.AddWithValue("studentID", slot.StudentID);
                command.Parameters.AddWithValue("roomID", slot.RoomID);
                command.Parameters.AddWithValue("startTime", slot.SlotDateTime);

                command.ExecuteNonQuery();
            }
        }
    }
}
