using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using

namespace EvicomApp.Models
{
    internal class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<House> Houses { get; set; } = new List<House>();
    }
}
