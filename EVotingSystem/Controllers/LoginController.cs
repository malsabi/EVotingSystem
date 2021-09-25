using EVotingSystem.Models;
using Microsoft.AspNetCore.Mvc;


namespace EVotingSystem.Controllers
{
    public class LoginController : Controller
    {
        #region "GET"
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Recover()
        {
            return View();
        }
        #endregion

        #region "POST"
        [HttpPost]
        public IActionResult Index(LoginModel model)
        {
            return View(model);
        }
        [HttpPost]
        public IActionResult Recover(RecoverModel model)
        {
            return View(model);
        }
        #endregion
    }
}