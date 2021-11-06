using EVotingSystem.DataBase;
using EVotingSystem.Helpers;
using EVotingSystem.Models.Candidate;
using EVotingSystem.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
            if (Identity.IsAdminLoggedOut())
            {
                List<CandidateModel> Candidates = FireStore.GetAllCandidates(true).Result;
                return View(Candidates.ToArray());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        #region "POST"
        [HttpPost]
        public IActionResult Index(string Id)
        {
            if (Identity.IsStudentLoggedIn())
            {
                //Get the candidate from the Id
                CandidateModel Candidate = FireStore.GetCandidate(CandidateHelper.EncryptField(Id)).Result;
                //Decrypt the candidate fields to show them for the student.
                Candidate.DecryptProperties();
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
            this.Candidate.ApplyVote(Identity.StudentSession(), Candidate);
            //Redirect to the candidate view after handling the voting.
            return RedirectToAction("Index", "Candidates");
        }
        #endregion
    }
}