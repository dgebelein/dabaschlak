using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dabaschlak
{
	class VmNachrichtenzuordnung:VmBase
	{

		#region Variable

		DataTable _dtZuordnung;
		int _personId;	// weil 'Mitarbeiter' zwischen Lesen u. Update wechseln kann...

		bool _doFilterYears;
		int _filterJahr;
		List<int> _versuchsjahre;
		//bool _doFilterTypes;
		//string _filterFlaeTyp;
		//List<string> _flaechentypen;

		RelayCommand _updateCommand;


		#endregion

		#region Construction and Init

		public VmNachrichtenzuordnung()
		{
			ViewVisual = new ViewNachrichtenzuordnung();	
			Init();
			ReadData();
		}
		
		void Init()
		{
			// Filter
			_doFilterYears = true;
			_filterJahr = DateTime.Now.Year;
			_versuchsjahre = new List<int>(20);
			for (int n = 2010; n <= 2030; n++)
				_versuchsjahre.Add(n);

			//_doFilterTypes = false;
			//_flaechentypen = Flaeche.GetFlaechentypenMz();
			//_filterFlaeTyp = _flaechentypen[0];
			
			// commands
			_updateCommand = new RelayCommand(param => this.UpdateNachrichtenzuordnung(), param => this.CanUpdate);

		}

		#endregion

		#region DataIO + Eventverarbeitung
		
		void ReadData()
		{
			_personId = GlobData.OperatorId;//Zwischenspeicher wegen evtl. Operatorwechsel

			DataTableZuordnung = SqlAccess.GetNachrichtenzuordnungTable(_personId, (_doFilterYears) ? _filterJahr : 0);//, _doFilterTypes ? _filterFlaeTyp: "");
			GlobData.HasUnsavedData = false;

			if (_dtZuordnung == null)
			{
				MsgWindow.Show("Versuchs-Daten konnten nicht gelesen werden", SqlAccess.ErrorMsg, MessageLevel.Error);
			}
			else
			{
				_dtZuordnung.Columns.Add("Jahr", typeof(string));
				foreach(DataRow dr in _dtZuordnung.Rows)
				{
					dr["Jahr"] = (Convert.ToInt32(dr["Start"])==Convert.ToInt32(dr["Ende"])) ? dr["Start"] : dr["Start"] + " - " + dr["Ende"];
				}
				_dtZuordnung.AcceptChanges();

				_dtZuordnung.RowChanged += Row_Changed;
			}
		}

		private  void Row_Changed(object sender, DataRowChangeEventArgs e)
		{
			GlobData.HasUnsavedData = true;
			
			//_dtZuordnung.RowChanged -= Row_Changed;//damit sich die Funktion nicht immer wieder selbst aufruft 
			

			//// E-Mail Benachrichtigung aktiviert immer auch das Nachrichtensystem
			//if (Convert.ToBoolean(e.Row["PerMail"]) == true)
			//{ 
			//	e.Row["doNotify"] = true;
			//}


			//_dtZuordnung.RowChanged += Row_Changed;
		}

		#endregion
		
		#region Command 

		public ICommand UpdateCommand
		{
			get{return _updateCommand;}
		}

		bool CanUpdate
		{
			get {	return (_dtZuordnung != null);}
		}

		void UpdateNachrichtenzuordnung()
		{
			if(!SqlAccess.UpdateNachrichtenzuordnung(_personId, _dtZuordnung))
			{
				MsgWindow.Show("Nachrichtenzuordnung konnte nicht geschrieben werden",SqlAccess.ErrorMsg, MessageLevel.Error);
			}
			ReadData();
		}


		public void  RememberUpdate()
		{
			if (GlobData.HasUnsavedData)
			{
				if (MsgYNWindow.Show("Geänderte Einstellungen wurden noch nicht übertragen", "Möchten Sie jetzt speichern?", MessageLevel.Warning) == true)
					UpdateNachrichtenzuordnung();

				GlobData.HasUnsavedData = false;

			}

		}
		#endregion

		#region Properties

		public DataTable DataTableZuordnung
		{
			get
			{
				return _dtZuordnung;
			}
			set
			{
				_dtZuordnung = value;
				OnPropertyChanged("DataTableZuordnung");
			}
		}

		public string Header
		{
			get
			{
				return GlobData.CurrentUser.FullName;
			}
		}

		public bool DoFilterYears
		{
			get { return _doFilterYears;}
			set 
			{
				_doFilterYears = value;
				OnPropertyChanged("DoFilterYears");
				OnPropertyChanged("Header");
				ReadData();
			}
		}	
		
		public List<int> Versuchsjahre
		{
			get { return _versuchsjahre;}

		}

		public int FilterJahr
		{
			get { return _filterJahr; }
			set 
			{
				if (_filterJahr == value)
					return;

				_filterJahr = value;
				OnPropertyChanged("FilterJahr");
				OnPropertyChanged("Header");
				ReadData();
			 }

		}

		//public bool DoFilterTypes
		//{
		//	get { return _doFilterTypes; }
		//	set
		//	{
		//		_doFilterTypes = value;
		//		OnPropertyChanged("DoFilterTypes");
		//		OnPropertyChanged("Header");
		//		ReadData();
		//	}
		//}

		//public List<string> Flaechentypen
		//{
		//	get { return _flaechentypen; }

		//}

		//public string FilterFlaeTyp
		//{
		//	get { return _filterFlaeTyp; }
		//	set
		//	{
		//		if (_filterFlaeTyp == value)
		//			return;

		//		_filterFlaeTyp = value;
		//		OnPropertyChanged("FilterFlaeTyp");
		//		OnPropertyChanged("Header");
		//		ReadData();

		//	}
		//}
		#endregion

	}
}
