using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace EvicomApp.Models
{
    internal class Image
    {
        public int Id { get; set; }
        public int SourceId { get; set; }
        public string SourceType { get; set; }
        public string ImagePath { get; set; }
        public bool IsMain { get; set; }
        public DateTime CreatedAt { get; set; }

        public static readonly string[] Imageables = { nameof(User), nameof(House) };

    }
}
