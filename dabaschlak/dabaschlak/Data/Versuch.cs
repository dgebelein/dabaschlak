using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dabaschlak
{
	public class Versuch
	{
		public int Id						{ get; set; }
		public int Leiter					{ get; set; }	// ForeignKey PersonId
		public string Standorte			{ get; set; }	// aus Tabelle VersuchxFlaeche
		public int Start					{ get; set; }
		public int Ende					{ get; set; }
		public string VerBez				{ get; set; }
		public string Plan				{ get; set; }
		public string Frage				{ get; set; }
		public string Kultur				{ get; set; }
		public string LeiterTxt			{ get; set; }	// nicht in 'Versuch' - durch Join ermitteln
		public List<int> IdsStandorte { get; set; }	// aus Tabelle VersuchxFlaeche
	}
}
