using Employee_Application.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DotNet.MSIdentity.Shared;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NuGet.Common;
using System.Data;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;

namespace Employee_Application.Controllers
{
    public class EmployeeController : Controller
    {
        IWebHostEnvironment _webhost;

        public EmployeeController(IWebHostEnvironment webhost)
        {
            _webhost = webhost;
        }
        public async Task<IActionResult> Index()
        {
            string errmessage = "";
            List<Employee> employee = new List<Employee>();
            try
            {       
                var accessToken = HttpContext.Session.GetString("JWToken");
                using (var httpClient = new HttpClient())
                {
                    var baseUri = new Uri("https://localhost:7163/api/GetAll");

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    httpClient.DefaultRequestHeaders.Add("Action", "null");

                    HttpResponseMessage response = await httpClient.GetAsync(baseUri);

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<List<Employee>>(jsonResponse);     
                }
            }
            catch(Exception ex)
            {
                errmessage = ex.Message;
            }

            if(errmessage != "")
            {
                TempData["ErrorMessage"] = "There was an error while retreiving the data.";
            }           
            TempData["SuccessMessage"] = "Employee data Retreived successfully.";
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> AddEmployee()
        {
            ViewBag.Designation = await FetchDesignations();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {

                if (employee.Designation == "1")
                {
                    employee.Designation = "Develop";
                }
                else if (employee.Designation == "2")
                {
                    employee.Designation = "Testing";
                }
                else if (employee.Designation == "3")
                {
                    employee.Designation = "Support";
                }
                else
                {
                    employee.Designation = "BusinessAdmin";
                }

                // Handle image upload if Image64 is provided
                if (employee.Image64 != null)
                {
                    string folder = "images/";
                    employee.ImageURL = await UploadImage(folder, employee.Image64);
                }

                // Post employee data to API using HttpClient
                var accessToken = HttpContext.Session.GetString("JWToken");
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    httpClient.DefaultRequestHeaders.Add("Action", "CREATE");

                    StringContent content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync("https://localhost:7163/api/Create", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Employee details added successfully.";
                        return View(); 
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "There was an error adding the employee.";
                    }
                }
            }
            ViewBag.Designation = await FetchDesignations();
            return View(nameof(AddEmployee), employee);
        }


        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_webhost.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folderPath;
        }
       
        [HttpGet]
        public async Task<IActionResult> EditEmployee(int Id)
        {
            string errmessage = string.Empty;
            Employee employee = null;
            try
            {
                var accessToken = HttpContext.Session.GetString("JWToken");

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Action", "READ");
                    httpClient.DefaultRequestHeaders.Add("Id", Convert.ToString(Id));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7163/api/GetbyId");

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<Employee>(jsonResponse);
                   
               }

            }
            catch (Exception ex)
            {
                errmessage = ex.Message;
            }
            if (errmessage != "")
            {
                TempData["ErrorMessage"] = "There was an error while retreiving the data.";
            }
            ViewBag.Designation = await FetchDesignations();

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(Employee employee)
        {
            string errmessage = string.Empty;
            if (ModelState.IsValid)
            {
                var accessToken = HttpContext.Session.GetString("JWToken");

                if (employee.Designation == "1")
                {
                    employee.Designation = "Develop";
                }
                else if (employee.Designation == "2")
                {
                    employee.Designation = "Testing";
                }
                else if (employee.Designation == "3")
                {
                    employee.Designation = "Support";
                }
                else
                {
                    employee.Designation = "BusinessAdmin";
                }

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Action", "UPDATE");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    StringContent content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync("https://localhost:7163/api/Update", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Employee details updated successfully.";
                        return View();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "There was an error adding the employee.";
                    }

                }

            }
            ViewBag.Designation = await FetchDesignations();
            return View(nameof(EditEmployee), employee);
           
        }

        [HttpGet]
        public async Task<IActionResult> RemoveEmployee(int? Id)
        {
            string errmessage = string.Empty;
            Employee employee = null;
            try
            {
                var accessToken = HttpContext.Session.GetString("JWToken");

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Action", "READ");
                    httpClient.DefaultRequestHeaders.Add("Id", Convert.ToString(Id));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7163/api/GetbyId");

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<Employee>(jsonResponse);
                   

                }

            }
            catch (Exception ex)
            {
                errmessage = ex.Message;
            }
            if (errmessage != "")
            {
                TempData["ErrorMessage"] = "There was an error while retreiving the data.";
            }

            return View(employee);
        }


        [HttpPost]
        public async Task<IActionResult> RemoveEmployee(int Id)
        {
            string errmessage = string.Empty;
            try
            {
                var accessToken = HttpContext.Session.GetString("JWToken");

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Action", "DELETE");
                    httpClient.DefaultRequestHeaders.Add("Id", Convert.ToString(Id));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    StringContent content = new StringContent(JsonConvert.SerializeObject(Id), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync("https://localhost:7163/api/Remove", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Employee details deleted successfully.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "There was an error adding the employee.";
                    }

                }
            }
            catch(Exception ex)
            {
                errmessage = ex.Message;
            }
            if (errmessage != "")
            {
                TempData["ErrorMessage"] = "There was an error while retreiving the data.";
            }

            return View(nameof(Index));
        }


        public async Task<IActionResult> ViewEmployee(int Id)
        {
            string errmessage = string.Empty;
            Employee employee = null;
            try
            {
                var accessToken = HttpContext.Session.GetString("JWToken");

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Action", "READ");
                    httpClient.DefaultRequestHeaders.Add("Id", Convert.ToString(Id));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7163/api/GetbyId");

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<Employee>(jsonResponse);

                }

            }
            catch (Exception ex)
            {
                errmessage = ex.Message;
            }
            if (errmessage != "")
            {
                TempData["ErrorMessage"] = "There was an error while retreiving the data.";
            }

            return View(employee);
        }

        public async Task<IEnumerable<SelectListItem>> FetchDesignations()
        {
            IEnumerable<SelectListItem> designations = new List<SelectListItem>();

            try
            {
                var accessToken = HttpContext.Session.GetString("JWToken");

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7163/api/GetRole");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var roles = JsonConvert.DeserializeObject<IEnumerable<Designation>>(jsonResponse);

                        designations = roles.Select(x => new SelectListItem
                        {
                            Text = x.Name,
                            Value = x.DesignationsId.ToString()
                        }).ToList();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Failed to retrieve designations from API.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
            }

            return designations;
        }

       
    }
}
