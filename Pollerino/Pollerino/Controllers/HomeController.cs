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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Poll poll)
        {
            //TODO check if options not empty angrytoken

            if (ModelState.IsValid)
            {
                db.Polls.Add(poll);
                db.SaveChanges();

                /*  foreach (var option in poll.Options) {
                      if (!String.IsNullOrWhiteSpace(option.OptionText))
                      {
                          option.WasChecked = false;
                          option.PollId = poll.PollId;
                          db.Options.Add(option);
                      }
                  }
                  db.SaveChanges();
   */
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
            /*
            if (db.Votes.Where(x => x.PollId == poll.PollId && x.VoterIp == System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]).Count() > 0)
            {
                //TODO - person already voted angrytoken
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

                        //pripocitat vote Options.numVotes


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

                //pripocitat vote Options.numVotes
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
            return View(poll);
        }

    }
}