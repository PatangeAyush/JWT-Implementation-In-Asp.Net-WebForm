using Employee.DALs;
using Employee.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.BALs
{
    public class EmployeeBAL
    {
        private readonly EmployeeDAL dal = new EmployeeDAL();
        public List<EmployeeDTO> GetEmployees()
        {
            return dal.GetEmployees();
        }

        public List<EmployeeDTO> GetEmployeeByID(int id)
        {
            return dal.GetEmployeeByID(id);
        }

        public bool AddEmployee(EmployeeDTO add)
        {
           return dal.AddEmployee(add);
        }

        public bool UpdateEmployee(EmployeeDTO update)
        {
            return dal.UpdateEmployee(update);
        }

        public bool DeleteEmployee(int id)
        {
            return dal.DeleteEmployee(id);
        }

        public static string GenerateJwtToken(string username)
        {
            return EmployeeDAL.GenerateJwtToken(username);
        }
    }
}
