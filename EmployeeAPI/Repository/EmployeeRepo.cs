using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Globalization;
using System.Data.Common;
using EmployeeAPI.Model;

public class EmployeeRepo : IEmployeeRepo
{
    private readonly DBConfig _dBConfig;

    public EmployeeRepo(DBConfig dBConfig)
    {
        this._dBConfig = dBConfig;
    }


    public async Task<int> Create(Employee employee, string action)
    {
        int Id = 0;

        using (var dbConnection = this._dBConfig.CreateSQLServerConnection())
        {
            // Cast IDbConnection to SqlConnection
            if (dbConnection is SqlConnection sqlConnection)
            {
                using (var command = new SqlCommand("sp_EmployeeCRUD", sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
                    command.Parameters.AddWithValue("@Name", employee.Name);
                    command.Parameters.AddWithValue("@DOB", employee.DOB);
                    command.Parameters.AddWithValue("@Age", employee.Age);
                    command.Parameters.AddWithValue("@Designation", employee.Designation);
                    command.Parameters.AddWithValue("@Gender", employee.Gender);
                    command.Parameters.AddWithValue("@Status", employee.Status);
                    command.Parameters.AddWithValue("@ImageURL", employee.ImageURL);
                    command.Parameters.AddWithValue("@action", action);

                    await sqlConnection.OpenAsync();
                    Id = await command.ExecuteNonQueryAsync();
                }
            }
            else
            {
                throw new InvalidOperationException("The database connection is not a SQL Server connection.");
            }
        }

        return Id;
    }



    public async Task<int> Delete(int id, string Action)
    {
        int status = 0;

            using (var dbconnection = this._dBConfig.CreateSQLServerConnection())
            {
              if (dbconnection is SqlConnection sqlConnection)
              {
                using (var command = new SqlCommand("sp_EmployeeCRUD", sqlConnection))
                {
                    int Id = Convert.ToInt32(id);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeID", Id);
                    command.Parameters.AddWithValue("@action", Action);

                    dbconnection.Open();
                    status = await command.ExecuteNonQueryAsync();
                    dbconnection.Close();
                }
              }
              else
              {
                throw new InvalidOperationException("The database connection is not a SQL Server connection.");
              }
            }
       return status;
    }

    public async Task<List<Employee>> GetAll(string Action)
    {
        List<Employee> employeelist = new List<Employee>();

            using (var dbconnection = this._dBConfig.CreateSQLServerConnection())
            {
                 SqlCommand command = new SqlCommand("sp_EmployeeCRUD",(SqlConnection)dbconnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@action", Action);


                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

            dbconnection.Open();
                 sqlDA.Fill(dtProducts);
            dbconnection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                  employeelist.Add(new Employee
                    {
                        EmployeeID = Convert.ToInt32(dr["EmployeeId"]),
                        Name = dr["Name"].ToString(),
                        DOB = Convert.ToDateTime(dr["DOB"]),
                        Age = Convert.ToInt32(dr["Age"]),
                        Designation = Convert.ToString(dr["Designation"]),
                        Gender = dr["Gender"].ToString()[0],
                        Status = Convert.ToBoolean(dr["Status"]),
                        ImageURL = dr["ImageURL"].ToString()
                    });
                }
            }
            return employeelist;
    }

    public async Task<List<Designation>> GetAllRoles()
    {
       List<Designation> rolelist = new List<Designation>();

            using (var dbconnection = this._dBConfig.CreateSQLServerConnection())
            {
              if (dbconnection is SqlConnection sqlConnection)
              {
                using (var command = new SqlCommand("select * from Designations", sqlConnection))
                {
                    command.CommandType = CommandType.Text;
                     SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                    DataTable dtProducts = new DataTable();

                    dbconnection.Open();
                    sqlDA.Fill(dtProducts);
                    dbconnection.Close();

                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        rolelist.Add(new Designation
                        {
                            DesignationsId = Convert.ToInt32(dr["DesignationsId"]),
                            Name = dr["Name"].ToString()
                        });
                    }

                }
                    
              }

                
            }
            return rolelist;
    }

    public async Task<Employee> GetByID(string id, string Action)
    {
        Employee employeee = new Employee();

        using (var dbConnection = this._dBConfig.CreateSQLServerConnection())
        {
            // Cast IDbConnection to SqlConnection
            if (dbConnection is SqlConnection sqlConnection)
            {
                using (var command = new SqlCommand("sp_EmployeeCRUD", sqlConnection))
                {
                    int Id = Convert.ToInt32(id);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeId", Id);
                    command.Parameters.AddWithValue("@Action", Action);

                    await sqlConnection.OpenAsync();

                    using (SqlDataAdapter sqlDA = new SqlDataAdapter(command))
                    {
                        DataTable dtEmployees = new DataTable();
                        sqlDA.Fill(dtEmployees);

                        foreach (DataRow row in dtEmployees.Rows)
                        {
                            Employee employee = new Employee
                            {
                                EmployeeID = Convert.ToInt32(row["EmployeeID"]),
                                Name = row["Name"].ToString(),
                                DOB = DateTime.Parse(row["DOB"].ToString(), CultureInfo.InvariantCulture),
                                Age = Convert.ToInt32(row["Age"]),
                                Designation = row["Designation"].ToString(),
                                Gender = row["Gender"].ToString()[0], // Convert the string to a char
                                Status = Convert.ToBoolean(row["Status"]),
                                ImageURL = row["ImageURL"].ToString()
                            };
                            employeee = employee;
                        }
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("The database connection is not a SQL Server connection.");
            }
        }

        return employeee;
    }




    public async Task<int> Update(Employee employee,string action)
    {
        int Id = 0;
        using (var dbConnection = this._dBConfig.CreateSQLServerConnection())
        {
            // Cast IDbConnection to SqlConnection
            if (dbConnection is SqlConnection sqlConnection)
            {
                using (var command = new SqlCommand("sp_EmployeeCRUD", sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
                    command.Parameters.AddWithValue("@Name", employee.Name);
                    command.Parameters.AddWithValue("@DOB", employee.DOB);
                    command.Parameters.AddWithValue("@Age", employee.Age);
                    command.Parameters.AddWithValue("@Designation", employee.Designation);
                    command.Parameters.AddWithValue("@Gender", employee.Gender);
                    command.Parameters.AddWithValue("@Status", employee.Status);
                    command.Parameters.AddWithValue("@ImageURL", employee.ImageURL);
                    command.Parameters.AddWithValue("@action", action);

                    await sqlConnection.OpenAsync();
                    Id = await command.ExecuteNonQueryAsync();
                }
            }
            else
            {
                throw new InvalidOperationException("The database connection is not a SQL Server connection.");
            }
        }

        return Id;
        
    }

    public async Task<UserInfo> GetUser(string username, string password)
    {
        using (var dbConnection = this._dBConfig.CreateSQLServerConnection())
        {
            if (dbConnection is SqlConnection sqlConnection)
            {
                await sqlConnection.OpenAsync();

                string query = "select * from UserInfo where UserName = @UserName AND Passwword = @Password";
                using (var command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@UserName", username);
                    command.Parameters.AddWithValue("@Password", password);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            UserInfo user = new UserInfo
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Password = reader.GetString(reader.GetOrdinal("Passwword"))
                            };

                            return user;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("The database connection is not a SQL Server connection.");
            }
        }
    }




}