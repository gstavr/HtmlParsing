using System;
using System.Collections.Generic;

namespace HtmlParserProgram.Models
{
    public partial class Game
    {
        public Game()
        {
            GamePick = new HashSet<GamePick>();
        }

        public int Id { get; set; }
        public int? CompetitionId { get; set; }
        public string Descr { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTime? MatchDate { get; set; }

        public Competition Competition { get; set; }
        public ICollection<GamePick> GamePick { get; set; }
    }
}
