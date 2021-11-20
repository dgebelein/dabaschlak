using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dabaschlak
{
	public enum ArbeitenTyp
	{ 
		Flaechenarbeit,
		Versuchsarbeit
	}

	class VmArbeiten : VmBase
	{
		#region Variable

		ArbeitenTyp _typArbeiten;
		VmAlleVersuche _vmVersuche;
		VmAlleFlaechen _vmFlaechen;

		int _idVersuch;
		int _idFlaeche;
		DataRowView _selectedRow;

		DataTable _dtArbeiten;
		Dictionary<int, string> _dictUsers;

		Aktion _editedArbeit;

		Propertymode _propMode;
		
		PropertyAktion _propArbeit;
		bool _propChanged;
		List<string> _propUsernames;
		string _propPerson;
		List<Kategorie> _kategorien;
		List<string> _propAktionstypen;
	
		DateTime _propLimStart;
		DateTime _propLimEnd;
		string _propVerBez;
		

		RelayCommand _removeCommand;
		RelayCommand _addCommand;
		RelayCommand _propertiesCommand;
		RelayCommand _propCloseCommand;
		RelayCommand _propUpdateCommand;

		#endregion

		#region Construction and Init

		public VmArbeiten(ArbeitenTyp aTyp)
		{
			_typArbeiten = aTyp;

			if(_typArbeiten == ArbeitenTyp.Versuchsarbeit)
			{ 
				_vmVersuche = new VmAlleVersuche(this);
				_vmVersuche.SelectionChanged += VersuchChanged;
				ViewVisual = new ViewVersuchsarbeiten();
			}
			else
			{ 
				_vmFlaechen = new VmAlleFlaechen(this);
				_vmFlaechen.SelectionChanged += FlaecheChanged;
				ViewVisual = new ViewFlaechenarbeiten();
			}

			Init();

		}

		void Init()
		{
			//Foreign Keys Dicts für Klartexte
			_dictUsers = SqlAccess.GetUserDict();
			_propUsernames = _dictUsers.Values.ToList();
			_propUsernames.Sort();

			Arbeitskategorien ak = new Arbeitskategorien();
			_propAktionstypen = ak.Aktionen;
			_kategorien = ak.Items;
			
			// Commands
			_addCommand = new RelayCommand(param => this.AddRow(), param => this.CanAdd);
			_removeCommand = new RelayCommand(param => this.RemoveRow(), param => this.CanRemove);
			_propertiesCommand = new RelayCommand(param => this.ShowProperties());
			_propUpdateCommand = new RelayCommand(param => this.PropUpdate(), param => this.CanPropUpdate);
			_propCloseCommand = new RelayCommand(param => this.PropClose());

		}

		#endregion

		#region DataIO + Auswahl

		void ReadData()
		{
			DataTableArbeiten = (_typArbeiten == ArbeitenTyp.Versuchsarbeit) ?
				SqlAccess.GetArbeitenWithVersuch(_idVersuch) :
				SqlAccess.GetArbeitenWithFlaeche(_idFlaeche);

			if (_dtArbeiten == null)
			{
				MsgWindow.Show("Aktionen konnten nicht gelesen werden", SqlAccess.ErrorMsg, MessageLevel.Error);
				return;
			}
			_dtArbeiten.Columns.Add("NotizenLine");
			foreach (DataRow row in _dtArbeiten.Rows)
			{
				if (row["Notizen"] is System.DBNull)
					continue;
				 
				string s = (string)row["Notizen"];
				row["NotizenLine"] = s.Replace(System.Environment.NewLine, " | ");
				
			}

		}

		private void VersuchChanged(object source, VersuchArgs e)
		{
			_idVersuch = e.IdVersuch;
			_idFlaeche = 0;
			_propLimStart = DateTime.Parse("01.01." + e.Start);
			_propLimEnd = DateTime.Parse("31.12." + e.Ende);

			ReadData();
			OnPropertyChanged("DataTableArbeiten");
			OnPropertyChanged("ArbeitenSubHeader");
			SelectedRow = null;		
		}

		private void FlaecheChanged(object Source, FlaecheArgs e)
		{
			_idFlaeche = e.IdFlaeche;

			ReadData();
			OnPropertyChanged("DataTableArbeiten");
			OnPropertyChanged("ArbeitenSubHeader");
			SelectedRow = null;	
		}

		#endregion

		#region Properties

		public object DCAuswahlListe 
		{
			get { return  (_typArbeiten == ArbeitenTyp.Versuchsarbeit)?  (object)_vmVersuche: (object)_vmFlaechen; }
		}

		public DataRowView SelectedRow
		{
			get { return _selectedRow; }
			set 
			{ 
				_selectedRow = value;
				OnPropertyChanged("SelectedRow"); 
			}
		}


		public string ArbeitenSubHeader
		{ 
			get 
			{ 
				if(_typArbeiten== ArbeitenTyp.Versuchsarbeit) 
				{ 
					if	(_dtArbeiten == null)
						return "noch kein Versuch ausgewählt";

					//DataRow row = _vmVersuche.DataTableVersuche.Rows[_vmVersuche.SelectedVersuch];
					DataRow row = _vmVersuche.SelectedVersuch.Row;
					return Convert.ToString(row["VerBez"]) + " / " + Convert.ToString(row["Standorte"]);
				}
				else {
					if	(_dtArbeiten == null)
						return "noch keine Fläche ausgewählt";

					//DataRow row = _vmFlaechen.DataTableFlaechen.Rows[_vmFlaechen.SelectedFlaeche];
					DataRow row = _vmFlaechen.SelectedFlaeche.Row;

					return Convert.ToString(row["FlaeBez"]);
				}
			} 
		}

		public string ArbeitenHeader
		{ 
			get 
			{
				return (_typArbeiten == ArbeitenTyp.Versuchsarbeit) ?
				 "Versuchsarbeiten für:" :
				 "Aktionen an Fläche:";
			} 
		}

		public DataTable DataTableArbeiten
		{
			get { return _dtArbeiten; }
			set 
			{
				_dtArbeiten = value;
				OnPropertyChanged("DataTableArbeiten");
			}

		}

		public string PropHeader
		{
			get { return _propVerBez; }
		}

		public DateTime PropDatum
		{
			get { return _editedArbeit.Datum; }
			set 
			{
				if(_editedArbeit.Datum!= value)
				{ 
					_editedArbeit.Datum = value;
					_propChanged = true;
					OnPropertyChanged("PropDatum");
					Validate();
				}
			}


		}

		public List<string> PropAktionen
		{
			get { return _propAktionstypen; }
		}

		public string PropAktion
		{
			get { return _editedArbeit.Typ; }
			set 
			{
				if(_editedArbeit.Typ != value)
				{
					_editedArbeit.Typ = value;
					_propChanged = true;
					Validate();
				}
			}
		}

		
		public List<string> PropUsernames
		{
			get { return _propUsernames; }
		}

		public string PropPerson
		{
			get { return _propPerson; }
			set 
			{
				if(_propPerson != value)
				{
					_propPerson = value;
					_propChanged = true;
					Validate();
				}
			}
		}

		public string PropNotizen
		{
			get { return _editedArbeit.Notizen;}
			set 
			{ 
				if(_editedArbeit.Notizen!= value)
				{
					_editedArbeit.Notizen = value;
					_propChanged = true;
					Validate();
				}
			}
		}


		
		#endregion

		#region Validierung
		void Validate()
		{
			ResetErrorList();
			
			if((_typArbeiten == ArbeitenTyp.Versuchsarbeit) && ((PropDatum < _propLimStart) ||(PropDatum > _propLimEnd)))
				AddErrorMessage( "PropDatum", "Termin muss in den Versuchszeitraum fallen.");
			
			if (PropDatum > DateTime.Now)
				AddErrorMessage( "PropDatum", "Termin kann nicht in der Zukunft liegen.");

			if (String.IsNullOrWhiteSpace(PropPerson))
				AddErrorMessage( "PropPerson", "hier wird der Name eines Instituts-Mitarbeiters gebraucht."); 

			if (String.IsNullOrWhiteSpace(PropAktion))
				AddErrorMessage( "PropAktion", "wählen Sie hier eine Aktion aus.");
			
			if(!string.IsNullOrEmpty(PropAktion))	
			{
				int id = _kategorien.FindIndex(item => item.Arbeit == PropAktion);
				if (id >=0)
				{ 
					if(_kategorien[id].hatNotizen && String.IsNullOrWhiteSpace(PropNotizen))	
						AddErrorMessage( "PropNotizen",_kategorien[id].Fehlertext);			
				}

			}

			OnPropertyChanged("PropDatum");
			OnPropertyChanged("PropPerson");
			OnPropertyChanged("PropAktion");
			OnPropertyChanged("PropNotizen");



		}

		#endregion

		#region  commands

		public ICommand AddCommand
		{
			get{return _addCommand;}
		}

		bool CanAdd
		{
			get { return ((GlobData.OperatorId >= 0) && (_dtArbeiten != null));  } // jemand angemeldet und Versuch ausgewählt?
		}

		public ICommand PropertiesCommand
		{
			get{return _propertiesCommand;}
		}

		public ICommand RemoveCommand
		{
			get{return _removeCommand;}
		}

		bool CanRemove
		{
			get
			{ // Admin und Akteur dürfen löschen
				if (GlobData.IsAdminMode)
					return true;
				else
				{
					if (SelectedRow == null)
						return false;
					DataRow row = SelectedRow.Row;
					return (GlobData.OperatorId == Convert.ToInt32(row["Person"]));
				}
			}
				
		}

		public ICommand PropUpdateCommand
		{
			get{return _propUpdateCommand;}
		}

		bool CanPropUpdate
		{
			get { return _propChanged && CanApply;  }   
		}

		public ICommand PropCloseCommand
		{
			get{return _propCloseCommand;}
		}


		#endregion
	
		#region command-execution

		void AddRow()
		{
			_propMode = Propertymode.Add;
			_propChanged = false;
	
			_editedArbeit = new Aktion();
			_editedArbeit.Datum = DateTime.Now.Date;
			_propPerson = (GlobData.CurrentUser != null) ? GlobData.CurrentUser.FullName : "";
			
			Validate();

			_propVerBez = "Arbeit für: " + ArbeitenSubHeader;
			_propArbeit = new PropertyAktion(this);
			_propArbeit.ShowDialog();
		}


		void ShowProperties()
		{
			_propMode = Propertymode.Edit;
			_propChanged = false;
			
			DataRow row = SelectedRow.Row;

			_editedArbeit = new Aktion();
			_editedArbeit.AktionId = Convert.ToInt32(row["ArbeitId"]);	
			_editedArbeit.Datum = Convert.ToDateTime(row["Datum"]);			
			_editedArbeit.Typ = Convert.ToString(row["Aktion"]);
			_editedArbeit.Notizen = Convert.ToString(row["Notizen"]);
			_propPerson = Convert.ToString(row["PersonName"]); 
			//_propLimStart = Convert.ToInt32(row["Start"]);
			//_propLimEnd = Convert.ToInt32(row["Ende"]);
			_propVerBez = "Arbeit für: " + ArbeitenSubHeader;

			Validate();

			_propArbeit = new PropertyAktion(this);
			_propArbeit.ShowDialog();


		}

		void PropUpdate()
		{
			_propArbeit.Close();
			if (!_propChanged)
				return;

			_editedArbeit.VersuchId = _idVersuch;
			_editedArbeit.FlaecheId =  _idFlaeche; ;


			////Klartextangaben in ForeignKeys umrechnen
			_editedArbeit.PersonId = _dictUsers.FirstOrDefault(x => x.Value == _propPerson).Key;

			bool success;
			if (_propMode == Propertymode.Add)
			{
				success = SqlAccess.InsertNewArbeit(_editedArbeit);
				if (success)
				{
					GlobData.AddedActions.Add(_editedArbeit);
				}
				
			}
			else
			{
				success = SqlAccess.UpdateArbeit(_editedArbeit);
			}

			
			if (!success)
				MsgWindow.Show("Arbeitsdaten können nicht übernommen werden.", SqlAccess.ErrorMsg, MessageLevel.Error);


			ReadData();
		}

		void PropClose()
		{
			_propArbeit.Close();
		}

		void RemoveRow()
		{
			DataRow row = SelectedRow.Row;
			int id = Convert.ToInt32(row["ArbeitId"]);

			if(!SqlAccess.DeleteArbeit(id))
				MsgWindow.Show("Eintrag konnte nicht gelöscht werden.", SqlAccess.ErrorMsg, MessageLevel.Error);
			
			ReadData();
		}

		#endregion		
		}
}
