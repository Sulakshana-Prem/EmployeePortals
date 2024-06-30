
using Employee_Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Employee_Application.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> LoginUser(UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {

                    StringContent content = new StringContent(JsonConvert.SerializeObject(userInfo), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync("https://localhost:7163/api/GetToken", content))
                    {
                        string token = await response.Content.ReadAsStringAsync();
                        HttpContext.Session.SetString("JWToken", token);
                    }
                    return Redirect("~/Employee/Index");
                }
                
            }
            return View(nameof(Index), userInfo);

        }
     
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("~/Home/Index");
        }
    }
}
