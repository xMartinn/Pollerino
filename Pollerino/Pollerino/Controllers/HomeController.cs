using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Pollerino.Models;

namespace Pollerino.Controllers
{
    public class HomeController : Controller
    {
        private ModelContext db = new ModelContext();

        public ActionResult Index()
        {
            ViewData["isCorrect"] = true;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Poll poll)
        {

            //TODO enhancement check if some options arent same 
            ViewData["isCorrect"] = true;

            if (String.IsNullOrWhiteSpace(poll.QuestionText) || poll.QuestionText == "enter your poll question here...")
            {
                ModelState.AddModelError("", "Please enter you poll question.");
                ViewData["isCorrect"] = false;

                return View(poll);
            }

            if (ModelState.IsValid)
            {
                List<Option> filtredOptions = new List<Option>();
                foreach (var option in poll.Options)
                {
                    if (!String.IsNullOrWhiteSpace(option.OptionText) && option.OptionText != "enter option here...")
                    {
                        option.WasChecked = false;
                        option.PollId = poll.PollId;
                        filtredOptions.Add(option);
                    }
                }
                if (filtredOptions.Count < 2)
                {
                    ModelState.AddModelError("", "Poll must contain atleast two options to chose.");
                    ViewData["isCorrect"] = false;
                    return View(poll);
                }

                poll.Options = filtredOptions;
                db.Polls.Add(poll);
                db.SaveChanges();

                return RedirectToAction("Vote", new { id = poll.PollId });

            }

            return View();
        }

        public ActionResult Vote(int? id)
        {

            Poll poll = db.Polls.Find(id);
            if (poll == null)
            {
                return Redirect("404");
            }

            poll.Options = db.Options.Where(x => x.PollId == poll.PollId).ToList();  //should load auto - virtual 
            return View(poll);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Vote(Poll poll)
        {

            //TODO

            /*
            if (db.Votes.Where(x => x.PollId == poll.PollId && x.VoterIp == System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]).Count() > 0)
            {     
                    ModelState.AddModelError("", "You have already voted.");
                    return View(poll);
            }
            */

            if (poll.MultipleChoices)
            {
                foreach (var option in poll.Options)
                {
                    if (option.WasChecked)
                    {
                        Vote vote = new Vote();
                        vote.VoterIp = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                        // or HttpContext.Current.Request.UserHostAddress;
                        vote.PollId = option.PollId;
                        vote.OptionId = option.OptionId;
                        db.Votes.Add(vote);


                        //TODO lock for counting 
                        Option optionToUdt = db.Options.Find(vote.OptionId);
                        optionToUdt.NumVotes += 1;
                        db.SaveChanges();


                    }
                }
            }
            else
            {
                Vote vote = new Vote();
                vote.VoterIp = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                // or HttpContext.Current.Request.UserHostAddress;
                vote.PollId = poll.PollId;
                vote.OptionId = (int)poll.SelectedItem;
                db.Votes.Add(vote);

                //TODO lock for counting 
                Option optionToUdt = db.Options.Find(vote.OptionId);
                optionToUdt.NumVotes += 1;
                db.SaveChanges();
            }

            return RedirectToAction("Result", new { id = poll.PollId });
        }


        public ActionResult Result(int? id)
        {
            Poll poll = db.Polls.Find(id);

            if (poll == null)
            {
                return Redirect("404");
            }

            int voteCount=0;

            foreach(Option option in poll.Options){
                voteCount += option.NumVotes;    
            }

            poll.TotalVotes = voteCount;

            foreach (Option option in poll.Options)
            {
                option.Percentage =  Math.Round(((double)option.NumVotes / (double)voteCount) * 100,2);
            }

            return View(poll);
        }

    }
}