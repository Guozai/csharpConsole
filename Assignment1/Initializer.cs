using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
//using Microsoft.Extensions.Configuration;

namespace Assignment1
{
    public class Initializer
    {
        public List<Staff> Staffs { get; }
        public List<Student> Students { get; }
        public List<string> Rooms { get; }
        public List<Slot> Slots { get; private set; }

        public Initializer()
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

        public void Add()
        {

        }

        public void Delete()
        {

        }

        public void Update()
        {

        }
    }
}
