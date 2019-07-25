using System;
using System.Collections.Generic;

namespace HtmlParserProgram.Models
{
    public partial class Competition
    {
        public Competition()
        {
            Game = new HashSet<Game>();
        }

        public int Id { get; set; }
        public int? SportId { get; set; }
        public int? GroupId { get; set; }
        public string Descr { get; set; }
        public string AlternativeDescr { get; set; }
        public string DynamicId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public short? IsValid { get; set; }

        public virtual GroupCompetition Group { get; set; }
        public virtual Sport Sport { get; set; }
        public virtual ICollection<Game> Game { get; set; }
    }
}
