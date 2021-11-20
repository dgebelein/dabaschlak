using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dabaschlak
{
	public class Aktion
	{
		public int AktionId			{ get; set; }
		public int VersuchId			{ get; set; } // ForeignKey VersuchId = 0 bei flächenarbeiten
		public int FlaecheId			{ get; set; } // ForeignKey FlaecheId = 0 bei versuchsarbeiten 
		public int PersonId			{ get; set; } // ForeignKey PersonId
		public DateTime Datum		{ get; set; }
		public string Typ				{ get; set; }
		public string Notizen		{ get; set; }

		public override string ToString()
		{
			return SqlAccess.GetArbeitenMailData(this);
		}



	}
}
