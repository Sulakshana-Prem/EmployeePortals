using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeAPIController : ControllerBase
    {
        private readonly IEmployeeRepo _repo;

        public EmployeeAPIController(IEmployeeRepo repo)
        {
            this._repo = repo;
        }

        [Route("/api/GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromHeader] string Action)
        {
            List<Employee> employees = new List<Employee>();
            string errmessage="";
            try
            {
                 employees = await this._repo.GetAll(Action);
            }
            catch(Exception ex)
            {
                errmessage = ex.Message;
            }
            if(errmessage == "" )
            {
                return Ok(employees);
            }
            return BadRequest(errmessage);
        }

        [Route("/api/GetbyId")]
        [HttpGet]
        public async Task<IActionResult> GetbyId([FromHeader] string id, [FromHeader] string Action)
        {
            Employee employee = new Employee(); 
            string errmessage = "";
            try
            {
                 employee = await this._repo.GetByID(id, Action);
            }
            catch(Exception ex)
            {
                errmessage = ex.Message;
            }

            if (errmessage == "")
            {
                return Ok(employee);
            }
            return BadRequest(errmessage);
        }

        [Route("/api/Remove")]
        [HttpPost]
        public async Task<IActionResult> Remove([FromHeader] int id, [FromHeader] string Action)
        {
            int status = 0;
            string errmessage="";
            try
            {
                 status = await this._repo.Delete(id, Action);
            }
            catch (Exception ex)
            {
                errmessage = ex.Message;
            }

            if (status != 0)
            {
                return Ok(true);
            }

            return BadRequest(errmessage);



        }

        //[Authorize(Roles ="admin")]
        [Route("/api/Create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Employee employee, [FromHeader] string action)
        {
            int status = 0;
            string errmessage = "";
            try
            {
                status = await this._repo.Create(employee, action);
            }
            catch (Exception ex)
            {
                errmessage = ex.Message;
            }

            if (status != 0)
            {
                return Ok(true);
            }

            return BadRequest(errmessage);
        }

        [Route("/api/Update")]
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Employee employee, [FromHeader]string action)
        {
            int status = 0;
            string errmessage = "";
            try
            {
                status = await this._repo.Update(employee, action);
            }
            catch (Exception ex)
            {
                errmessage = ex.Message;
            }

            if (status != 0)
            {
                return Ok(true);
            }

            return BadRequest(errmessage);
        }

        [Route("/api/GetRole")]
        [HttpGet]
        public async Task<IActionResult> GetRole()
        {
           
            List<Designation> roles = new List<Designation>();
            string errmessage = "";
            try
            {
                roles = await this._repo.GetAllRoles();
            }
            catch (Exception ex)
            {
                errmessage = ex.Message;
            }
            if (errmessage == "")
            {
                return Ok(roles);
            }
            return BadRequest(errmessage);
        }

    }
}
