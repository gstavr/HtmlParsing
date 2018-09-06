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

        public Game Game { get; set; }
        public ICollection<GamePickValue> GamePickValue { get; set; }
        public ICollection<GamePickValueLog> GamePickValueLog { get; set; }
    }
}
