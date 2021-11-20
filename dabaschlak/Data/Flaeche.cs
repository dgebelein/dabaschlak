using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dabaschlak
{
	public class Flaeche
	{
		public Flaeche()
		{

	 
		}

		public Flaeche(DataRow row)
		{
			FlaeId = Convert.ToInt32(row["FlaeId"]);
			FlaeBez = Convert.ToString(row["FlaeBez"]);
			FlaeTyp = Convert.ToString(row["FlaeTyp"]);
			FlaeEig = Convert.ToString(row["FlaeEig"]);
	 
		}

		public int FlaeId			{ get; set; }
		public string FlaeBez	{ get; set; }
		public string FlaeEig	{ get; set; }
		public string FlaeTyp	{ get; set; }	//A,G,K,S

		public static List<string>GetFlaechentypen()
		{
			List<string> ft = new List<string>();
			ft.Add("Ackerfläche");
			ft.Add("Gewächshaus");
			ft.Add("Klimakammer");
			ft.Add("Stellfläche");
			return ft;
		}

		public static List<string>GetFlaechentypenMz()
		{
			List<string> ft = new List<string>();
			ft.Add("Ackerflächen");
			ft.Add("Gewächshäuser");
			ft.Add("Klimakammern");
			ft.Add("Stellflächen");
			return ft;
		}

	}
}
