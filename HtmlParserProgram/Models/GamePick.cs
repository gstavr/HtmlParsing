using System;
using System.Collections.Generic;

namespace HtmlParserProgram.Models
{
    public partial class GamePick
    {
        public GamePick()
        {
            GamePickValue = new HashSet<GamePickValue>();
            GamePickValueLog = new HashSet<GamePickValueLog>();
        }

        public int Id { get; set; }
        public int GameId { get; set; }
        public string Descr { get; set; }
        public int? OddSumNum { get; set; }
        public int? HasCashout { get; set; }

        public virtual Game Game { get; set; }
        public virtual ICollection<GamePickValue> GamePickValue { get; set; }
        public virtual ICollection<GamePickValueLog> GamePickValueLog { get; set; }
    }
}
