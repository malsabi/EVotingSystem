using EVotingSystem.DataBase;
using EVotingSystem.Helpers;
using EVotingSystem.Models.Candidate;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EVotingSystem.Controllers
{
    public class ResultsController : Controller
    {
        #region "Fields"
        private readonly FireStoreManager FireStore;
        #endregion

        public ResultsController()
        {
            FireStore = new FireStoreManager();
        }

        #region "GET"
        [HttpGet]
        public IActionResult Index()
        {
            return View(FireStore.GetResult());
        }
        #endregion

        #region "POST"
        [HttpPost]
        public IActionResult Check()
        {
            ResultModel Result = FireStore.GetResult();
            if (Result.WinnerCandidate != null)
            {
                return Json(new { State = "Error", Reason = "Winner Candidate is not empty" });
            }
            else
            {
                if (DateHelper.IsDateBeforeOrEqual(Result.DueDate))
                {
                    List<CandidateVoteModel> CandidateVotes = FireStore.GetAllCandidateVotes(true).Result;
                    if (CandidateVotes.Count > 0)
                    {
                        string Id = CandidateVotes[0].Id;
                        int TotalVotes = int.Parse(CandidateVotes[0].TotalVotes);
                        foreach (CandidateVoteModel Cv in CandidateVotes)
                        {
                            int CurrentTotalVotes = int.Parse(Cv.TotalVotes);
                            if (TotalVotes <= CurrentTotalVotes)
                            {
                                Id = Cv.Id;
                                TotalVotes = CurrentTotalVotes;
                            }
                        }
                        CandidateModel Winner = FireStore.GetCandidate(CandidateHelper.EncryptField(Id)).Result;
                        Result.WinnerCandidate = Winner;
                        Result.TotalVotes = TotalVotes.ToString();
                        FireStore.UpdateResult(Result);
                        return Json(new { State = "Success", Id});
                    }
                    else
                    {
                        return Json(new { State = "Error", Reason = "Candidate Votes are zero"});
                    }
                }
                else
                {
                    return Json(new { State = "Error", Reason = "Due date is still early and didn't pass the last day."});
                }
            }
        }
        #endregion
    }
}