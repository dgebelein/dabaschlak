using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dabaschlak
{
	public struct Kategorie
	{
		public string Arbeit;
		public bool hatNotizen;
		public string Fehlertext;
	}


	public class Arbeitskategorien
	{
		List<Kategorie> _items;
		
		public Arbeitskategorien()	
		{
			InitItems();
		}

		private void InitItems()
		{
			_items = new List<Kategorie>();
			DataTable dt = SqlAccess.GetArbeitskategorien();
			if (dt== null)
			{ 
				MsgWindow.Show("Kein Zugriff auf Arbeitskategorien", SqlAccess.ErrorMsg, MessageLevel.Error);
				return;
			}

			foreach(DataRow row in dt.Rows)
			{
				_items.Add(new Kategorie { Arbeit = Convert.ToString(row["Kategorie"]), hatNotizen = Convert.ToBoolean(row["hatNotizen"]), Fehlertext = Convert.ToString(row["FehlerText"])}); 
			}

		}

		public List<Kategorie> Items
		{
			get { return _items; }
		}

		public List<string> Aktionen
		{ 
			get
			{
				List<string> aktionen = new List<string>();
				foreach(Kategorie k in _items)
				{
					aktionen.Add(k.Arbeit);
				}
				return aktionen;
			}
		}
	}
}
