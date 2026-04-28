using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace EvicomApp.Models
{
    internal class Ad
    {
        public int Id { get; set; }
        public int HouseId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public House House { get; set; }

        public List<Rental> Rentals { get; set; } = new List<Rental>();

        public static readonly string[] statuses = {"active", "passive"};
    }
}
