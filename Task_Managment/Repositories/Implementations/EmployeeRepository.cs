using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories.Implementations
{
    public class EmployeeRepository : EmployeeInterface
    {
        private readonly NpgsqlConnection _conn;

        public EmployeeRepository(NpgsqlConnection connection)
        {
            _conn = connection;
        }
        

        public async Task<ResponseModel<string>> AddEmployee(Employee employee)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                await _conn.OpenAsync();
                
                using (NpgsqlCommand cm = new NpgsqlCommand(@"INSERT INTO public.t_employee (c_name,c_email,c_password,c_role,c_profileimage) VALUES (@c_name,@c_email,@c_password,@c_role,@c_profileimage);", _conn))
                {
                    cm.Parameters.AddWithValue("@c_name", employee.EmpName);
                    cm.Parameters.AddWithValue("@c_email", employee.Email);
                    cm.Parameters.AddWithValue("@c_password", employee.Password);
                    cm.Parameters.AddWithValue("@c_role", "Employee");
                    cm.Parameters.AddWithValue("@c_profileimage", employee.ProfileImage == null ? DBNull.Value : employee.ProfileImage);


                    var result = await cm.ExecuteNonQueryAsync();
                    if (result > 0)
                    {
                        response.success = true;
                        response.message = "Record Inserted Succsessfully.";
                    }
                    else
                    {
                        response.success = false;
                        response.message = "Error occurs while Registration.";
                    }
                }

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.success = false;
            }
            finally
            {
                await _conn.CloseAsync();
            }
            return response;
        }



        public async Task<ResponseModel<string>> DeleteEmployee(int id)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                await _conn.OpenAsync();

                using (NpgsqlCommand cm = new NpgsqlCommand(@"DELETE FROM public.t_employee WHERE c_empid=@c_empid;", _conn))
                {
                    cm.Parameters.AddWithValue("c_empid", id);
                    var result = await cm.ExecuteNonQueryAsync();
                    if (result > 0)
                    {
                        response.success = true;
                        response.message = "Record Deleted Succsessfully.";
                    }
                    else
                    {
                        response.success = false;
                        response.message = "Error occurs while Deletion.";
                    }
                }

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.success = false;
            }
            finally
            {
                await _conn.CloseAsync();
            }
            return response;
        }

      

        public async Task<ResponseModel<List<Employee>>> GetAllEmployee()
        {
            ResponseModel<List<Employee>> response = new ResponseModel<List<Employee>>();
            List<Employee> listEmployee = new List<Employee>();
            try
            {
                await _conn.OpenAsync();

                using (NpgsqlCommand cm = new NpgsqlCommand(@"SELECT c_empid, c_name, c_email, c_password, c_role, c_profileimage FROM public.t_employee;", _conn))
                {
                    NpgsqlDataReader dataReader = await cm.ExecuteReaderAsync();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            listEmployee.Add(new Employee
                            {
                                EmpId = Convert.ToInt32(dataReader["c_empid"]),
                                EmpName = dataReader["c_name"].ToString()!,
                                Email = dataReader["c_email"].ToString()!,
                                Password = dataReader["c_password"].ToString()!,
                                Role = dataReader["c_role"].ToString()!,
                                ProfileImage = dataReader["c_profileimage"].ToString()!,
                            });
                            response.message = "";
                            response.success = true;
                            response.data = listEmployee;
                        }
                    }
                    else
                    {
                        response.message = "No records found";
                        response.success = false;
                    }
                }

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.success = false;
            }
            finally
            {
                await _conn.CloseAsync();
            }
            return response;
        }

        public async Task<ResponseModel<Employee>> GetEmployeeById(int id)
        {
            ResponseModel<Employee> response = new ResponseModel<Employee>();
            Employee employee = new Employee();
            try
            {
                await _conn.OpenAsync();

                using (NpgsqlCommand cm = new NpgsqlCommand(@"SELECT c_empid, c_name, c_email, c_password, c_role, c_profileimage FROM public.t_employee WHERE c_empid=@c_empid;", _conn))
                {
                    cm.Parameters.AddWithValue("c_empid", id);
                    NpgsqlDataReader dataReader = await cm.ExecuteReaderAsync();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {

                            employee.EmpId = Convert.ToInt32(dataReader["c_empid"]);
                            employee.EmpName = dataReader["c_name"].ToString()!;
                            employee.Email = dataReader["c_email"].ToString()!;
                            employee.Password = dataReader["c_password"].ToString()!;
                            employee.Role = dataReader["c_role"].ToString()!;
                            employee.ProfileImage = dataReader["c_profileimage"].ToString()!;
                        }
                        response.message = "";
                        response.success = true;
                        response.data = employee;


                    }
                    else
                    {
                        response.message = "No record found";
                        response.success = false;
                    }
                }

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.success = false;
            }
            finally
            {
                await _conn.CloseAsync();
            }
            return response;

        }

        public async Task<ResponseModel<Employee>> Login(Login login)
        {
            ResponseModel<Employee> response = new ResponseModel<Employee>();
            Employee employee = new Employee();
            try
            {
                await _conn.OpenAsync();
                using (NpgsqlCommand cm = new NpgsqlCommand(@"SELECT * FROM t_employee WHERE c_email=@c_email AND c_password=@c_password;", _conn))
                {
                    cm.Parameters.AddWithValue("@c_email", login.Email);
                    cm.Parameters.AddWithValue("@c_password", login.Password);

                    NpgsqlDataReader dataReader = await cm.ExecuteReaderAsync();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            employee.EmpId = Convert.ToInt32(dataReader["c_empid"]);
                            employee.EmpName = dataReader["c_name"]?.ToString()!;
                            employee.Email = dataReader["c_email"]?.ToString()!;
                            employee.Password = dataReader["c_password"]?.ToString()!;
                            employee.Role = dataReader["c_role"]?.ToString()!;
                            employee.ProfileImage = dataReader["c_profileimage"]?.ToString();
                        }
                        response.message = "";
                        response.success = true;
                        response.data = employee;
                    }
                    else
                    {
                        response.message = "No record found";
                        response.success = false;
                    }

                }

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.success = false;
            }
            finally
            {
                await _conn.CloseAsync();
            }
            return response;
            // throw new NotImplementedException();
        }

        public async Task<ResponseModel<string>> UpdateEmployee(Employee employee)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                await _conn.OpenAsync();
                using (NpgsqlCommand cm = new NpgsqlCommand(@"UPDATE public.t_employee SET c_name=@c_name, c_email=@c_email, c_password=@c_password, c_role=@c_role, c_profileimage=@c_profileimage WHERE c_empid=@c_empid;", _conn))
                {
                    cm.Parameters.AddWithValue("@c_empid", employee.EmpId);
                    cm.Parameters.AddWithValue("@c_name", employee.EmpName);
                    cm.Parameters.AddWithValue("@c_email", employee.Email);
                    cm.Parameters.AddWithValue("@c_password", employee.Password);
                    cm.Parameters.AddWithValue("@c_role", employee.Role!);
                    cm.Parameters.AddWithValue("@c_profileimage", employee.ProfileImage == null ? DBNull.Value : employee.ProfileImage);


                    var result = await cm.ExecuteNonQueryAsync();
                    if (result > 0)
                    {
                        response.success = true;
                        response.message = "Record Updated Succsessfully.";
                    }
                    else
                    {
                        response.success = false;
                        response.message = "Somthing Went Wrong Here.";
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                await _conn.CloseAsync();
            }

        }
    }
}