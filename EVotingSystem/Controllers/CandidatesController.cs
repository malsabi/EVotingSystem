using EVotingSystem.DataBase;
using EVotingSystem.Models;
using EVotingSystem.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace EVotingSystem.Controllers
{
    public class CandidatesController : Controller
    {
        #region "Fields"
        private readonly FireStoreManager FireStore;
        private readonly IdentityHandler Identity;
        private readonly CandidateHandler Candidate;
        #endregion

        public CandidatesController()
        {
            FireStore = new FireStoreManager();
            Identity = new IdentityHandler(this);
            Candidate = new CandidateHandler();
        }

        #region "GET"
        [HttpGet]
        public IActionResult Index()
        {
            //Get the updated candidates from the database.
            return View(FireStore.GetAllCandidates().Result.ToArray());
        }
        #endregion

        #region "POST"
        [HttpPost]
        public IActionResult Index(string Id)
        {
            if (Identity.IsUserLoggedIn())
            {
                //Get the candidate from the Id
                CandidateModel Candidate = FireStore.GetCandidate(Id).Result;
                //Pass the specified candidate to the vote view.
                return View("Vote", Candidate);
            }
            else
            {
                //Redirect to login page if the user is not logged in.
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult Attempt(CandidateModel Candidate)
        {
            //Handle Voting/UnVoting
            this.Candidate.ApplyVote(Identity.StudentSession(), Candidate);
            //Redirect to the candidate view after handling the voting/unvoting.
            return RedirectToAction("Index", "Candidates");
        }
        #endregion
    }
}