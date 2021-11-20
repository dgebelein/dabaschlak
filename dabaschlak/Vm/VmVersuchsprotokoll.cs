using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace dabaschlak
{
	class VmVersuchsprotokoll:VmBase
	{
		#region variable
		
		VmAlleVersuche _vmVersuche;
		int _idVersuch;
		DataTable _dtArbeiten;
		Versuch _dataVersuch;
		string _browserFilename;
		
		RelayCommand _printCommand;
		RelayCommand _saveCommand;
		RelayCommand _exportCommand;



		#endregion

		#region construction+ init

		public VmVersuchsprotokoll()
		{
			Init();

			_vmVersuche = new VmAlleVersuche(this);
			_vmVersuche.SelectionChanged += VersuchSelected;
			ViewVisual = new ViewVersuchsprotokoll();
		}
		void Init()
		{
			_printCommand = new RelayCommand(param => this.PrintReport());
			_saveCommand = new RelayCommand(param => this.SaveReport());
			_exportCommand = new RelayCommand(param => this.ExportReport());
		}

		#endregion

		#region DataIO

		private void VersuchSelected(object source, VersuchArgs e)
		{
			_idVersuch = e.IdVersuch;

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
				((ViewVersuchsprotokoll)ViewVisual)._aweBrowser.Source = uri;
			}
		}

		private bool ReadData()
		{
			_dtArbeiten = SqlAccess.GetArbeitenWithVersuch(_idVersuch);
			if (_dtArbeiten == null)
			{
				MsgWindow.Show("Aktionen konnten nicht gelesen werden", SqlAccess.ErrorMsg, MessageLevel.Error);
				return false;
			}

			_dataVersuch = _vmVersuche.GetVersuchFromId(_idVersuch);
			if (_dataVersuch == null)
			{
				MsgWindow.Show("Versuchsdaten konnten nicht gelesen werden", SqlAccess.ErrorMsg, MessageLevel.Error);
				return false;
			}

			return true;

		}

		#endregion
		
		#region Properties

		public object DCAuswahlListe 
		{
			get { return  (object)_vmVersuche; }
		}



		#endregion

		#region Report erstellen

		private string GetReportFile()
		{
			HtmlCreator creator = new HtmlCreator();
			List<int> colWidth = new List<int>() {7,20,0,20};
			List<string> headers = new List<string>();
			List<List<string>> rows = new List<List<string>>();

			headers.Add("Datum");
			headers.Add("Aktion");
			headers.Add("Notizen");
			headers.Add("Arbeitskraft");
			
			foreach (DataRow dtRow in _dtArbeiten.Rows)
			{
				List<string> htmlRow = new List<string>();
				DateTime dt =  DateTime.Parse(Convert.ToString(dtRow["Datum"]));
				htmlRow.Add( dt.ToString("dd.MM.yyyy"));

				htmlRow.Add( Convert.ToString(dtRow["Aktion"]));
				htmlRow.Add(Convert.ToString(dtRow["Notizen"]));//.Replace("\r\n", "<br/>"));
				htmlRow.Add( Convert.ToString(dtRow["PersonName"]));
				rows.Add(htmlRow);
			}


			return creator.GetVersuchsprotokoll(_dataVersuch, headers, rows, colWidth);
		}

		#endregion

		#region Print, Save, Export

		public ICommand PrintCommand{	get {	return _printCommand; }	}
		public ICommand SaveCommand {	get {	return _saveCommand; }	}
		public ICommand ExportCommand{get {	return _exportCommand; }}


		void PrintReport()
		{
			((ViewVersuchsprotokoll)(ViewVisual))._aweBrowser.Print();
		}

		void SaveReport()
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Html Datei|*.html";
			sfd.Title = "Versuchsprotokoll als Datei speichern";
			sfd.FileName = _dataVersuch.VerBez.Replace(" ", "_");
			sfd.ShowDialog();

			if(sfd.FileName!="")
			{
				try { 
					File.Copy(_browserFilename, sfd.FileName, true);
				}
				catch(Exception e)
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
			((ViewVersuchsprotokoll)(ViewVisual))._aweBrowser.SelectAll();
			((ViewVersuchsprotokoll)(ViewVisual))._aweBrowser.Copy();
			((ViewVersuchsprotokoll)(ViewVisual))._aweBrowser.ExecuteJavascriptWithResult("document.getSelection().removeAllRanges()");// Markierung weg
	}
		
	#endregion
	}
}
