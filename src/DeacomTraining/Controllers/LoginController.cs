using DeacomTraining.Service;
using Microsoft.AspNetCore.Mvc;

namespace DeacomTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// Loads all necessary data to be used all along the system life
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult Login()
        {
            LoginService service = new LoginService();
            bool success = service.Load();
            string welcomeMessage = string.Empty;
            if (success)
                welcomeMessage = "Welcome, all data has been loaded succesfully";
            else
                welcomeMessage = "Sorry! an error has ocurred and we are not able to show the report";
            var response = new
            {
                welcomeMessage = welcomeMessage
            };
            return Ok(response);
        }
    }
}
