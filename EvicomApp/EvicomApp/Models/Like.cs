using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace EvicomApp.Models
{
        internal class Like
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public int SourceId { get; set; }
            public string SourceType { get; set; }

            public User User { get; set; }
        }
}
