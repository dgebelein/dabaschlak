using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace dabaschlak
{
	public enum EnSearchTarget
	{
		Aktion,
		Versuch
	}

	public enum EnSearchSort
	{
		Datum,
		Versuch,
		Flaeche,
		Person,
		Kategorie,
		Kultur
	}

	class VmAbfrage:VmBase
	{
		#region variable

		VmAbfrageVersuchsleiter _vmAbfrageVersuchsleiter;
		VmAbfrageflaechen _vmAbfrageflaechen;
		VmAbfrageTexte _vmAbfrageTexte;
		VmAbfrageKategorien _vmAbfragekategorien;


		EnSearchTarget _searchTarget;
		EnSearchSort _searchSortVersuche = EnSearchSort.Datum;
		EnSearchSort _searchSortAktionen = EnSearchSort.Datum;

		bool _alleJahre;
		int _startJahr;
		int _endJahr;
		DateTime _startZeitraum;
		DateTime _endZeitraum;

		List<int> _searchPersonenIds;
		List<int> _searchFlaechenIds;
		List<string> _searchVersuche;
		List<string> _searchKategorien;
		string _searchKulturText;
		string _searchVersuchText;
		string _searchAktionVersuchText;
		string _searchAktionNotizenText;


		DataTable _dtVersuche;
		DataTable _dtAktionen;	
		string _browserFilename;


		List<int> _versuchsjahre;
		
		RelayCommand _printCommand;
		RelayCommand _saveCommand;
		RelayCommand _exportCommand;
		RelayCommand _executeCommand;




		#endregion

		#region construction + init
		public VmAbfrage()
		{
			Init();
			SearchTarget = EnSearchTarget.Aktion;
			ViewVisual = new ViewAbfrage();
		}

		void Init()
		{
			_printCommand = new RelayCommand(param => this.PrintReport());
			_saveCommand = new RelayCommand(param => this.SaveReport());
			_exportCommand = new RelayCommand(param => this.ExportReport());
			_executeCommand = new RelayCommand(param => this.ExecuteSearch(), param => this.CanExecute());

			//_searchPersonenIds = new List<int>();
			//_searchFlaechenIds = new List<int>();
			_searchVersuche = new List<string>();			
			_searchKategorien = new List<string>();
			_startJahr = _endJahr = DateTime.Now.Year;
			_startZeitraum = Convert.ToDateTime("1/1/" + DateTime.Now.Year);
			_endZeitraum = Convert.ToDateTime("31/12/" + DateTime.Now.Year);


			_versuchsjahre = new List<int>(20);
			for (int n = 2010; n <= 2030; n++)
				_versuchsjahre.Add(n);

			_vmAbfrageVersuchsleiter = new VmAbfrageVersuchsleiter();
			_vmAbfrageflaechen = new VmAbfrageflaechen();
			_vmAbfragekategorien = new VmAbfrageKategorien();
			_vmAbfrageTexte = new VmAbfrageTexte();

		}

		#endregion

		#region Ausgabe erstellen

		private bool CanExecute()
		{
			if (_searchTarget == EnSearchTarget.Versuch)
			{
				return (this.CanApply && !_vmAbfrageVersuchsleiter.NoUsers && !_vmAbfrageflaechen.KeineFlaechen);
			}
			else
			{
				return (this.CanApply && !_vmAbfrageflaechen.KeineFlaechen &&!_vmAbfragekategorien.KeineKategorien);
			}
		}

		private bool ReadData()
		{
			bool succ;
			if(_searchTarget== EnSearchTarget.Versuch)
			{
				succ = ReadDataVersuche();
			}
			else
			{
				succ = ReadDataAktionen();
			}

			if (!succ)
			{
				MsgWindow.Show("Abfrage konnte nicht durchgeführt werden", SqlAccess.ErrorMsg, MessageLevel.Error);
			}
			return succ;

		}

		private bool ReadDataVersuche()
		{

			_searchPersonenIds = _vmAbfrageVersuchsleiter.PersonenIdListe;
			_searchFlaechenIds = _vmAbfrageflaechen.FlaechenIdListe;
			_searchVersuchText = _vmAbfrageTexte.KulturSuchText.Trim();
			_searchKulturText = _vmAbfrageTexte.VersuchSuchText.Trim();

			_dtVersuche = (_alleJahre) ?
				SqlAccess.GetExtVersuchsTable(0, 0, _searchPersonenIds, _searchFlaechenIds, _searchKulturText, _searchVersuchText, _searchSortVersuche) :
				SqlAccess.GetExtVersuchsTable(_startJahr, _endJahr, _searchPersonenIds, _searchFlaechenIds, _searchKulturText, _searchVersuchText, _searchSortVersuche);

			if (_dtVersuche == null)
				return false;

			_dtVersuche.Columns.Add("Jahr", typeof(string));
			foreach (DataRow dr in _dtVersuche.Rows)
			{
				dr["Jahr"] = (Convert.ToInt32(dr["Start"]) == Convert.ToInt32(dr["Ende"])) ? dr["Start"] : dr["Start"] + " - " + dr["Ende"];
			}


			return true;
		}

		private bool ReadDataAktionen()
		{
			//_searchPersonenIds = _vmAbfrageVersuchsleiter.PersonenIdListe;
			_searchFlaechenIds = _vmAbfrageflaechen.FlaechenIdListe;
			_searchAktionVersuchText = _vmAbfrageTexte.AktionVersuchSuchText.Trim();
			_searchAktionNotizenText = _vmAbfrageTexte.AktionNotizenSuchText.Trim();
			_searchKategorien = _vmAbfragekategorien.Kategorienliste;

			_dtAktionen = SqlAccess.GetExtArbeitenTable(_startZeitraum, _endZeitraum, 
				_searchFlaechenIds, _searchKategorien, _searchAktionVersuchText, _searchAktionNotizenText, _searchSortAktionen);

			if (_dtAktionen == null)
				return false;

			//_dtVersuche.Columns.Add("Jahr", typeof(string));
			//foreach (DataRow dr in _dtVersuche.Rows)
			//{
			//	dr["Jahr"] = (Convert.ToInt32(dr["Start"]) == Convert.ToInt32(dr["Ende"])) ? dr["Start"] : dr["Start"] + " - " + dr["Ende"];
			//}


			return true;
		}

		private string GetReportFile()
		{
			if (_searchTarget == EnSearchTarget.Versuch)
			{
				return GetVersucheFile();
			}
			else
			{
				return GetAktionenFile();
			}

		}

		private string GetAktionenFile()
		{
			HtmlCreator creator = new HtmlCreator();
			List<int> colWidth = new List<int>() { 5, 15, 25, 10, 0 };
			List<string> headers = new List<string>();
			List<List<string>> rows = new List<List<string>>();

			headers.Add("Datum");
			headers.Add("Standort");
			headers.Add("Versuch");
			headers.Add("Aktion");
			headers.Add("Notizen");

			foreach (DataRow dtRow in _dtAktionen.Rows)
			{
				List<string> htmlRow = new List<string>
				{
					HtmlCreator.FormatDatum( Convert.ToString(dtRow["Datum"])),
					HtmlCreator.FormatStandorte(Convert.ToString(dtRow["Standorte"])),
					Convert.ToString(dtRow["VerBez"]),
					Convert.ToString(dtRow["Aktion"]),
					Convert.ToString(dtRow["Notizen"])
				};
				rows.Add(htmlRow);
			}

			return creator.GetAktionenAbfrage(headers, rows, colWidth);
		}

		private string GetVersucheFile()
		{
			HtmlCreator creator = new HtmlCreator();
			List<int> colWidth = new List<int>() { 0, 10, 20, 10,5 };
			List<string> headers = new List<string>();
			List<List<string>> rows = new List<List<string>>();

			headers.Add("Versuch");
			headers.Add("Jahr");
			headers.Add("Standort");
			headers.Add("Kulturpfl.");
			headers.Add("Versuchsleiter");

			foreach (DataRow dtRow in _dtVersuche.Rows)
			{
				List<string> htmlRow = new List<string>
				{
					Convert.ToString(dtRow["VerBez"]),
					Convert.ToString(dtRow["Jahr"]),
					Convert.ToString(dtRow["Standorte"]),
					Convert.ToString(dtRow["Kultur"]),
					Convert.ToString(dtRow["Versuchsleiter"])
				};
				rows.Add(htmlRow);
			}

			return creator.GetVersucheAbfrage(headers, rows, colWidth);
		}

		#endregion

		#region Properties
		public EnSearchTarget SearchTarget
		{
			get { return _searchTarget; }
			set 
			{
				_searchTarget = value;
				_vmAbfrageTexte.SearchTarget = _searchTarget;
				OnPropertyChanged("SearchTarget");
				OnPropertyChanged("VisAuswahlVersuche");
				OnPropertyChanged("VisAuswahlAktionen");
				OnPropertyChanged("HeaderSort");

				Validate();

			}

		}

		public Visibility VisAuswahlVersuche
		{
			get { return (_searchTarget == EnSearchTarget.Versuch) ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility VisAuswahlAktionen
		{
			get { return (_searchTarget == EnSearchTarget.Aktion) ? Visibility.Visible : Visibility.Collapsed; }
		}
		
		public List<int> Versuchsjahre
		{
			get { return _versuchsjahre;}

		}

		public bool AuswahlAlleJahre
		{
			get { return _alleJahre; }
			set
			{
				_alleJahre = value;
				Validate();
			}

		}

		public bool EnableAuswahlJahre
		{
			get { return !_alleJahre; }
		}

		public int StartJahr
		{
			get { return _startJahr; }
			set 
			{
				if (_startJahr == value)
					return;

				_startJahr = value;
				Validate();
			}
		}

		public int EndJahr
		{
			get { return _endJahr; }
			set 
			{
				if (_endJahr == value)
					return;

				_endJahr = value;
				Validate();
			}
		}

		public string HeaderAuswahlJahre
		{
			get
			{
				if (_alleJahre)
					return "alle";

				if (StartJahr == EndJahr)
					return $"{StartJahr}";

				return $"{StartJahr} - {EndJahr}";
			}
		}

		public SolidColorBrush ColorHeaderAuswahlJahre
		{
			get
			{
				return ((ContainsError("StartJahr") || ContainsError("EndJahr")) ? Brushes.Red : Brushes.Yellow);
			}
		}

		public DateTime StartZeitraum
		{
			get { return _startZeitraum; }
			set 
			{
				if (_startZeitraum == value)
					return;

				_startZeitraum = value;
				Validate();
			}
		}

		public DateTime EndZeitraum
		{
			get { return _endZeitraum; }
			set 
			{
				if (_endZeitraum == value)
					return;

				_endZeitraum = value;
				Validate();
			 }
		}

		public SolidColorBrush ColorHeaderAuswahlZeitraum
		{
			get
			{
				return ((ContainsError("StartZeitraum") || ContainsError("EndZeitraum")) ? Brushes.Red : Brushes.Yellow);
			}
		}

		public string HeaderAuswahlZeitraum
		{
			get { return _startZeitraum.ToString("dd.MM.yyyy") + " - " + _endZeitraum.ToString("dd.MM.yyyy"); }
		}

		public VmAbfrageVersuchsleiter VmVersuchsleiter
		{
			get { return _vmAbfrageVersuchsleiter; }
		}

		public VmAbfrageflaechen VmVersuchsflaechen
		{
			get { return _vmAbfrageflaechen; }
		}

		public VmAbfrageKategorien VmKategorien
		{

			get { return _vmAbfragekategorien; }
		}

		public VmAbfrageTexte VmSuchTexte
		{
			get { return _vmAbfrageTexte; }
		}

		public EnSearchSort SortVersuche
		{
			get { return _searchSortVersuche; }
			set
			{
				_searchSortVersuche = value;
				OnPropertyChanged("SortVersuche");
				OnPropertyChanged("HeaderSort");
			}
		}

		public EnSearchSort SortAktion
		{
			get { return _searchSortAktionen; }
			set
			{
				_searchSortAktionen = value;
				OnPropertyChanged("SortAktion");
				OnPropertyChanged("HeaderSort");
			}
		}

		public string HeaderSort
		{
			get
			{
				if (_searchTarget == EnSearchTarget.Versuch)
				{
					switch (_searchSortVersuche)
					{

						case EnSearchSort.Datum: return "Versuchsjahr";
						case EnSearchSort.Versuch:return "Versuchsbezeichnung";
						case EnSearchSort.Flaeche:return "Standort";
						case EnSearchSort.Person:return "Versuchsleiter";
						case EnSearchSort.Kultur:return "Kulturpflanze";
						default:return "?";
					}

				}
				else // Aktionen
					switch (_searchSortAktionen)
					{
						case EnSearchSort.Datum: return "Datum";
						case EnSearchSort.Flaeche: return "Standort";
						case EnSearchSort.Kategorie: return "Aktion";
						case EnSearchSort.Versuch: return "Versuchsbezeichnung";
						default: return "?";
					}
			}
		}
		#endregion

		#region Validierung

		void Validate()
		{
			ResetErrorList();
			if(_searchTarget== EnSearchTarget.Versuch)
			{ 
				if ( (!_alleJahre) && (EndJahr < StartJahr))
				{ 
					AddErrorMessage("EndJahr", "'bis' darf nicht kleiner als 'von' sein");
					AddErrorMessage("StartJahr", "'bis' darf nicht kleiner als 'von' sein");
				}
			}
			if (_searchTarget == EnSearchTarget.Aktion)
			{
				if ( (_startZeitraum >= _endZeitraum))
				{
					AddErrorMessage("StartZeitraum", "'bis' darf nicht kleiner als 'von' sein");
					AddErrorMessage("EndZeitraum", "'bis' darf nicht kleiner als 'von' sein");
				}
			}
			OnPropertyChanged("AuswahlAlleJahre");
			OnPropertyChanged("EnableAuswahlJahre");
			OnPropertyChanged("HeaderAuswahlJahre");
			OnPropertyChanged("ColorHeaderAuswahlJahre");
			OnPropertyChanged("StartJahr");
			OnPropertyChanged("EndJahr");
			OnPropertyChanged("HeaderAuswahlZeitraum");
			OnPropertyChanged("ColorHeaderAuswahlZeitraum"); 
			OnPropertyChanged("StartZeitraum");
			OnPropertyChanged("EndZeitraum");



		}
		#endregion


		#region Commands

		public ICommand PrintCommand{	get {	return _printCommand; }	}
		public ICommand SaveCommand {	get {	return _saveCommand; }	}
		public ICommand ExportCommand{get {	return _exportCommand; }}
		public ICommand ExecuteCommand{get{ return _executeCommand; }}



		void PrintReport()
		{
			((ViewAbfrage)(ViewVisual))._aweBrowser.Print();
		}

		void SaveReport()
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Html Datei|*.html";
			sfd.Title = "Abfrageergebnis als Datei speichern";
			//sfd.FileName = _dataVersuch.VerBez.Replace(" ", "_");
			sfd.ShowDialog();

			if (sfd.FileName != "")
			{
				try
				{
					File.Copy(_browserFilename, sfd.FileName, true);
				}
				catch (Exception e)
				{
					MsgWindow.Show("Versuchsprotokoll kann nicht als Datei gespeichert werden:", e.Message, MessageLevel.Error);
				}
			}

		}

		void ExportReport()
		{
			Microsoft.Office.Interop.Excel.Application excel = null;

			try
			{
				//Excel-Instanz erzeugen 
				excel = new Microsoft.Office.Interop.Excel.Application();

				// Arbeitsmappe ohne Vorlage erzeugen
				object missing = Missing.Value;
				excel.Workbooks.Add(missing);

				// aktives Workbook referenzieren
				Microsoft.Office.Interop.Excel.Workbook wb = excel.ActiveWorkbook;

				// Arbeitsblatt erzeugen und referenzieren
				if (wb.Worksheets.Count == 0)
					wb.Worksheets.Add(missing, missing, missing, missing);
				Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1];

				CopyBrowserCode();
				ws.Paste();
				// ActiveSheet.Paste

				// erst am Ende sichtbar machen, weil Programmabstürze drohen, wenn mit Excel gearbeitet wird, solange die Datenübertragung dauert
				excel.Visible = true;
			}
			catch (Exception)
			{
				if (excel != null)
					excel.Quit();
			}
		}

		void CopyBrowserCode()
		{
			((ViewAbfrage)(ViewVisual))._aweBrowser.SelectAll();
			((ViewAbfrage)(ViewVisual))._aweBrowser.Copy();
			((ViewAbfrage)(ViewVisual))._aweBrowser.ExecuteJavascriptWithResult("document.getSelection().removeAllRanges()");// Markierung weg
		}

		void ExecuteSearch()
		{

			if (!ReadData())
            return;

			_browserFilename = GetReportFile();
			if(string.IsNullOrEmpty(_browserFilename))
			{ 
				MsgWindow.Show("Protokoll konnte erstellt werden", "Fehler beim Schreiben in eine temporäre Datei", MessageLevel.Error);
			}
			else
			{ 
				var uri = new Uri(_browserFilename, UriKind.Absolute);
				((ViewAbfrage)ViewVisual)._aweBrowser.Source = uri;
			}
		}
		#endregion
	}
}
