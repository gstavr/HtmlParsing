using System;
using System.Collections.Generic;

namespace HtmlParserProgram.Models
{
    public partial class GamePickValue
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public int? GamePickId { get; set; }
        public string Descr { get; set; }
        public string AlternativeDescr { get; set; }
        public double? PickValue { get; set; }
        public DateTime? OddsUpdated { get; set; }

        public Companies Company { get; set; }
        public GamePick GamePick { get; set; }
    }
}
