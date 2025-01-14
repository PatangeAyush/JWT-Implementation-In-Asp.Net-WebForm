using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Employee.DTOs;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Employee.DALs
{
    public class EmployeeDAL
    {
        private const string SecretKey = "R2z5UdTn9XmKsd3jN9P2QaH4FjWp0uLg1n5W0A5n5Mw";
        private const int TokenExpiryMinutes = 30;

        string GetSqlConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public List<EmployeeDTO> GetEmployees()
        {
            using (SqlConnection con = new SqlConnection(GetSqlConnection))
            {
                using (SqlCommand cmd = new SqlCommand("Select * from RegisterDetails", con))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    List<EmployeeDTO> EmpList = new List<EmployeeDTO>();

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        EmpList.Add(new EmployeeDTO
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = dr["Name"].ToString(),
                            Email = dr["Email"].ToString(),
                            Emp_Photo = dr["Emp_Photo"].ToString(),
                            MobileNumber = dr["MobileNUmber"].ToString()
                        });
                    }
                    return EmpList;
                }
            }
        }

        public List<EmployeeDTO> GetEmployeeByID(int id)
        {
            using (SqlConnection con = new SqlConnection(GetSqlConnection))
            {
                using (SqlCommand cmd = new SqlCommand("Select * from RegisterDetails Where Id =@id", con))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("@id", id);

                    List<EmployeeDTO> EmpList = new List<EmployeeDTO>();

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);

                    Console.WriteLine($"Number of Rows:{dt.Rows.Count}");

                    if (dt.Rows.Count >= 1)
                    {
                        DataRow dr = dt.Rows[0];

                        EmployeeDTO employee = new EmployeeDTO
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = dr["Name"].ToString(),
                            Email = dr["Email"].ToString(),
                            Emp_Photo = dr["Emp_Photo"].ToString(),
                            MobileNumber = dr["MobileNumber"].ToString()
                        };

                        EmpList.Add(employee);
                    }
                    return EmpList;
                }
            }
        }

        public bool AddEmployee(EmployeeDTO add)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetSqlConnection))
                {
                    using (SqlCommand cmd = new SqlCommand("AddEmployee_SP", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Name", add.Name);
                        cmd.Parameters.AddWithValue("@Email", add.Email);
                        cmd.Parameters.AddWithValue("@Emp_Photo", add.Emp_Photo);
                        cmd.Parameters.AddWithValue("@Mobile", add.MobileNumber);

                        con.Open();
                        int i = cmd.ExecuteNonQuery();

                        if (i >= 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return false;
            }

        }

        public bool UpdateEmployee(EmployeeDTO update)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetSqlConnection))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateEmployee_SP", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Id", update.Id);
                        cmd.Parameters.AddWithValue("@Name", update.Name);
                        cmd.Parameters.AddWithValue("@Email", update.Email);
                        cmd.Parameters.AddWithValue("@Emp_Photo", update.Emp_Photo);
                        cmd.Parameters.AddWithValue("@Mobile", update.MobileNumber);

                        con.Open();
                        int i = cmd.ExecuteNonQuery();

                        if (i >= 1)
                        {
                            return true;
                        }
                        else { return false; }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error is : " + e);
                return false;
            }
        }

        public bool DeleteEmployee(int id)
        {
            using (SqlConnection con = new SqlConnection(GetSqlConnection))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM RegisterDetails WHERE Id=@Id", con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public static string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: "YourAppName",
                audience: "YourAppUsers",
                claims: claims,
                expires: DateTime.Now.AddMinutes(TokenExpiryMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
