using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace EvicomApp.Models
{
    internal class House
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int CategoryId { get; set; }
        public int DistrictId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FullAddress { get; set; }
        public DateTime CreatedAt { get; set; }

        public User Owner { get; set; } = new User();
        public Category Category { get; set; } = new Category();
        public District District { get; set; } = new District();

        public List<HouseProperty> Properties { get; set; } = new List<HouseProperty>();
        public List<Ad> Ads { get; set; } = new List<Ad>();
        public List<Image> Images { get; set; } = new List<Image>();

        public static readonly string[] Categories = { "Daire", "Villa", "Rezidans", "Müstakil", "Apart" };
    }
}