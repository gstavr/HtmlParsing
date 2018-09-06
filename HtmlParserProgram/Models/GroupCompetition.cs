using System;
using System.Collections.Generic;

namespace HtmlParserProgram.Models
{
    public partial class GroupCompetition
    {
        public GroupCompetition()
        {
            Competition = new HashSet<Competition>();
        }

        public int Id { get; set; }
        public string Descr { get; set; }
        public string AlternativeDescr { get; set; }

        public ICollection<Competition> Competition { get; set; }
    }
}
