using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pollerino.Models
{
    public class Poll
    {
       public int PollId { get; set; }
       public string Link { get; set; }
       public string QuestionText { get; set; }
       public bool MultipleChoices { get; set; }

       public virtual IEnumerable<Option> Options { get; set; }

    }
}