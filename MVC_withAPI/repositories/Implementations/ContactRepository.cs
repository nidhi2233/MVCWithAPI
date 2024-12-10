using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using repositories.Interfaces;
using repositories.Models;


namespace repositories.Implementations
{
    public class ContactRepository : IContactInterface
    {
        private readonly NpgsqlConnection _conn;
        public ContactRepository(NpgsqlConnection connection)
        {
            _conn = connection;
        }

        public async Task<int> Add(t_Contact data)
        {
            try
            {
                await _conn.CloseAsync();
                NpgsqlCommand cm = new NpgsqlCommand(@"INSERT INTO t_contact (c_userid,c_contactname,c_email,c_mobile,c_address,c_image,c_status,c_group) VALUES (@c_userid,@c_contactname,@c_email,@c_mobile,@c_address,@c_image,@c_status,@c_group)", _conn);
                cm.Parameters.AddWithValue("@c_userid", data.UserId);
                cm.Parameters.AddWithValue("@c_contactname", data.ContactName);
                cm.Parameters.AddWithValue("@c_email", data.Email);
                cm.Parameters.AddWithValue("@c_mobile", data.Mobile);
                cm.Parameters.AddWithValue("@c_address", data.Address);
                cm.Parameters.AddWithValue("@c_image", data.Image == null ? DBNull.Value : data.Image);
                cm.Parameters.AddWithValue("@c_status", data.Status);
                cm.Parameters.AddWithValue("@c_group", data.Group);

                await _conn.OpenAsync();
                await cm.ExecuteNonQueryAsync();
                await _conn.CloseAsync();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error on Add Contact : " + ex.Message);
                return 0;
            }
        }

        public async Task<int> Delete(string contactid)
        {
            try
            {
                NpgsqlCommand cm = new NpgsqlCommand(@"DELETE FROM t_contact WHERE c_contactid=@c_contactid", _conn);
                cm.Parameters.AddWithValue("@c_contactid", int.Parse(contactid));
                await _conn.CloseAsync();
                await _conn.OpenAsync();
                await cm.ExecuteNonQueryAsync();
                await _conn.CloseAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }



            //throw new NotImplementedException();
        }

        public async Task<List<t_Contact>> GetAll()
        {
            DataTable dt = new DataTable();
            NpgsqlCommand cm = new NpgsqlCommand("Select * from t_contact", _conn);
            await _conn.CloseAsync();
            await _conn.OpenAsync();
            NpgsqlDataReader datar = cm.ExecuteReader();
            if (datar.HasRows)
            {
                dt.Load(datar);
            }
            List<t_Contact> contactList = new List<t_Contact>();
            contactList = (from DataRow dr in dt.Rows
                           select new t_Contact()
                           {
                               ContactId = Convert.ToInt32(dr["c_contactid"]),
                               UserId = int.Parse(dr["c_userid"].ToString()),
                               ContactName = dr["c_contactname"].ToString(),
                               Email = dr["c_email"].ToString(),
                               Mobile = dr["c_mobile"].ToString(),
                               Address = dr["c_address"].ToString(),
                               Image = dr["c_image"].ToString(),
                               Group = dr["c_group"].ToString(),
                               Status = dr["c_status"].ToString(),
                           }).ToList();
            await _conn.CloseAsync();
            return contactList;
            //throw new NotImplementedException();
        }

        public async Task<List<t_Contact>> GetAllByUser(string userid)
        {
            DataTable dt = new DataTable();
            NpgsqlCommand cm = new NpgsqlCommand("select * from t_contact", _conn);
            await _conn.CloseAsync();
            await _conn.OpenAsync();
            NpgsqlDataReader datar = cm.ExecuteReader();
            if (datar.HasRows)
            {
                dt.Load(datar);
            }
            List<t_Contact> contactList = new List<t_Contact>();
            contactList = (from DataRow dr in dt.Rows
                           where dr["c_userid"].ToString() == userid
                           select new t_Contact()
                           {
                               ContactId = Convert.ToInt32(dr["c_contactid"]),
                               UserId = int.Parse(dr["user_id"].ToString()),
                               ContactName = dr["c_contactname"].ToString(),
                               Email = dr["c_email"].ToString(),
                               Mobile = dr["c_mobile"].ToString(),
                               Address = dr["c_address"].ToString(),
                               Image = dr["c_image"].ToString(),
                               Group = dr["c_group"].ToString(),
                               Status = dr["c_status"].ToString(),
                           }).ToList();
            await _conn.CloseAsync();
            return contactList;
            //throw new NotImplementedException();
        }

        public async Task<t_Contact> GetOne(string contactid)
        {
            DataTable dt = new DataTable();
            NpgsqlCommand cm = new NpgsqlCommand("select * from t_contact WHERE c_contactid=@c_contactid", _conn);
            cm.Parameters.AddWithValue("@c_contactid", int.Parse(contactid));
            await _conn.CloseAsync();
            await _conn.OpenAsync();
            NpgsqlDataReader datar = cm.ExecuteReader();
            t_Contact contact = null;
            if (datar.Read())
            {
                contact = new t_Contact()
                {
                    ContactId = Convert.ToInt32(datar["c_contactid"]),
                    UserId = int.Parse(datar["c_userid"].ToString()),
                    ContactName = datar["c_contactname"].ToString(),
                    Email = datar["c_email"].ToString(),
                    Mobile = datar["c_mobile"].ToString(),
                    Address = datar["c_address"].ToString(),
                    Image = datar["c_image"].ToString(),
                    Group = datar["c_group"].ToString(),
                    Status = datar["c_status"].ToString()
                };
            }
            await _conn.CloseAsync();
            return contact;
            //throw new NotImplementedException();
        }

        public async Task<int> Update(t_Contact data)
        {
            try
            {
                NpgsqlCommand cm = new NpgsqlCommand(@"UPDATE t_contact set c_userid=@c_userid,c_contactname=@c_contactname,c_email=@c_email,c_mobile=@c_mobile, c_address=@c_address,c_image=@c_image, c_status=@c_status,c_group=@c_group WHERE c_contactid=@c_contactid", _conn);
                cm.Parameters.AddWithValue("@c_userid", data.UserId);
                cm.Parameters.AddWithValue("@c_contactname", data.ContactName);
                cm.Parameters.AddWithValue("@c_email", data.Email);
                cm.Parameters.AddWithValue("@c_mobile", data.Mobile);
                cm.Parameters.AddWithValue("@c_address", data.Address);
                cm.Parameters.AddWithValue("@c_image", data.Image == null ? DBNull.Value : data.Image);
                cm.Parameters.AddWithValue("@c_status", data.Status);
                cm.Parameters.AddWithValue("@c_group", data.Group);
                cm.Parameters.AddWithValue("@c_contactid", data.ContactId);
                await _conn.CloseAsync();
                await _conn.OpenAsync();
                await cm.ExecuteNonQueryAsync();
                await _conn.CloseAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
            //throw new NotImplementedException();
        }


    }
}