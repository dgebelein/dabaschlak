using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace dabaschlak
{
	class VmAbfrageVersuchsleiter : VmBase
	{
		#region Variable
		//Dictionary<int, string> _dictUsers;
		//List<string> _userNames;
		DataTable _dtVersuchsleiter;

		bool _allUsers;
		bool _noUsers;
		string _headerVersuchsleiter="alle";

		RelayCommand _allUsersCommand;
		RelayCommand _noUsersCommand;

		#endregion

		#region Construction + Init

		public VmAbfrageVersuchsleiter()
		{
			Init();
		}

		void Init()
		{
			DataTableVersuchsleiter = CreateDataTable();

			_allUsersCommand = new RelayCommand(param => this.SetAllUsers());
			_noUsersCommand = new RelayCommand(param => this.SetNoUsers());
		}

		DataTable CreateDataTable()
		{
			//DataTable dt = new DataTable();
			//_dictUsers = SqlAccess.GetUserDict();
			//_userNames = _dictUsers.Values.ToList();
			
			DataTable dt = SqlAccess.GetPersonsTable();

			dt.Columns.Add("Checked", typeof(bool));
			dt.Columns.Add("Person", typeof(string));
			foreach (DataRow row in dt.Rows)
			{
				string person = Convert.ToString(row["Name"]) + ", " + Convert.ToString(row["Vorname"]);
				row["Person"] = person;
				row["Checked"] = true;
			}
			//dt.RowChanged += Row_Changed;
			CheckAuswahl();

			return dt;
		}

		#endregion

		#region Bearbeitung
		private void Row_Changed(object sender, DataRowChangeEventArgs e)
		{
			CheckAuswahl();
		}

		void CheckAuswahl()
		{
			if (_dtVersuchsleiter == null)
				return;

			_allUsers = _noUsers = false;
			int num = 0;
			string s="";
			foreach (DataRow row in _dtVersuchsleiter.Rows)
			{
				if (Convert.ToBoolean(row["Checked"]) == true)
				{
					num++;
					if (num == 1)
						s = Convert.ToString(row["Name"]);

					if (num == 2)
						s = s + " ...";
				}
			}

			if (num == 0)
			{
				_noUsers = true;
				HeaderAuswahlVersuchsleiter = "kein Versuchsleiter ausgewählt";
				return;
			}

			if (num == _dtVersuchsleiter.Rows.Count)
			{
				_allUsers = true;
				HeaderAuswahlVersuchsleiter = "alle";
				return;
			}

			HeaderAuswahlVersuchsleiter = s;

		}

		#endregion

		#region Properties

		public DataTable DataTableVersuchsleiter
		{
			get { return _dtVersuchsleiter; }
			set
			{
				_dtVersuchsleiter = value;
				_dtVersuchsleiter.RowChanged += Row_Changed;

				OnPropertyChanged("DataTableVersuchsleiter");
			}
		}

		public string HeaderAuswahlVersuchsleiter
		{
			get{ return _headerVersuchsleiter;}
			set
			{
				_headerVersuchsleiter = value;
				OnPropertyChanged("HeaderAuswahlVersuchsleiter");
				OnPropertyChanged("HeaderColor");
			}
		}

		public SolidColorBrush HeaderColor
		{
			get
			{
				return (_noUsers)? Brushes.Red: Brushes.Yellow;
			}
		}

		public List<int> PersonenIdListe // gibt  für 'alle' Null, für 'gar keine' eine leere Liste zurück
		{
			get
			{
				if (AllUsers)
					return null;

				List<int> personen= new List<int>();

				foreach (DataRow row in _dtVersuchsleiter.Rows)
				{
					if (Convert.ToBoolean(row["Checked"]) == true)
						personen.Add(Convert.ToInt32(row["PersonId"]));
				}

				return personen;
			}
		}

		public bool AllUsers
		{
			get { return _allUsers; }
		}

		public bool NoUsers
		{
			get { return _noUsers; }
		}

		#endregion

		#region Commands

		public ICommand AllUsersCommand { get { return _allUsersCommand; } }
		public ICommand NoUsersCommand { get { return _noUsersCommand; } }

		void SetUsers(bool allUsers)
		{
			DataTable newTable = DataTableVersuchsleiter.Copy();

			foreach (DataRow row in newTable.Rows)
			{
				row["Checked"] = allUsers;
			}
			DataTableVersuchsleiter = newTable;
			CheckAuswahl();
		}

		void SetAllUsers()
		{
			SetUsers(true);
		}

		void SetNoUsers ()
		{
			SetUsers(false);
		}

		#endregion
	}
}
