using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace dabaschlak
{
	class VmAbfrageflaechen : VmBase
	{
		#region Variable

		DataTable _dtFlaechen;

		bool _alleFlaechen;
		bool _keineFlaechen;
		bool _alleAckerflaechen;
		bool _alleKlimakammern;
		bool _alleGewaechshaeuser;
		bool _alleStellflaechen;

		string _headerFlaechen = "alle";

		RelayCommand _alleFlaechenCommand;
		RelayCommand _keineFlaechenCommand;
		RelayCommand _alleAckerflaechenCommand;
		RelayCommand _alleKlimakammernCommand;
		RelayCommand _alleGewaechshaeuserCommand;
		RelayCommand _alleStellflaechenCommand;

		#endregion

		#region Construction + Init

		public VmAbfrageflaechen()
		{
			Init();
		}

		void Init()
		{
			DataTableVersuchsflaechen = CreateDataTable();

			_alleFlaechenCommand = new RelayCommand(param => this.SetAlleFlaechen());
			_keineFlaechenCommand = new RelayCommand(param => this.SetKeineFlaechen()); 
			_alleAckerflaechenCommand = new RelayCommand(param => this.SetAlleAckerflaechen()); 
			_alleKlimakammernCommand = new RelayCommand(param => this.SetAlleKlimakammern());
			_alleGewaechshaeuserCommand = new RelayCommand(param => this.SetAlleGewaechshaeuser());
			_alleStellflaechenCommand = new RelayCommand(param => this.SetAlleStellflaechen());
		}

		DataTable CreateDataTable()
		{
			DataTable dt = SqlAccess.GetFlaechenTable("");

			dt.Columns.Add("Checked", typeof(bool));
			
			foreach (DataRow row in dt.Rows)
			{
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
			if (_dtFlaechen == null)
				return;

			_alleFlaechen = _keineFlaechen = _alleAckerflaechen = _alleKlimakammern = _alleGewaechshaeuser = _alleStellflaechen = true;

			int num = 0;
			
			foreach (DataRow row in _dtFlaechen.Rows)
			{
				bool c = (Convert.ToBoolean(row["Checked"]));
				if (c)
					num++;
				else
				{ 
					switch (Convert.ToString(row["FlaeTyp"])[0])
					{
						case 'A': _alleAckerflaechen = false; break;
						case 'K': _alleKlimakammern = false; break;
						case 'G': _alleGewaechshaeuser = false; break;
						case 'S': _alleStellflaechen = false; break;
						default: break;
					}
				}
			}

			if (num == 0)
			{
				_keineFlaechen = true;
				HeaderAuswahlVersuchsflaechen = "keine Versuchsfläche ausgewählt";
				return;
			}
			else
				_keineFlaechen = false;

			if (num == _dtFlaechen.Rows.Count)
			{
				_alleFlaechen = true;
				HeaderAuswahlVersuchsflaechen = "alle";
				return;
			}
			else
				_alleFlaechen = false;

			_headerFlaechen = "";

			if (_alleAckerflaechen)
			{
				HeaderAuswahlVersuchsflaechen+= "Ackerflächen";
			}

			if (_alleKlimakammern)
			{
				if (String.IsNullOrWhiteSpace(_headerFlaechen))
					HeaderAuswahlVersuchsflaechen = "Klimakammern";
				else
					HeaderAuswahlVersuchsflaechen += " + Klimakammern";
			}
			if (_alleGewaechshaeuser)
			{
				if (String.IsNullOrWhiteSpace(_headerFlaechen))
					HeaderAuswahlVersuchsflaechen = "Gewächshäuser";
				else
					HeaderAuswahlVersuchsflaechen += " + Gewächshäuser";
			}

			if (_alleStellflaechen)
			{
				if (String.IsNullOrWhiteSpace(_headerFlaechen))
					HeaderAuswahlVersuchsflaechen = "Stellflächen";
				else
					HeaderAuswahlVersuchsflaechen += " + Stellflächen";
			}

			string s = "";
			num = 0;
			foreach (DataRow row in _dtFlaechen.Rows)
			{
				bool c = (Convert.ToBoolean(row["Checked"]));
				if (c)
				{ 
					switch (Convert.ToString(row["FlaeTyp"])[0])
					{
						case 'A': if (_alleAckerflaechen) continue; break;
						case 'K': if (_alleKlimakammern) continue; break;
						case 'G': if (_alleGewaechshaeuser) continue; break;
						case 'S': if (_alleStellflaechen) continue; break;
						default: break;
					}
					num++;
					if (num == 1)
						s = Convert.ToString(row["FlaeBez"]);
					if (num == 2)
						s = s + " ...";
				}
			}
			if (!String.IsNullOrWhiteSpace(s))
			{ 
				if (String.IsNullOrWhiteSpace(_headerFlaechen))
					HeaderAuswahlVersuchsflaechen = s;
				else
					HeaderAuswahlVersuchsflaechen += $" + {s}";
			}
			
		}

		#endregion

		#region Properties

		public DataTable DataTableVersuchsflaechen
		{
			get { return _dtFlaechen; }
			set
			{
				_dtFlaechen = value;
				_dtFlaechen.RowChanged += Row_Changed;

				OnPropertyChanged("DataTableVersuchsflaechen");
			}
		}

		public string HeaderAuswahlVersuchsflaechen
		{
			get { return _headerFlaechen; }
			set
			{
				_headerFlaechen = value;
				OnPropertyChanged("HeaderAuswahlVersuchsflaechen");
				OnPropertyChanged("HeaderColor");
			}
		}

		public SolidColorBrush HeaderColor
		{
			get
			{
				return (_keineFlaechen) ? Brushes.Red : Brushes.Yellow;
			}
		}

		public List<int> FlaechenIdListe // gibt  für 'alle' Null, für 'gar keine' eine leere Liste zurück
		{
			get
			{
				if (_alleFlaechen)
					return null;

				List<int> flaechen = new List<int>();

				foreach (DataRow row in _dtFlaechen.Rows)
				{
					if (Convert.ToBoolean(row["Checked"]) == true)
						flaechen.Add(Convert.ToInt32(row["FlaeId"]));
				}

				return flaechen;
			}
		}

		public bool AlleFlaechen
		{
			get { return _alleFlaechen; }
		}

		public bool KeineFlaechen
		{
			get { return _keineFlaechen; }
		}

		#endregion

		#region Commands

		public ICommand AlleFlaechenCommand { get { return _alleFlaechenCommand; } }
		public ICommand KeineFlaechenCommand { get { return _keineFlaechenCommand; } }
		public ICommand AlleAckerFlaechenCommand { get { return _alleAckerflaechenCommand; } }
		public ICommand AlleKlimakammernCommand { get { return _alleKlimakammernCommand; } }
		public ICommand AlleGewaechshaeuserCommand { get { return _alleGewaechshaeuserCommand; } }
		public ICommand AlleStellflaechenCommand { get { return _alleStellflaechenCommand; } }

		void SetFlaechen(string  flaechentyp)
		{
			DataTable newTable = DataTableVersuchsflaechen.Copy();

			foreach (DataRow row in newTable.Rows)
			{
				switch(flaechentyp)
				{
					case "" : row["Checked"] = false; break;
					case "*": row["Checked"] = true; break;
					default:	if (Convert.ToString(row["FlaeTyp"]) == flaechentyp)
									row["Checked"] = true;
								break;
				}
			}

			DataTableVersuchsflaechen = newTable;
			CheckAuswahl();
		}

		void SetKeineFlaechen()
		{
			SetFlaechen("");
		}

		void SetAlleFlaechen()
		{
			SetFlaechen("*");
		}


		void SetAlleAckerflaechen()
		{
			SetFlaechen("Ackerfläche");
		}

		void SetAlleKlimakammern()
		{
			SetFlaechen("Klimakammer");
		}

		void SetAlleGewaechshaeuser()
		{
			SetFlaechen("Gewächshaus");
		}

		void SetAlleStellflaechen()
		{
			SetFlaechen("Stellfläche");
		}


		#endregion
	}
}
