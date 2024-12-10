using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Npgsql;
using repositories.Interfaces;
using repositories.Models;

namespace repositories.Implementations
{
    public class UserRepositories : IUserInterface
    {
        private readonly NpgsqlConnection _conn;
        public UserRepositories(NpgsqlConnection connection)
        {
            _conn = connection;
        }
         public async Task<int> Register(t_User data)
        {
            int status = 0;
            try
            {
                await _conn.CloseAsync();
                NpgsqlCommand comcheck = new NpgsqlCommand(@"SELECT * FROM t_user WHERE c_email = @c_email ;", _conn);
                comcheck.Parameters.AddWithValue("@c_email", data.Email);
                await _conn.OpenAsync();
                using (NpgsqlDataReader datadr = await comcheck.ExecuteReaderAsync())
                {
                    if(datadr.HasRows)
                    {
                        await _conn.CloseAsync();
                        return 0;
                    }
                    else 
                    {
                        await _conn.CloseAsync();
                        NpgsqlCommand com = new NpgsqlCommand(@"INSERT INTO t_user (c_username,c_email,c_password,c_address,c_gender,c_mobile,c_image) VALUES (@c_userName,@c_email,@c_password,@c_address,@c_gender,@c_mobile,@c_image)", _conn);
                        com.Parameters.AddWithValue("@c_userName", data.UserName);
                        com.Parameters.AddWithValue("@c_email", data.Email);
                        com.Parameters.AddWithValue("@c_password", data.Password);
                        com.Parameters.AddWithValue("@c_address", data.Address);
                        com.Parameters.AddWithValue("@c_gender", data.Gender);
                        com.Parameters.AddWithValue("@c_mobile", data.Mobile);
                        com.Parameters.AddWithValue("@c_image", data.Image);
                        await _conn.OpenAsync();
                        await com.ExecuteNonQueryAsync();
                        await _conn.CloseAsync();
                        return 1;
                    }

                }

            }
            catch (Exception e)
            {
                await _conn.CloseAsync();
                Console.WriteLine("Register Failed , Error :-"+e.Message);
                return -1;
            }
            //throw new NotImplementedException();
        }
        public async Task<t_User> Login(vm_Login user)
        {
            t_User UserData = new t_User();
            var qry = "SELECT * FROM t_user WHERE c_email=@c_email AND c_password=@c_password;";
            try
            {
                using(NpgsqlCommand cmd = new NpgsqlCommand(qry, _conn))
                {
                    cmd.Parameters.AddWithValue("@c_email", user.Email);
                    cmd.Parameters.AddWithValue("@c_password", user.Password);
                    await _conn.OpenAsync();
                    var reader = await cmd.ExecuteReaderAsync();
                    if(reader.Read())
                    {
                        UserData.UserId = (int)reader["c_userid"];
                        UserData.UserName = (string)reader["c_username"];
                        UserData.Email = (string)reader["c_email"];
                        UserData.Gender = (string)reader["c_gender"];
                        UserData.Mobile = (string)reader["c_mobile"];
                        UserData.Address =  (string)reader["c_address"];
                        UserData.Image = (string)reader["c_image"];

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Login Error , Error :" + e.Message);
            }
            finally
            {
                await _conn.CloseAsync();
            }
            return UserData;
            //throw new NotImplementedException();
        }

       
    }
}