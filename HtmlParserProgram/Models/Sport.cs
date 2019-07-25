using System;
using System.Collections.Generic;

namespace HtmlParserProgram.Models
{
    public partial class Sport
    {
        public Sport()
        {
            Competition = new HashSet<Competition>();
        }

        public int Id { get; set; }
        public string Descr { get; set; }

        public virtual ICollection<Competition> Competition { get; set; }
    }
}
