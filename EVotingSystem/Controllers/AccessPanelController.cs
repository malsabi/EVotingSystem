using EVotingSystem.DataBase;
using EVotingSystem.Models;
using EVotingSystem.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace EVotingSystem.Controllers
{
    public class AccessPanelController : Controller
    {
        #region "Fields"
        private readonly FireStoreManager FireStore;
        private readonly IdentityHandler Identity;
        #endregion

        public AccessPanelController()
        {
            FireStore = new FireStoreManager();
            Identity = new IdentityHandler(this);
        }

        #region "GET"
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region "POST"
        [HttpPost]
        public IActionResult Index(AccessPanelModel AccessPanel)
        {
            return View();
        }
        #endregion
    }
}