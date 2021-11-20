using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace dabaschlak
{
	public class SqlAccess
	{
		static string cs = @"Data Source = " + GlobData.DbSource;    //sqlite Datei


		public static string ErrorMsg;

		#region Zugriffsberechtigungen

		public static bool IsAdmin(int personId)
		{
			ErrorMsg = null;
			string sqlCmd =
			@"SELECT * FROM Admins
				WHERE PersonId = @PersonId;";
			
			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					con.Open();

					using (SQLiteCommand cmd = new SQLiteCommand(sqlCmd, con))
					{ 
						cmd.Parameters.Add(new SQLiteParameter("@PersonId", personId));
						using (SQLiteDataReader reader = cmd.ExecuteReader())
						{
							return (reader.Read());
						}
					}
					
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}
		}

		#endregion

		#region Personen

		public static Dictionary<int, string> GetNetNamesDict()
		{
			ErrorMsg = null;
			Dictionary<int, string> dict = new Dictionary<int, string>();

			DataTable dt = GetPersonsTable();
			if (dt != null)
			{
				foreach (DataRow row in dt.Rows)
				{
					int index = Convert.ToInt32(row["PersonId"]);
					string u = Convert.ToString(row["Netzname"]);
					dict.Add(index, u);
				}
			}
			else
				ErrorMsg = "Mitarbeiter-Daten nicht gefunden.";

			return dict;

		}

		//public static List<string> GetNetnames()
		//{
		//	ErrorMsg = null;
		//	Dictionary<int, string> dict = GetNetNamesDict();
		//	if (dict == null)
		//		return null;

		//	var users = dict.Values.ToList();
		//	users.Sort();
		//	return users;

		//}

		public static Dictionary<int,string> GetUserDict()
		{	
			ErrorMsg = null;
			Dictionary<int, string> dict = new Dictionary<int, string>();
					
			DataTable dt = GetPersonsTable();
			if (dt != null)
			{
				foreach (DataRow row in dt.Rows)
				{
					int index = Convert.ToInt32(row["PersonId"]);
					string u = row["Name"] + ", " + row["Vorname"];
					dict.Add(index, u);
				}
			}
			else
				ErrorMsg = "Mitarbeiter-Daten nicht gefunden.";

			return dict;

		}

		//public static List<string> GetUserNames()
		//{
		//	ErrorMsg = null;
		//	Dictionary<int, string> dict = GetUserDict();
		//	if (dict == null)
		//		return null;

		//	var users = dict.Values.ToList();
		//	users.Sort();
		//	return users;

		//}


		public static  Person FindPersonFromUsername(string userName)
		{
			ErrorMsg = null;

			Person person = null;
			if (string.IsNullOrEmpty(userName))
				return null;


			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					con.Open();

					using (SQLiteCommand cmd = new SQLiteCommand(con))
					{
	
						cmd.CommandText = "Select * from personen where Netzname like @Username;";
						cmd.Parameters.Add(new SQLiteParameter("@Username", userName));
	
						using (SQLiteDataReader reader = cmd.ExecuteReader())
						{
							if (reader.Read())
							{
								person = new Person
								{
									PersonId = reader.GetInt32(reader.GetOrdinal("PersonId")),
									Name = reader.GetString(reader.GetOrdinal("Name")),
									Vorname = reader.GetString(reader.GetOrdinal("Vorname")),
									Netzname = reader.GetString(reader.GetOrdinal("Netzname")),
									Tel = reader.GetString(reader.GetOrdinal("Tel")),
									Email = reader.GetString(reader.GetOrdinal("Email")),
									//Aktiv = (bool)reader["Aktiv"]
								};
							}
							else
								ErrorMsg = "kein registrierter Nutzer";
						}
					}
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return null;
			}

			return person;



			//if (string.IsNullOrEmpty(userName))
			//	return null;

			//string[] sep = { ",", "." };
			//string[] nameParts = userName.Split(sep, StringSplitOptions.RemoveEmptyEntries);
			//for (int n=0; n< nameParts.Length;n++)
			//{
			//	nameParts[n] = nameParts[n].Trim();
			//}

			//try
			//{
			//	using (SQLiteConnection con = new SQLiteConnection(cs))
			//	{
			//		con.Open();

			//		using (SQLiteCommand cmd = new SQLiteCommand(con))
			//		{
			//			if (nameParts.Length < 2)
			//			{
			//				cmd.CommandText = "Select * from personen where Name like @nachname;";
			//				cmd.Parameters.Add(new SQLiteParameter("@nachname", nameParts[0]));
			//			}
			//			else
			//			{
			//				cmd.CommandText = "Select * from personen where Name like @nachname and Vorname like @vorname;";
			//				cmd.Parameters.Add(new SQLiteParameter("@nachname", nameParts[0]));
			//				cmd.Parameters.Add(new SQLiteParameter("@vorname", nameParts[1]));

			//			}
			//			using (SQLiteDataReader reader = cmd.ExecuteReader())
			//			{ 
			//				if (reader.Read())
			//				{
			//					person = new Person
			//					{
			//						PersonId = reader.GetInt32(reader.GetOrdinal("PersonId")),
			//						Name = reader.GetString(reader.GetOrdinal("Name")),
			//						Vorname = reader.GetString(reader.GetOrdinal("Vorname")),
			//						Tel = reader.GetString(reader.GetOrdinal("Tel")),
			//						Email = reader.GetString(reader.GetOrdinal("Email")),
			//						//Aktiv = (bool)reader["Aktiv"]
			//					};
			//				}
			//				else
			//					ErrorMsg = "kein registrierter Nutzer";
			//			}
			//		}
			//	}
			//}
			//catch (Exception e)
			//{
			//	ErrorMsg = e.Message;
			//	return null;
			//}

			//return person;
		}
	
		public  static DataTable GetPersonsTable()
		{
			ErrorMsg = null;
			DataTable dt = null;

			try {
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{ 
					SQLiteDataAdapter da = new SQLiteDataAdapter("Select * from personen order by Name,Vorname", con);
					SQLiteCommandBuilder cb = new SQLiteCommandBuilder(da);
					dt = new DataTable();
					da.Fill(dt);
				}
			}
			catch(Exception e)
			{
				ErrorMsg = e.Message;
				return null;
			}

			return dt;
		}

		public static bool UpdatePersons(DataTable dt)
		{
			ErrorMsg = null;

			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					//con.Open();

					using (SQLiteCommand cmd = new SQLiteCommand(con))
					{
						cmd.CommandText = "Select * from personen;";
						SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

						SQLiteCommandBuilder builder = new SQLiteCommandBuilder(da);
						da.Update(dt);
					}

					//con.Close();
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}

			return true;
		}

		public static bool IsValidPerson(Person person)
		{
			ErrorMsg = null;

			string sqlCmd = @"SELECT * FROM	Personen
									WHERE PersonId != @PersonId AND Name = @Name AND Vorname = @Vorname;";
			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					con.Open();

					using (SQLiteCommand cmd = new SQLiteCommand(sqlCmd, con))
					{ 
						cmd.Parameters.Add(new SQLiteParameter("@PersonId", person.PersonId));
						cmd.Parameters.Add(new SQLiteParameter("@Name", person.Name));
						cmd.Parameters.Add(new SQLiteParameter("@Vorname", person.Vorname));

						
						using (SQLiteDataReader reader = cmd.ExecuteReader())
						{
							if (reader.Read())
								return false;
							else
								return true;
						}
					}
					
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}
		}
		
		public static bool CanDeletePerson(int personId)
		{
			ErrorMsg = null;
			//darf nicht in Versuchen, versuchsarbeiten, benachrichtigungen vorkommen
			string[] sqlCmds = { @"SELECT * FROM Versuche WHERE Leiter=@PersonId;",
										@"SELECT * FROM Aktionen WHERE Person=@PersonId;",
										@"SELECT * FROM Nachrichten WHERE PersonId=@PersonId;"
									 };
			string[] errMsg = { @"Dieser Mitarbeiter ist in einem Versuch als 'Versuchsleiter' eingetragen",
										@"Dieser Mitarbeiter hat Aktionen durchgeführt",
										@"Dieser Mitarbeiter hat sich für Nachrichten registriert"
									 };

			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					con.Open();
					for ( int n=0;n<sqlCmds.Length; n++)
					{ 
						using (SQLiteCommand cmd = new SQLiteCommand(sqlCmds[n], con))
						{ 
							cmd.Parameters.Add(new SQLiteParameter("@PersonId", personId));
						
							using (SQLiteDataReader reader = cmd.ExecuteReader())
							{
								if (reader.Read())
								{
									ErrorMsg = errMsg[n];
									return false;
								}
							}
						}
					}
					return true;
					
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}

		}

		public static bool DeletePerson( int personId)
		{
			if (!CanDeletePerson(personId))
				return false;

			string sqlCmd = @"DELETE FROM Personen
									WHERE PersonId=@PersonId;";

			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{	
					con.Open();
					using (SQLiteCommand cmd = new SQLiteCommand(sqlCmd, con))
					{ 
						cmd.Parameters.AddWithValue("@PersonId", personId);

						int n = cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}

			return true;
		}
		
		#endregion

		#region Versuche
		
		public  static DataTable GetVersuchsTableRaw()
		{
			ErrorMsg = null;
			DataTable dt = null;

			try {
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{ 
					SQLiteDataAdapter da = new SQLiteDataAdapter("Select * from Versuche", con);
					SQLiteCommandBuilder cb = new SQLiteCommandBuilder(da);
					dt = new DataTable();
					da.Fill(dt);
				}
			}
			catch(Exception e)
			{
				ErrorMsg = e.Message;
				return null;
			}

			return dt;
		}

		public  static DataTable GetVersuchsTable(int versuchsJahr, string flTyp)
		{
			string limStart, limEnd;
			if(versuchsJahr!=0)
			{
				limStart = "v.Start";
				limEnd = "v.Ende";
			}
			else 
			{ 
				limStart = "0";
				limEnd = "3000";
			}

			string cmdFmt =
			@"SELECT v.VersuchId,
					v.VerBez,
					Personen.Name || ', ' || Personen.Vorname AS Versuchsleiter,
					v.Start,
					v.Ende,
					v.Versuchsplan,
					v.Versuchsfrage,
					v.Kultur,
					v.Leiter,
					GROUP_CONCAT(x.Fid) AS StandortIds,
					GROUP_CONCAT(f.FlaeBez) As Standorte
			FROM Versuche v
			LEFT JOIN Personen ON v.Leiter = Personen.PersonId
			LEFT JOIN VxF x ON v.VersuchId = x.Vid
			LEFT JOIN Flaechen f ON f.FlaeId= x.Fid
			WHERE (@Jahr BETWEEN {0} AND {1}) AND (f.FlaeTyp LIKE {2})
			GROUP BY x.Vid;";

			string searchFlaeche = (String.IsNullOrWhiteSpace(flTyp)) ? "'%'" : "'%" + flTyp[0] + @"%'";

			string cmdRaw = String.Format(cmdFmt, limStart, limEnd, searchFlaeche);
			
         ErrorMsg = null;
			DataTable dt = null;

			try {
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					using (SQLiteCommand sqlCmd = new SQLiteCommand(cmdRaw, con))
					{ 
						sqlCmd.Parameters.AddWithValue("@Jahr", versuchsJahr);
						sqlCmd.Parameters.AddWithValue("@Start", "v.Start");
						//sqlCmd.Parameters.AddWithValue("@FlTyp",flTyp);

						SQLiteDataAdapter da = new SQLiteDataAdapter(sqlCmd);
						dt = new DataTable();
						da.Fill(dt);
						FormatStandortColumn(dt);
					}
				}
			}
			catch(Exception e)
			{
				ErrorMsg = e.Message;
				return null;
			}

			return dt;
		}

		public  static DataTable GetExtVersuchsTable(int startJahr, int endJahr, List<int> personenIds, List<int> flaechenIds,string searchKulturText,string searchVersuchText,EnSearchSort sortBy )
		{
			ErrorMsg = null;
			DataTable dt = null;

			string sqlSelect =
			@"SELECT v.VersuchId,
					v.VerBez,
					Personen.Name || ', ' || Personen.Vorname AS Versuchsleiter,
					v.Start,
					v.Ende,
					v.Versuchsplan,
					v.Versuchsfrage,
					v.Kultur,
					GROUP_CONCAT(x.Fid) AS StandortIds,
					GROUP_CONCAT(f.FlaeBez) As Standorte
			FROM Versuche v
			LEFT JOIN Personen ON v.Leiter = Personen.PersonId
			LEFT JOIN VxF x ON v.VersuchId = x.Vid
			LEFT JOIN Flaechen f ON f.FlaeId= x.Fid";


			StringBuilder sqlWhere = new StringBuilder(" ");
			if ((startJahr == 0) || (endJahr == 0))
			{
				startJahr = 0;
				endJahr = 3000;
			}

			sqlWhere.Append($"WHERE ((v.Start BETWEEN {startJahr} AND {endJahr}) OR (v.Ende BETWEEN {startJahr} AND {endJahr}))");
			if (personenIds != null)
			{
				if (personenIds.Count > 0)
				{
					sqlWhere.Append($"AND (v.Leiter IN ({personenIds[0]}");
					for (int p = 1; p < personenIds.Count; p++)
						sqlWhere.Append($",{personenIds[p]}");
					sqlWhere.Append($"))");
				}
			}

			if (flaechenIds != null)
			{
				if (flaechenIds.Count > 0)
				{
					sqlWhere.Append($"AND (f.FlaeId IN ({flaechenIds[0]}");
					for (int p = 1; p < flaechenIds.Count; p++)
						sqlWhere.Append($",{flaechenIds[p]}");
					sqlWhere.Append($"))");
				}
			}

			sqlWhere.Append($"AND (v.Kultur LIKE @KSearch) AND (v.VerBez LIKE @VSearch) ");
			string kSearch = (string.IsNullOrEmpty(searchKulturText)) ? "%" : "%" + searchKulturText + "%";
			string vSearch = (string.IsNullOrEmpty(searchVersuchText)) ? "%" : "%" + searchVersuchText + "%";

			string orderBy = "v.Start";
			switch (sortBy)
			{
				case EnSearchSort.Versuch:orderBy = "v.VerBez"; break;
				case EnSearchSort.Flaeche: orderBy = "Standorte"; break;
				case EnSearchSort.Person: orderBy = "Versuchsleiter"; break;
				case EnSearchSort.Kultur: orderBy = "v.Kultur"; break;
			}
			string sqlGroupAndOrder = $"GROUP BY x.Vid ORDER BY {orderBy}; ";
			


			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					string sqlQuery = sqlSelect + sqlWhere.ToString() + sqlGroupAndOrder;
					using (SQLiteCommand sqlCmd = new SQLiteCommand(sqlQuery, con))
					{
						sqlCmd.Parameters.AddWithValue("@KSearch", kSearch);
						sqlCmd.Parameters.AddWithValue("@VSearch", vSearch);
						SQLiteDataAdapter da = new SQLiteDataAdapter(sqlCmd);
						dt = new DataTable();
						da.Fill(dt);
						FormatStandortColumn(dt);
					}
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return null;
			}

			return dt;
		}

		public static bool InsertNewVersuch(Versuch versuch)
		{
			ErrorMsg = null;
			string sqlCmd =
			@"INSERT INTO Versuche ( [Leiter], [Start], [Ende],[VerBez], [Versuchsplan], [Versuchsfrage],[Kultur])
			VALUES(@Leiter, @Start, @Ende, @VerBez, @Versuchsplan, @Versuchsfrage, @Kultur)";

			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{	
					con.Open();
					using (SQLiteCommand cmd = new SQLiteCommand(sqlCmd, con))
					{ 
						cmd.Parameters.AddWithValue("@Leiter", versuch.Leiter);
						cmd.Parameters.AddWithValue("@Start", versuch.Start);
						cmd.Parameters.AddWithValue("@Ende", versuch.Ende);
						cmd.Parameters.AddWithValue("@VerBez", versuch.VerBez);
						cmd.Parameters.AddWithValue("@Versuchsplan", versuch.Plan);
						cmd.Parameters.AddWithValue("@Versuchsfrage", versuch.Frage);
						cmd.Parameters.AddWithValue("@Kultur", versuch.Kultur);


						int n = cmd.ExecuteNonQuery();
						versuch.Id = (int)con.LastInsertRowId;
					}
					//con.Close();
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}

			return UpdateFlaechenzuordnungVersuch(versuch);
		}

		public static bool UpdateVersuch(Versuch versuch)
		{
			ErrorMsg = null;

			string sqlCmd = @"UPDATE Versuche
									SET	Leiter=@Leiter,
											Start=@Start,
											Ende=@Ende,
											VerBez=@VerBez,
											Versuchsplan=@Versuchsplan,
											Versuchsfrage=@Versuchsfrage,
											Kultur=@Kultur
									WHERE VersuchId=@VersuchId;";

			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{	
					con.Open();
					using (SQLiteCommand cmd = new SQLiteCommand(sqlCmd, con))
					{ 
						cmd.Parameters.AddWithValue("@Leiter", versuch.Leiter);
						cmd.Parameters.AddWithValue("@Start", versuch.Start);
						cmd.Parameters.AddWithValue("@Ende", versuch.Ende);
						cmd.Parameters.AddWithValue("@VerBez", versuch.VerBez);
						cmd.Parameters.AddWithValue("@Versuchsplan", versuch.Plan);
						cmd.Parameters.AddWithValue("@Versuchsfrage", versuch.Frage);
						cmd.Parameters.AddWithValue("@Kultur", versuch.Kultur);
						cmd.Parameters.AddWithValue("@VersuchId", versuch.Id);

						int n = cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}

			return UpdateFlaechenzuordnungVersuch(versuch);
		}
			
		public static bool IsValidVersuch(Versuch versuch)
		{
			ErrorMsg = null;

			string sqlCmd = @"SELECT * FROM	Versuche
									WHERE VersuchId != @VersuchId AND VerBez = @VerBez;";
			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					con.Open();

					using (SQLiteCommand cmd = new SQLiteCommand(sqlCmd, con))
					{ 
						cmd.Parameters.Add(new SQLiteParameter("@VersuchId", versuch.Id));
						cmd.Parameters.Add(new SQLiteParameter("@VerBez", versuch.VerBez));
						
						using (SQLiteDataReader reader = cmd.ExecuteReader())
						{
							if (reader.Read())
								return false;
							else
								return true;
						}
					}
					
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}
		}
		
		public static bool CanDeleteVersuch(int versuchId)
		{
			ErrorMsg = null;

			string sqlCmd = 
			@"SELECT * FROM Aktionen
			  WHERE Versuch=@VersuchId;";

			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					con.Open();

					using (SQLiteCommand cmd = new SQLiteCommand(sqlCmd, con))
					{ 
						cmd.Parameters.Add(new SQLiteParameter("@VersuchId", versuchId));
						
						using (SQLiteDataReader reader = cmd.ExecuteReader())
						{
							if (reader.Read())
							{
								ErrorMsg = "Für diesen Versuch existieren bereits Aktionen";
								return false;
							}
							else
								return true;
						}
					}
					
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}
		}

		public static bool DeleteVersuch(int versuchId)
		{
			ErrorMsg = null;

			if(!CanDeleteVersuch(versuchId))	// Aktionen für Versuch vorhanden?
			{
				return false;
			}

			string sqlCmd = @"DELETE FROM Versuche
									WHERE VersuchId=@VersuchId;";

			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{	
					con.Open();
					using (SQLiteCommand cmd = new SQLiteCommand(sqlCmd, con))
					{ 
						cmd.Parameters.AddWithValue("@VersuchId", versuchId);

						int n = cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}

			if (!ClearFlaechenzuordnungVersuch(versuchId))
				return false;

			return ClearNachrichtenzuordnungVersuch(versuchId);
		}

		#endregion

		#region Flächen
		//public static Dictionary<int,string> GetFlaechenDict()
		//{	
		//	ErrorMsg = null;
		//	Dictionary<int, string> dict = new Dictionary<int, string>();
					
		//	DataTable dt = GetFlaechenTable(null);
		//	if (dt != null)
		//	{
		//		foreach (DataRow row in dt.Rows)
		//		{
		//			int index = Convert.ToInt32(row["FlaeId"]);
		//			string f = Convert.ToString(row["FlaeBez"]);
		//			dict.Add(index, f);
		//		}
		//	}
		//	else
		//		ErrorMsg = "Versuchsflächen-Daten nicht gefunden.";

		//	return dict;
		//}


		public  static DataTable GetFlaechenTable(string flaechenTyp)
		{
			string limFlaeche = string.IsNullOrWhiteSpace(flaechenTyp) ? "%" : flaechenTyp[0] + "%";
			string sql =
			//@"SELECT * from Flaechen
			//WHERE FlaeTyp LIKE @FlTyp
			//Order By FlaeTyp, FlaeBez";
			@"SELECT * from Flaechen
			WHERE FlaeTyp LIKE @FlTyp
			Order By FlaeTyp,
			substr(FlaeBez,1,1), cast(substr(FlaeBez,2) as integer)";


			ErrorMsg = null;
			DataTable dt = null;

			try {
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					using (SQLiteCommand sqlCmd = new SQLiteCommand(sql, con))
					{
						sqlCmd.Parameters.AddWithValue("@FlTyp", limFlaeche);

						SQLiteDataAdapter da = new SQLiteDataAdapter(sqlCmd);
						SQLiteCommandBuilder cb = new SQLiteCommandBuilder(da);
						dt = new DataTable();
						da.Fill(dt);

						foreach (DataRow row in dt.Rows)
						{
							switch ((string)row["FlaeTyp"])
							{
								case "A": row["FlaeTyp"] = "Ackerfläche"; break;
								case "K": row["FlaeTyp"] = "Klimakammer"; break;
								case "G": row["FlaeTyp"] = "Gewächshaus"; break;
								case "S": row["FlaeTyp"] = "Stellfläche"; break;
								default: row["FlaeTyp"] = "???"; break;
							}
						}
					}
				}
			}
			catch(Exception e)
			{
				ErrorMsg = e.Message;
				return null;
			}

			return dt;
		}

		public static bool UpdateFlaechen(DataTable dt)
		{
			foreach(DataRow row in dt.Rows)
			{
				if (row.RowState == DataRowState.Deleted)
					continue;
				row["FlaeTyp"] = ((string)row["FlaeTyp"])[0];
			}	

			ErrorMsg = null;

			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					using (SQLiteCommand cmd = new SQLiteCommand(con))
					{
						cmd.CommandText = "Select * from flaechen;";
						SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);

						SQLiteCommandBuilder builder = new SQLiteCommandBuilder(da);
						da.Update(dt);
					}
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}

			return true;
		}

		//public static List <string> GetFlaechenBezeichnungen()
		//{
		//	ErrorMsg = null;
		//	List<string> bezeichnungen = new List<string>();

		//	DataTable dt = GetFlaechenTable(null);
		//	if (dt != null)
		//	{
		//		foreach(DataRow row in dt.Rows)
		//		{
		//			bezeichnungen.Add((string)row["FlaeBez"]);
		//		}
		//	}

		//	return bezeichnungen;
		//}

		public static bool ExistsFlaeche(Flaeche flaeche)
		{
			ErrorMsg = null;

			string sqlCmd = @"SELECT * FROM	Flaechen
									WHERE FlaeId != @FlaeId AND FlaeBez = @FlaeBez;";
			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					con.Open();

					using (SQLiteCommand cmd = new SQLiteCommand(sqlCmd, con))
					{ 
						cmd.Parameters.Add(new SQLiteParameter("@FlaeId", flaeche.FlaeId));
						cmd.Parameters.Add(new SQLiteParameter("@FlaeBez", flaeche.FlaeBez));
						
						using (SQLiteDataReader reader = cmd.ExecuteReader())
						{
							if (reader.Read())
								return true;
							else
								return false;
						}
					}
					
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}
		}

		public static bool CanDeleteFlaeche(int flaecheId)
		{
			ErrorMsg = null;
			//darf nicht in Versuchen, versuchsarbeiten, benachrichtigungen vorkommen
			string[] sqlCmds = { @"SELECT * FROM VxF WHERE Fid=@FlaecheId;",
										@"SELECT * FROM Aktionen WHERE Flaeche=@FlaecheId;",
									 };
			string[] errMsg = { @"Diese Fläche ist einem Versuch als Standort zugewiesen",
									  @"Dieser Fläche ist eine Arbeit zugewiesen",
									};

			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					con.Open();
					for ( int n=0;n<sqlCmds.Length; n++)
					{ 
						using (SQLiteCommand cmd = new SQLiteCommand(sqlCmds[n], con))
						{ 
							cmd.Parameters.Add(new SQLiteParameter("@FlaecheId", flaecheId));
						
							using (SQLiteDataReader reader = cmd.ExecuteReader())
							{
								if (reader.Read())
								{
									ErrorMsg = errMsg[n];
									return false;
								}
							}
						}
					}
					return true;
					
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}

		}

		public static bool DeleteFlaeche( int flaechenId)
		{
			if (!CanDeleteFlaeche(flaechenId))
				return false;

			string sqlCmd = @"DELETE FROM Flaechen
									WHERE FlaeId=@FlaeId;";

			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{	
					con.Open();
					using (SQLiteCommand cmd = new SQLiteCommand(sqlCmd, con))
					{ 
						cmd.Parameters.AddWithValue("@FlaeId", flaechenId);

						int n = cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}

			return true;
		}


		#endregion

		#region Versuch- Flächenzuordnung
		
		public  static DataTable GetFlaechenzuordnungTable(int versuchId, string  flaechenTyp)
		{
			string limFlaeche = string.IsNullOrWhiteSpace(flaechenTyp) ? "%" : flaechenTyp[0] + "%";

			string cmdRaw =
			@"SELECT 
				f.FlaeId,
				f.FlaeBez,
				f.FlaeTyp,
				EXISTS (SELECT 1
							FROM VxF vxf
							WHERE vxf.Vid = @VersuchId AND 
									vxf.Fid = f.FlaeId
				)
				AS doAssign
			FROM Flaechen f 
			WHERE f.FlaeTyp LIKE @FTyp
			Order By f.FlaeTyp, 
						substr(FlaeBez,1,1), cast(substr(FlaeBez,2) as integer)";
			//f.FlaeBez;";



			ErrorMsg = null;
			DataTable dt = null;
			try {
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					using (SQLiteCommand sqlCmd = new SQLiteCommand(cmdRaw, con))
					{ 
						sqlCmd.Parameters.AddWithValue("@VersuchId", versuchId);
						sqlCmd.Parameters.AddWithValue("@FTyp", limFlaeche);

						SQLiteDataAdapter da = new SQLiteDataAdapter(sqlCmd);
						dt = new DataTable();
						da.Fill(dt);
					}
				}
			}
			catch(Exception e)
			{
				ErrorMsg = e.Message;
				return null;
			}

			return dt;


		}


		public static bool ClearFlaechenzuordnungVersuch(int id_Versuch)
		{
			string sqlRemove = 
			@"DELETE from VxF
			WHERE Vid=@Vid;";

			ErrorMsg = null;
			
			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{	
					con.Open();

					using (SQLiteCommand cmd = new SQLiteCommand(sqlRemove, con))
					{ 
						cmd.Parameters.AddWithValue("@Vid", id_Versuch);
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}

			return true;
		}

		public static bool UpdateFlaechenzuordnungVersuch(Versuch v)
		{
			string sqlAdd = 
			@"INSERT INTO VxF ( [Vid], [Fid]) VALUES(@Vid, @Fid);";

			ErrorMsg = null;

			if (!ClearFlaechenzuordnungVersuch(v.Id))
				return false;


			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{	
					con.Open();
					using (SQLiteCommand cmd = new SQLiteCommand(sqlAdd, con))
					{
						foreach (int fid in v.IdsStandorte)
						{
							cmd.Parameters.AddWithValue("@Vid", v.Id);
							cmd.Parameters.AddWithValue("@Fid", fid);
							cmd.ExecuteNonQuery();
						}
					}
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}

			return true;
		}

		#endregion

		#region Aktionen

		public static DataTable GetArbeitskategorien()
		{ 
			string cmdRaw =
			@"SELECT *	FROM Arbeitskategorien
			ORDER By Kategorie ASC;";

			ErrorMsg = null;
			DataTable dt = null;

			try {
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					using (SQLiteCommand sqlCmd = new SQLiteCommand(cmdRaw, con))
					{ 
						SQLiteDataAdapter da = new SQLiteDataAdapter(sqlCmd);
						dt = new DataTable();
						da.Fill(dt);
					}
				}
			}
			catch(Exception e)
			{
				ErrorMsg = e.Message;
				return null;
			}

			return dt;		
		
		
		}
		
		public static DataTable GetArbeitenWithVersuch(int VersuchId)
		{
			string cmdRaw =
			@"SELECT
					a.ArbeitId, 
					a.Datum,
					a.Person,
					a.Aktion,
					a.Notizen,
					p.Name || ', ' || p.Vorname AS PersonName,
					v.VerBez
			FROM Aktionen a
			LEFT JOIN
					Personen p ON a.Person = p.PersonId
			LEFT JOIN
					Versuche v ON a.Versuch = v.VersuchId
			WHERE a.Versuch = @VersuchId
			ORDER By Datum ASC;";

			ErrorMsg = null;
			DataTable dt = null;

			try {
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					using (SQLiteCommand sqlCmd = new SQLiteCommand(cmdRaw, con))
					{ 
						sqlCmd.Parameters.AddWithValue("@VersuchId", VersuchId);
						SQLiteDataAdapter da = new SQLiteDataAdapter(sqlCmd);
						dt = new DataTable();
						da.Fill(dt);
					}
				}
			}
			catch(Exception e)
			{
				ErrorMsg = e.Message;
				return null;
			}

			return dt;
		}

		public static string GetArbeitenMailData(Aktion aktion)
		{
			//string cmdRaw =
			//@"SELECT
			//		a.Datum,
			//		a.Person,
			//		a.Aktion,
			//		a.Notizen,
			//		p.Name || ', ' || p.Vorname AS PersonName,
			//		v.VerBez
			//FROM Aktionen a
			//LEFT JOIN
			//		Personen p ON a.Person = p.PersonId
			//LEFT JOIN
			//		Versuche v ON a.Versuch = v.VersuchId
			//WHERE a.ArbeitId = @ArbeitId;";
			string cmdRaw =
			@"SELECT
					v.VerBez,
					a.Datum,
					a.Aktion,
					a.Notizen
				FROM Aktionen a
			LEFT JOIN
					Versuche v ON a.Versuch = v.VersuchId
			WHERE a.ArbeitId = @ArbeitId;";

			ErrorMsg = null;
			string txt="";
			string nl = Environment.NewLine;

			try {
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					con.Open();

					using (SQLiteCommand sqlCmd = new SQLiteCommand(cmdRaw, con))
					{ 
						sqlCmd.Parameters.AddWithValue("@ArbeitId", aktion.AktionId);
						using (SQLiteDataReader reader = sqlCmd.ExecuteReader())
						{
							if (reader.Read())
							{

								txt = nl + nl + "Versuch: " + GetColString(reader, "VerBez");
								txt += nl + "Datum: " + GetColString(reader, "Datum");
								txt += nl + GetColString(reader,"Aktion");
								txt += nl + GetColString(reader, "Notizen");
							}
							else
								txt = null;
							}
					}
				}
			}
			catch(Exception e)
			{
				ErrorMsg = e.Message;
				return null;
			}

			return txt;
		}


		public static DataTable GetArbeitenWithFlaeche(int FlaecheId)
		{
			string cmdRaw =
			@"SELECT
					a.ArbeitId, 
					a.Datum,
					a.Person,
					a.Aktion,
					a.Notizen,
					p.Name || ', ' || p.Vorname AS PersonName
			FROM Aktionen a
			LEFT JOIN
					Personen p ON a.Person = p.PersonId
			WHERE a.Flaeche = @FlaecheId AND a.Versuch = 0
			ORDER By Datum ASC;";

			ErrorMsg = null;
			DataTable dt = null;

			try {
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					using (SQLiteCommand sqlCmd = new SQLiteCommand(cmdRaw, con))
					{ 
						sqlCmd.Parameters.AddWithValue("@FlaecheId", FlaecheId);
						SQLiteDataAdapter da = new SQLiteDataAdapter(sqlCmd);
						dt = new DataTable();
						da.Fill(dt);
					}
				}
			}
			catch(Exception e)
			{
				ErrorMsg = e.Message;
				return null;
			}

			return dt;
		}

		private static DataTable GetExtArbeitenFuerVersuchTable(DateTime startZeit, DateTime endZeit, List<int> flaechenIds, List<string> aktions, string searchVersuchText, string searchNotizenText)
		{
			string cmdSelect =
			@"SELECT
					a.ArbeitId, 
					a.Datum,
					a.Aktion,
					a.Notizen,
					v.VerBez,
					GROUP_CONCAT(f.FlaeBez) As Standorte
			FROM Aktionen a
			LEFT JOIN
					Versuche v ON a.Versuch = v.VersuchId
			LEFT JOIN VxF x ON v.VersuchId = x.Vid
			LEFT JOIN Flaechen f ON f.FlaeId= x.Fid";


			StringBuilder sqlWhere = new StringBuilder(" ");

			sqlWhere.Append($"WHERE  a.Versuch > 0 AND (strftime('%s', Datum) BETWEEN strftime('%s','{startZeit:yyyy-MM-dd}') AND strftime('%s','{endZeit:yyyy-MM-dd}'))");

			if (flaechenIds != null)
			{
				if (flaechenIds.Count > 0)
				{
					sqlWhere.Append($"AND (f.FlaeId IN ({flaechenIds[0]}");
					for (int p = 1; p < flaechenIds.Count; p++)
						sqlWhere.Append($",{flaechenIds[p]}");
					sqlWhere.Append($"))");
				}
			}

			if (aktions != null)
			{
				if (aktions.Count > 0)
				{
					sqlWhere.Append($"AND (a.Aktion IN ('{aktions[0]}'");
					for (int p = 1; p < aktions.Count; p++)
						sqlWhere.Append($",'{aktions[p]}'");
					sqlWhere.Append($"))");
				}
			}

			sqlWhere.Append($"AND (a.Notizen LIKE @NSearch) AND (v.VerBez LIKE @VSearch) ");
			string nSearch = (string.IsNullOrEmpty(searchNotizenText)) ? "%" : "%" + searchNotizenText + "%";
			string vSearch = (string.IsNullOrEmpty(searchVersuchText)) ? "%" : "%" + searchVersuchText + "%";

			string sqlGroupAndOrder = $"GROUP BY a.ArbeitId ORDER BY a.Datum; ";
			//ErrorMsg = null;
			DataTable dt = null;

			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					string sqlQuery = cmdSelect + sqlWhere.ToString() + sqlGroupAndOrder;
					using (SQLiteCommand sqlCmd = new SQLiteCommand(sqlQuery, con))
					{
						sqlCmd.Parameters.AddWithValue("@NSearch", nSearch);
						sqlCmd.Parameters.AddWithValue("@VSearch", vSearch);
						
						SQLiteDataAdapter da = new SQLiteDataAdapter(sqlCmd);
						dt = new DataTable();
						da.Fill(dt);
					}
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return null;
			}

			return dt;
		}
		
					


		private static DataTable GetExtArbeitenFuerFlaechenTable(DateTime startZeit, DateTime endZeit,  List<int> flaechenIds, List<string> aktions,  string searchNotizenText)
		{
			string cmdSelect =
			@"SELECT
					a.ArbeitId, 
					a.Datum,
					a.Aktion,
					a.Notizen,
					GROUP_CONCAT(f.FlaeBez) As Standorte
			FROM Aktionen a
			LEFT JOIN Flaechen f ON f.FlaeId= a.Flaeche";


			StringBuilder sqlWhere = new StringBuilder(" ");

			sqlWhere.Append($"WHERE  a.Flaeche > 0 AND (strftime('%s', Datum) BETWEEN strftime('%s','{startZeit:yyyy-MM-dd}') AND strftime('%s','{endZeit:yyyy-MM-dd}'))");
			if (flaechenIds != null)
			{
				if (flaechenIds.Count > 0)
				{
					sqlWhere.Append($"AND (a.Flaeche IN ({flaechenIds[0]}");
					for (int p = 1; p < flaechenIds.Count; p++)
						sqlWhere.Append($",{flaechenIds[p]}");
					sqlWhere.Append($"))");
				}
			}

			if (aktions != null)
			{
				if (aktions.Count > 0)
				{
					sqlWhere.Append($"AND (a.Aktion IN ('{aktions[0]}'");
					for (int p = 1; p < aktions.Count; p++)
						sqlWhere.Append($",'{aktions[p]}'");
					sqlWhere.Append($"))");
				}
			}

			sqlWhere.Append($"AND (a.Notizen LIKE @NSearch) ");
			string nSearch = (string.IsNullOrEmpty(searchNotizenText)) ? "%" : "%" + searchNotizenText + "%";


			string sqlGroupAndOrder = $"GROUP BY a.ArbeitId ORDER BY a.Datum; ";

			DataTable dt = null;

			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					string sqlQuery = cmdSelect + sqlWhere.ToString() + sqlGroupAndOrder;
					using (SQLiteCommand sqlCmd = new SQLiteCommand(sqlQuery, con))
					{
						sqlCmd.Parameters.AddWithValue("@NSearch", nSearch);
						SQLiteDataAdapter da = new SQLiteDataAdapter(sqlCmd);
						dt = new DataTable();
						da.Fill(dt);
					}
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return null;
			}

			return dt;
		}


		public static DataTable GetExtArbeitenTable(DateTime startZeit, DateTime endZeit, List<int> flaechenIds, List<string> aktions,
			string searchVersuchText, string searchNotizenText, EnSearchSort sortBy)
		{

			ErrorMsg = null;

			DataTable dtVersuchsarbeiten = GetExtArbeitenFuerVersuchTable(startZeit, endZeit, flaechenIds, aktions, searchVersuchText, searchNotizenText);
			if (dtVersuchsarbeiten == null)
				return null;

			if (string.IsNullOrWhiteSpace(searchVersuchText)) // bei Suche nach Versuchen keine Flächenarbeiten
			{ 
				DataTable dtFlaechenarbeiten = GetExtArbeitenFuerFlaechenTable(startZeit, endZeit,  flaechenIds, aktions,  searchNotizenText);
				if (dtFlaechenarbeiten == null)
					return null;

				// beide Tabellen vereinigen

				foreach (DataRow row in dtFlaechenarbeiten.Rows)
				{
					DataRow newRow = dtVersuchsarbeiten.NewRow();
					newRow["Datum"] = row["Datum"];
					newRow["Standorte"] = row["Standorte"];
					newRow["Aktion"] = row["Aktion"];
					newRow["Notizen"] = row["Notizen"];
				
					dtVersuchsarbeiten.Rows.Add(newRow);
				}
			}

			string cmdSort;
			switch (sortBy)
			{
				case EnSearchSort.Datum: cmdSort = "Datum"; break;
				case EnSearchSort.Versuch: cmdSort = "VerBez"; break;
				case EnSearchSort.Flaeche: cmdSort = "Standorte"; break;
				case EnSearchSort.Kategorie: cmdSort = "Aktion"; break;
				case EnSearchSort.Kultur: cmdSort = "Kultur"; break;
				default: cmdSort = "Datum"; break;
			}
			dtVersuchsarbeiten.DefaultView.Sort = cmdSort;
			DataTable dtResult = dtVersuchsarbeiten.DefaultView.ToTable();

			return dtResult;

		}

		public static bool InsertNewArbeit(Aktion a)
		{
			//int lastIndex = -1;
			ErrorMsg = null;
			string sqlCmd =
			@"INSERT INTO Aktionen ( [Versuch], [Flaeche], [Person],[Datum], [Aktion],[Notizen])
			  VALUES(@Versuch, @Flaeche, @Person, @Datum, @Aktion, @Notizen)";

			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					int n;
					con.Open();
					using (SQLiteCommand cmd = new SQLiteCommand(sqlCmd, con))
					{ 
						cmd.Parameters.AddWithValue("@Versuch", a.VersuchId);
						cmd.Parameters.AddWithValue("@Flaeche", a.FlaecheId);
						cmd.Parameters.AddWithValue("@Person", a.PersonId);
						cmd.Parameters.AddWithValue("@Datum", a.Datum);
						cmd.Parameters.AddWithValue("@Aktion", a.Typ);
						cmd.Parameters.AddWithValue("@Notizen", a.Notizen);
					
						n = cmd.ExecuteNonQuery();
					}

					if(n > 0)
					{
						using (SQLiteCommand cmd = new SQLiteCommand("Select last_insert_rowid();", con))
						{
							a.AktionId = Convert.ToInt32(cmd.ExecuteScalar());
						}
					}
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
			}
			
			return (a.AktionId > 0);

		}
		
		public static bool UpdateArbeit(Aktion a)
		{
			ErrorMsg = null;

			string sqlCmd = @"UPDATE Aktionen
									SET	Versuch=@Versuch,
											Flaeche=@Flaeche,
											Person=@Person,
											Datum=@Datum,
											Aktion=@Aktion,
											Notizen=@Notizen
									WHERE ArbeitId=@ArbeitId;";

			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{	
					con.Open();
					using (SQLiteCommand cmd = new SQLiteCommand(sqlCmd, con))
					{ 
						cmd.Parameters.AddWithValue("@Versuch", a.VersuchId);
						cmd.Parameters.AddWithValue("@Flaeche", a.FlaecheId);
						cmd.Parameters.AddWithValue("@Person", a.PersonId);
						cmd.Parameters.AddWithValue("@Datum", a.Datum);
						cmd.Parameters.AddWithValue("@Aktion", a.Typ);
						cmd.Parameters.AddWithValue("@Notizen", a.Notizen);
						cmd.Parameters.AddWithValue("@ArbeitId", a.AktionId);
						
						int n = cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}
			return true;
		}

		public static bool DeleteArbeit(int arbeitId)
		{
			ErrorMsg = null;

			string sqlCmd = @"DELETE FROM Aktionen
									WHERE ArbeitId=@ArbeitId;";

			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{	
					con.Open();
					using (SQLiteCommand cmd = new SQLiteCommand(sqlCmd, con))
					{ 
						cmd.Parameters.AddWithValue("@ArbeitId", arbeitId);

						int n = cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}
			return true;
		}

		#endregion

		#region Nachrichtenzuordnung

		public  static DataTable GetNachrichtenzuordnungTable( int personId,int versuchsJahr)
		{
			string limStart, limEnd;
			if(versuchsJahr!=0)
			{
				limStart = "v.Start";
				limEnd = "v.Ende";
			}
			else 
			{ 
				limStart = "0";
				limEnd = "3000";
			}

			string cmdFmt =
			@"SELECT 
				VersuchId,
				VerBez,
				v.Start,
				v.Ende,
				Personen.Name || ', ' || Personen.Vorname AS Versuchsleiter,
				
				GROUP_CONCAT(f.FlaeBez) As Standorte,
				EXISTS (SELECT 1
							FROM Nachrichten n
							WHERE n.VersuchId = v.VersuchId AND 
									n.PersonId = @PersonId
				)
				AS doNotify,
				EXISTS (SELECT 1
							FROM Nachrichten n
							WHERE n.VersuchId = v.VersuchId AND 
									n.PersonId = @PersonId AND
									n.PerMail = 1
				)
				AS PerMail
			FROM Versuche v
			LEFT JOIN Personen ON v.Leiter = Personen.PersonId
			LEFT JOIN VxF x ON v.VersuchId = x.Vid
			LEFT JOIN Flaechen f ON f.FlaeId= x.Fid  
			WHERE (@Jahr BETWEEN {0} AND {1})
			GROUP BY x.Vid;";

			string cmdRaw = String.Format(cmdFmt, limStart, limEnd);

			ErrorMsg = null;
			DataTable dt = null;
			try {
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					using (SQLiteCommand sqlCmd = new SQLiteCommand(cmdRaw, con))
					{ 
						sqlCmd.Parameters.AddWithValue("@PersonId", personId);
						sqlCmd.Parameters.AddWithValue("@Jahr", versuchsJahr);
						//sqlCmd.Parameters.AddWithValue("@FlTyp", limFlaeche);


						SQLiteDataAdapter da = new SQLiteDataAdapter(sqlCmd);
						dt = new DataTable();
						da.Fill(dt);
						FormatStandortColumn(dt);
					}
				}
			}
			catch(Exception e)
			{
				ErrorMsg = e.Message;
				return null;
			}

			return dt;


		}

		public static bool UpdateNachrichtenzuordnung(int personId,DataTable dt)
		{

		string sqlAdd = 
			@"INSERT OR REPLACE INTO Nachrichten ([Id], [PersonId], [VersuchId],[PerMail]) 
			VALUES(
				(select Id from Nachrichten where PersonId=@PersonId AND VersuchId = @VersuchId),
				@PersonId,
				@VersuchId,
				@PerMail
			);";


			string sqlRemove = 
			@"DELETE from Nachrichten
			WHERE PersonId= @PersonId AND VersuchId=@VersuchId;";

			ErrorMsg = null;
			SQLiteCommand cmd;

			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{	
					con.Open();		

					foreach(DataRow row in dt.Rows)
					{
						if (row.RowState == DataRowState.Unchanged)
							continue;

						if(  Convert.ToBoolean(row["doNotify"]) == false)
							cmd = new SQLiteCommand(sqlRemove, con);
						else
							cmd = new SQLiteCommand(sqlAdd, con);
						

						cmd.Parameters.AddWithValue("@PersonId", personId);
						cmd.Parameters.AddWithValue("@VersuchId", Convert.ToInt32(row["VersuchId"]));
						cmd.Parameters.AddWithValue("@PerMail", Convert.ToInt32(row["PerMail"]));

						cmd.ExecuteNonQuery();
						
					}
					con.Close();
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}

			return true;
		}

		public static DataTable GetMeineNachrichtenTable(int personId)
		{
			string sql =
			@"SELECT 
				Datum, 
				Aktion, 
				Notizen, 
				v.VerBez, 
				GROUP_CONCAT(f.FlaeBez) As Standorte,
				a.Versuch,
				p.Name||', ' || p.Vorname AS Akteur
			FROM Aktionen a
			JOIN VERSUCHE v On a.Versuch= v.VersuchId
			JOIN Personen p On p.PersonId = a.Person
			LEFT JOIN VxF x ON v.VersuchId = x.Vid
			LEFT JOIN Flaechen f ON f.FlaeId= x.Fid  
			JOIN Nachrichten n
			Where n.VersuchId = a.Versuch and n.PersonId=@PersonId
			GROUP BY a.ArbeitId
			Order By Datum DESC;";
			
			ErrorMsg = null;
			DataTable dt = null;
			try {
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					using (SQLiteCommand sqlCmd = new SQLiteCommand(sql, con))
					{ 
						sqlCmd.Parameters.AddWithValue("@PersonId", personId);

						SQLiteDataAdapter da = new SQLiteDataAdapter(sqlCmd);
						dt = new DataTable();
						da.Fill(dt);
						FormatStandortColumn(dt);
					}
				}
			}
			catch(Exception e)
			{
				ErrorMsg = e.Message;
				return null;
			}

			return dt;
		}

		public static bool ClearNachrichtenzuordnungVersuch(int versuchId)
		{
			string sqlRemove = 
			@"DELETE from Nachrichten
			WHERE VersuchId=@VersuchId;";

			ErrorMsg = null;
			
			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{	
					con.Open();

					using (SQLiteCommand cmd = new SQLiteCommand(sqlRemove, con))
					{ 
						cmd.Parameters.AddWithValue("@VersuchId", versuchId);
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}

			return true;
		}


		public static bool ClearNachrichtenzuordnungPerson(int personId)
		{
			string sqlRemove = 
			@"DELETE from Nachrichten
			WHERE PersonId=@PersonId;";

			ErrorMsg = null;
			
			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{	
					con.Open();

					using (SQLiteCommand cmd = new SQLiteCommand(sqlRemove, con))
					{ 
						cmd.Parameters.AddWithValue("@VersuchId", personId);
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return false;
			}

			return true;
		}

		public static List<int> GetMailPersonsForAction(int versuchId)
		{
			List<int> personIds = new List<int>();
			string cmdRaw =
			@"SELECT PersonId	FROM Nachrichten
			WHERE VersuchId=@VersuchId AND PerMail=1;";
			
			try
			{
				using (SQLiteConnection con = new SQLiteConnection(cs))
				{
					con.Open();

					using (SQLiteCommand cmd = new SQLiteCommand(cmdRaw,con))
					{
					
						cmd.Parameters.AddWithValue("@VersuchId", versuchId);
			
						using (SQLiteDataReader reader = cmd.ExecuteReader())
						{ 
							while (reader.Read())
							{
								personIds.Add(reader.GetInt32(reader.GetOrdinal("PersonId"))); 
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				return null;
			}			

			return personIds;

		}

		#endregion

		#region helper

		private static void FormatStandortColumn(DataTable dt)
		{
			foreach(DataRow row in dt.Rows)
			{
				row["Standorte"] = ((string)row["Standorte"]).Replace(",", ", ");
			}

		}

		private static string  GetColString(SQLiteDataReader reader, string colName)
		{
			try 
			{
				int colId = reader.GetOrdinal(colName);
				if (reader.IsDBNull(colId))
					return string.Empty;

				switch (reader.GetDataTypeName(colId))
				{
					case "TEXT": return reader.GetString(colId);
					case "DATETIME":return reader.GetDateTime(colId).ToShortDateString();
					case "INTEGER": return reader.GetInt32(colId).ToString();
					default:return string.Empty;




				}
			}
			catch
			{ return string.Empty;}
			
		}

		#endregion

	}
}

