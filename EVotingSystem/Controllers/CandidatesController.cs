using EVotingSystem.DataBase;
using EVotingSystem.Helpers;
using EVotingSystem.Models;
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
            List<CandidateModel> Candidates = FireStore.GetAllCandidates().Result;
            foreach (CandidateModel C in Candidates)
            {
                C.DecryptProperties();
            }
            return View(Candidates.ToArray());
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
            //string[] Elements = Request.Form[Candidate.Id].ToArray();

            //Handle Voting/UnVoting
            if (this.Candidate.ApplyVote(Identity.StudentSession(), Candidate))
            {
                //Handle Success-Popup Message
            }
            else
            {
                //Handle Error-Popup Message
            }
            //Redirect to the candidate view after handling the voting/unvoting.
            return RedirectToAction("Index", "Candidates");
        }
        #endregion
    }
}