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

        public GroupCompetition Group { get; set; }
        public Sport Sport { get; set; }
        public ICollection<Game> Game { get; set; }
    }
}
