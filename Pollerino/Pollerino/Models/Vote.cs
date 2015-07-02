using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pollerino.Models
{
    public class Vote
    {
        public int VoteId { get; set; }
        public string VoterIp { get; set; }

        public int OptionId { get; set; }
        public virtual Option Option { get; set; }

        public int PollId { get; set; }
        public virtual Poll Poll { get; set; }

    }
}
