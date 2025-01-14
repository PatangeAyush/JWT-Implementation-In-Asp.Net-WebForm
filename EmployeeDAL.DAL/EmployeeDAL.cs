using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Employee.DTOs;

namespace EmployeeDAL.DAL
{
    public class EmployeeDAL
    {
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


    }
}
