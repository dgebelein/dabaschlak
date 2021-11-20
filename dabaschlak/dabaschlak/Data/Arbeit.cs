using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dabaschlak
{
	public class Arbeit
	{
		public int ArbeitId			{ get; set; }
		public int VersuchId			{ get; set; } // ForeignKey VersuchId = 0 bei flächenarbeiten
		public int FlaecheId			{ get; set; } // ForeignKey FlaecheId = 0 bei versuchsarbeiten 
		public int PersonId			{ get; set; } // ForeignKey PersonId
		public DateTime Datum		{ get; set; }
		public string Aktion			{ get; set; }
		public string Notizen		{ get; set; }
		
		public static List<string>GetAktionsypen()
		{
			List<string> at = new List<string>();
			at.Add("Anzucht");
			at.Add("Beregnung");	
			at.Add("Bodenbearbeitung");
			at.Add("Düngung");
			at.Add("Drillsaat");
			at.Add("Ernte");
			at.Add("Hacken");
			at.Add("Herbizidbehandlung");
			at.Add("Pflanzung");
			at.Add("Pflanzenschutz");
			at.Add("Pflegemaßnahmen");
			at.Add("Sonstiges");
			at.Add("Versuchsende");
			return at;
		}

		// todo: mögliche Arbeiten in Tabelle auslagern - mit Fehlertexten

	}
}
