using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvicomApp.Models
{
    internal class District
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Name { get; set; }

        public City City { get; set; }

        public List<House> Houses { get; set; } = new List<House>();
    }
}
