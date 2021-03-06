﻿using System;
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
        public string DynamicId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual ICollection<GamePick> GamePick { get; set; }
    }
}
