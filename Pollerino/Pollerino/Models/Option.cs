using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pollerino.Models
{
    public class Option
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; }
        public virtual IEnumerable<Vote> Votes{get; set;}

        public int PollId { get; set; }
        public virtual Poll Poll { get; set; }
    }
}
