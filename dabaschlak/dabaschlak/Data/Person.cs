using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dabaschlak
{
	public class Person
	{
		public Person() { }

		public Person(DataRow row)
		{
			PersonId = Convert.ToInt32(row["PersonId"]);
			Name = Convert.ToString(row["Name"]);
			Vorname = Convert.ToString(row["Vorname"]);
			Netzname = Convert.ToString(row["Netzname"]);
			Tel = Convert.ToString(row["Tel"]);
			Email = Convert.ToString(row["Email"]);
			Aktiv = Convert.ToBoolean(row["Aktiv"]);		 
		}

		public int PersonId		{ get; set; }
		public string Name		{ get; set; }
		public string Vorname	{ get; set; }
		public string Netzname { get; set; }
		public string Tel			{ get; set; }
		public string Email		{ get; set; }
		public bool	Aktiv		{ get; set; }
		
		public string FullName
		{
			get { return Name + ", " + Vorname; }
		}
	}
}
