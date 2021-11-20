using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace dabaschlak
{
	class VmAbfrageKategorien:VmBase
	{
		#region Variable

		DataTable _dtKategorien;

		bool _alleKategorien;
		bool _keineKategorien;


		string _headerKategorien = "alle";

		RelayCommand _alleKategorienCommand;
		RelayCommand _keineKategorienCommand;


		#endregion

		#region Construction + Init

		public VmAbfrageKategorien()
		{
			Init();
		}

		void Init()
		{
			DataTableKategorien = CreateDataTable();

			_alleKategorienCommand = new RelayCommand(param => this.SetAlleKategorien());
			_keineKategorienCommand = new RelayCommand(param => this.SetKeineKategorien());

		}

		DataTable CreateDataTable()
		{
			DataTable dt = SqlAccess.GetArbeitskategorien();

			dt.Columns.Add("Checked", typeof(bool));

			foreach (DataRow row in dt.Rows)
			{
				row["Checked"] = true;
			}
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
			if (_dtKategorien == null)
				return;

			int num = 0;

			foreach (DataRow row in _dtKategorien.Rows)
			{
				if (Convert.ToBoolean(row["Checked"]))
					num++;
			}

			_keineKategorien = _alleKategorien = false;

			if (num == 0)
			{
				_keineKategorien = true;
				HeaderAuswahlKategorien = "keine Aktionen ausgewählt";
				return;
			}

			if (num == _dtKategorien.Rows.Count)
			{
				_alleKategorien = true;
				HeaderAuswahlKategorien = "alle";
				return;
			}


			_headerKategorien = "";
			string s = "";
			num = 0;

			foreach (DataRow row in _dtKategorien.Rows)
			{
				if (Convert.ToBoolean(row["Checked"]))
				{
					num++;
					if (num == 1)
						s = Convert.ToString(row["Kategorie"]);
					if (num == 2)
						s = s + " ...";
				}
			}
			if (!String.IsNullOrWhiteSpace(s))
			{
				if (String.IsNullOrWhiteSpace(_headerKategorien))
					HeaderAuswahlKategorien = s;
				else
					HeaderAuswahlKategorien += $" + {s}";
			}

		}

		#endregion

		#region Properties

		public DataTable DataTableKategorien
		{
			get { return _dtKategorien; }
			set
			{
				_dtKategorien = value;
				_dtKategorien.RowChanged += Row_Changed;

				OnPropertyChanged("DataTableKategorien");
			}
		}

		public string HeaderAuswahlKategorien
		{
			get { return _headerKategorien; }
			set
			{
				_headerKategorien = value;
				OnPropertyChanged("HeaderAuswahlKategorien");
				OnPropertyChanged("HeaderColor");
			}
		}

		public SolidColorBrush HeaderColor
		{
			get
			{
				return (_keineKategorien) ? Brushes.Red : Brushes.Yellow;
			}
		}

		public List<string> Kategorienliste // gibt  für 'alle' Null, für 'gar keine' eine leere Liste zurück
		{
			get
			{
				if (_alleKategorien)
					return null;

				List<string> kat = new List<string>();

				foreach (DataRow row in _dtKategorien.Rows)
				{
					if (Convert.ToBoolean(row["Checked"]) == true)
						kat.Add(Convert.ToString(row["Kategorie"]));
				}

				return kat;
			}
		}

		public bool AlleKategorien
		{
			get { return _alleKategorien; }
		}

		public bool KeineKategorien
		{
			get { return _keineKategorien; }
		}

		#endregion

		#region Commands

		public ICommand AlleKategorienCommand { get { return _alleKategorienCommand; } }
		public ICommand KeineKategorienCommand { get { return _keineKategorienCommand; } }


		void SetAlleKategorien(bool setTo)
		{
			DataTable newTable = DataTableKategorien.Copy();

			foreach (DataRow row in newTable.Rows)
			{
				row["Checked"] = setTo;
			}

			DataTableKategorien = newTable;
			CheckAuswahl();
		}

		void SetKeineKategorien()
		{
			SetAlleKategorien(false);
		}

		void SetAlleKategorien()
		{
			SetAlleKategorien(true);
		}

		#endregion
	}
}
