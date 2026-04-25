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
    internal class Rental
    {
        public int Id { get; set; }
        public int AdId { get; set; }
        public int TenantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public User Tenant { get; set; } = new User();
        public Ad Ad { get; set; } = new Ad();
    }
}
