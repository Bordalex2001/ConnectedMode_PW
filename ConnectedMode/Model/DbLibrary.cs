using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Windows;

namespace ConnectedMode.Model
{
    class DbLibrary : IDisposable
    {
        private string connectionString = null;
        private SqlConnection connection = null;

        public DbLibrary()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = @"Alex";
            builder.InitialCatalog = "Library2";
            builder.IntegratedSecurity = true;
           // builder.UserID = "Vasya";
           // builder.Password = "qwerty";
            

            connectionString = builder.ConnectionString;
            connection = new SqlConnection(connectionString);
        }
        public void Dispose()
        {
            connection.Close();
        }

        public List<Faculty> GetFaculties()
        {
            string commandText = "SELECT * FROM Faculties";
            List<Faculty> facultiesList = new List<Faculty>();

            try
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = commandText;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    facultiesList.Add(new Faculty()
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"]
                    });
                }

                reader.Close();
                
            }
            finally
            {
                connection.Close();
            }

            return facultiesList;
        }

        public List<Group> GetGroups(int idFaculty)
        {
            string commandText = "SELECT * FROM Groups WHERE Id_Faculty = @id";
            List<Group> groupsList = new List<Group>();
            try
            {
                connection.Open();
                
                var command = connection.CreateCommand();
                command.CommandText = commandText;
                command.Parameters.AddWithValue("@id", idFaculty);
                
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    groupsList.Add(new Group()
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        IdFaculty = (int)reader["Id_Faculty"]
                    });
                }
                reader.Close();
            }
            finally
            {
                connection.Close();
            }

            return groupsList;
        }

        public List<Student> GetStudents(int idGroup)
        {
            string commandText = "SELECT * FROM Students WHERE Id_Group = @id";
            List<Student> studentsList = new List<Student>();
            try
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = commandText;
                command.Parameters.AddWithValue("@id", idGroup);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    studentsList.Add(new Student()
                    {
                        Id = (int)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        IdGroup = (int)reader["Id_Group"]
                    });
                }
                reader.Close();
            }
            finally
            {
                connection.Close();
            }

            return studentsList;
        }

        public int GetCountOfStudents(int idGroup)
        {
            try
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) from Students  WHERE Id_Group = @id_group";
                command.Parameters.AddWithValue("@id_group", idGroup);
               
                object count = command.ExecuteScalar ();
                return (int)count;
            }
            finally
            {
                connection.Close();
            }
            //return 0;
        }
    }
}
