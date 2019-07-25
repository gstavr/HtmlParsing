using System;
using System.Collections.Generic;

namespace HtmlParserProgram.Models
{
    public partial class Companies
    {
        public Companies()
        {
            GamePickValue = new HashSet<GamePickValue>();
            GamePickValueLog = new HashSet<GamePickValueLog>();
        }

        public int Id { get; set; }
        public string Descr { get; set; }
        public string Link { get; set; }
        public string DynamicParam { get; set; }
        public int? IsActive { get; set; }

        public virtual ICollection<GamePickValue> GamePickValue { get; set; }
        public virtual ICollection<GamePickValueLog> GamePickValueLog { get; set; }
    }
}
