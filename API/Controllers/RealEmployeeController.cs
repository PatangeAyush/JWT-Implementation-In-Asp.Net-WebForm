using Employee.BALs;
using Employee.DTOs;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{

    [RoutePrefix("api/RealEmployee")]
    public class RealEmployeeController : ApiController
    {
        private readonly EmployeeBAL bal = new EmployeeBAL();
   
        [Route("Login")]
        [HttpPost]
        public IHttpActionResult Login(LoginDTO login)
        {
            
            if (login.username != null && login.password != null)
            {               
                string token = EmployeeBAL.GenerateJwtToken(login.username);
                
                return Ok(new { Token = token });
            }
            else
            {
                // Invalid credentials ka response
                return Unauthorized();
            }
        }
        [Authorize]
        [HttpPost]
        [Route("Get")]
        public IHttpActionResult Get()
        {
            var employees = bal.GetEmployees();
            return Ok(employees);
        }
        [Authorize]
        [HttpPost]
        [Route("Get/{id}")]
        public IHttpActionResult Get(int id)
        {
            var employees = bal.GetEmployees().Find(model => model.Id == id);
            if(employees == null) return NotFound();
            return Ok(employees);
        }
        [Authorize]
        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add(EmployeeDTO add)
        {
            if (bal.AddEmployee(add))
            {
                return Ok("Employee Added Succesfully");
            }
            return BadRequest("Failed To Add Employee");
        }
        [Authorize]
        [HttpPost]
        [Route("Update/{id}")]
        public IHttpActionResult Update(int id, EmployeeDTO update)
        {
            update.Id = id; 
            if (bal.UpdateEmployee(update))
                return Ok("Employee updated successfully!");

            return BadRequest("Failed to update employee.");
        }
        [Authorize]
        [HttpPost]
        [Route("Delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (bal.DeleteEmployee(id))
                return Ok("Employee deleted successfully!");

            return BadRequest("Failed to delete employee.");
        }
    }
}
