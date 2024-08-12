using ConnectedMode.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedMode.Providers
{
    internal class SqlDbProvider : IDbProvider
    {
        private readonly string connectionString = null;
        private readonly SqlConnection connection = null;

        public SqlDbProvider()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = @"Alex",
                InitialCatalog = "Library2",
                IntegratedSecurity = true
            };

            connectionString = builder.ConnectionString;
            connection = new SqlConnection(connectionString);
        }

        public void Dispose()
        {
            connection.Close();
        }

        public async Task<List<Faculty>> GetFacultiesAsync()
        {
            string commandText = "SELECT * FROM Faculties";
            List<Faculty> facultiesList = new List<Faculty>();

            try
            {
                //await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    using (var reader = await command.ExecuteReaderAsync())
                    { 
                        while (await reader.ReadAsync())
                        {
                            facultiesList.Add(new Faculty()
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"]
                            });
                        }
                    }
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return facultiesList;
        }

        public async Task<List<Group>> GetGroupsAsync(int idFaculty)
        {
            string commandText = "SELECT * FROM Groups WHERE Id_Faculty = @id";
            List<Group> groupsList = new List<Group>();
            
            try
            {
                //await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    await Task.Run(() => command.Parameters.AddWithValue("@id", idFaculty));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            groupsList.Add(new Group()
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                IdFaculty = (int)reader["Id_Faculty"]
                            });
                        }
                    }
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return groupsList;
        }

        public async Task<List<Student>> GetStudentsAsync(int idGroup)
        {
            string commandText = "SELECT * FROM Students WHERE Id_Group = @id";
            List<Student> studentsList = new List<Student>();
            try
            {
                //await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    await Task.Run(() => command.Parameters.AddWithValue("@id", idGroup));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            studentsList.Add(new Student()
                            {
                                Id = (int)reader["Id"],
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                IdGroup = (int)reader["Id_Group"]
                            });
                        }
                    }
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return studentsList;
        }

        public async Task<int> GetCountOfStudentsAsync(int idGroup)
        {
            try
            {
                //await connection.OpenAsync();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) from Students  WHERE Id_Group = @id_group";
                    await Task.Run(() => command.Parameters.AddWithValue("@id_group", idGroup));

                    object count = await command.ExecuteScalarAsync();
                    return (int)count;
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            //return 0;
        }
    }
}
