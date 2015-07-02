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
                return RedirectToAction("Index");
            }


            return View();
        }

        public ActionResult About(int? id)
        {
            Poll poll= db.Polls.Find(id);
         //   poll.Options = db.Options.Where(x => x.PollId == poll.PollId).ToList();  //should load auto - virtual 
            return View(poll);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult About(Poll poll)
        {

            return View();
        }



        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}