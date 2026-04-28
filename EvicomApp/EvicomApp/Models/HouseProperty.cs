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

        public static readonly Dictionary<string, List<string>> KeysValues = new Dictionary<string, List<string>>
        {
            { "Isýnma", new List<string> { "Doðalgaz (Kombi)", "Merkezi Sistem", "Yerden Isýtma", "Klima", "Güneþ Enerjisi", "Soba" } },
            { "Cephe", new List<string> { "Kuzey", "Güney", "Doðu", "Batý" } },
            { "Kullaným Durumu", new List<string> { "Boþ", "Kiracýlý", "Mülk Sahibi Oturuyor" } },
            { "Eþya Durumu", new List<string> { "Eþyalý", "Eþyasýz" } },
            { "Yapý Tipi", new List<string> { "Betonarme", "Çelik", "Ahþap", "Prefabrik" } },
            { "Site Ýçerisinde", new List<string> { "Evet", "Hayýr" } }
        };
    }
}
