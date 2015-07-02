using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pollerino.Models
{
    public class Poll
    {
       public int PollId { get; set; }
       public string QuestionText { get; set; }
       public bool MultipleChoices { get; set; }

       public virtual List<Option> Options { get; set; }

       public int? SelectedItem { get; set; } //just for radiobuttons

    }
}