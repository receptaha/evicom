using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace EvicomApp.Models
{
	internal class HouseProperty
	{
		public int Id { get; set; }
		public int HouseId { get; set; }
		public string PropertyKey { get; set; }
		public string PropertyValue { get; set; }

		public House House { get; set; }
	}
}
