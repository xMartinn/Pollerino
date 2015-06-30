using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Pollerino.Models
{

    public class ModelInitializer : DropCreateDatabaseIfModelChanges<ModelContext>
    {

        protected override void Seed(ModelContext context)
        {
/*
            var polls = new List<Poll>
             {
                 new Poll {Question = "Whats your favorite color?", MultipleChoices = false},
             };

            foreach (var item in polls)
            {
                context.Polls.Add(item);
            }

            context.SaveChanges();
 */
        }
    }
}