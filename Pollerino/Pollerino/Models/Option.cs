using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Pollerino.Models
{
    public class Option
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; }

        [NotMapped]
        public bool WasChecked { get; set; } //just for checkbuttons

        public virtual List<Vote> Votes{get; set;}

        public int PollId { get; set; }
        public virtual Poll Poll { get; set; }

        public int NumVotes { get; set; }

        [NotMapped]
        public double Percentage { get; set; }
    }
}
