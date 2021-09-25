using EVotingSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace EVotingSystem.Controllers
{
    public class SignUpController : Controller
    {
        #region "GET"
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region "POST"
        [HttpPost]
        public IActionResult Index(SignUpModel model)
        {
            return View(model);
        }
        #endregion
    }
}