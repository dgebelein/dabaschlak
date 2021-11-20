using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dabaschlak
{
	class VmVersuchsarbeiten : VmBase
	{
		#region Variable

		VmAlleVersuche _vmVersuche;
		int _idVersuch;
		int _idFlaeche;
		int _selectedRowIndex;

		DataTable _dtArbeiten;
		Dictionary<int, string> _dictUsers;

		Arbeit _editedArbeit;

		Propertymode _propMode;
		
		PropertyArbeit _propArbeit;
		bool _propChanged;
		List<string> _propUsernames;
		string _propPerson;
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

		public VmVersuchsarbeiten()
		{

			Init();
			_vmVersuche = new VmAlleVersuche(this);
			ViewVisual = new ViewVersuchsarbeiten();

		}
		void Init()
		{

			//Foreign Keys Dicts für Klartexte
			_dictUsers = SqlAccess.GetUserDict();
			_propUsernames = _dictUsers.Values.ToList();
			_propUsernames.Sort();

			_propAktionstypen = Arbeit.GetAktionsypen();
		}

		#endregion

		#region DataIO

		void ReadData()
		{
			DataTableArbeiten = SqlAccess.GetArbeitenWithVersuch(_idVersuch);
			if (_dtArbeiten == null)
			{
				MsgWindow.Show("Versuchsarbeiten konnten nicht gelesen werden", SqlAccess.ErrorMsg, MessageLevel.Error);
				return;
			}
			_dtArbeiten.Columns.Add("NotizenLine");
			foreach(DataRow row in _dtArbeiten.Rows)
			{
				row["NotizenLine"] = ((string) row["Notizen"]).Replace(System.Environment.NewLine, " | ");
			}

		}

		public void SetAktuellerVersuch(int versuchsId, int flaechenId, int startJahr, int endJahr)
		{

			_idVersuch = versuchsId;
			_idFlaeche = flaechenId;
			_propLimStart = DateTime.Parse("01.01." + startJahr);
			_propLimEnd = DateTime.Parse("31.12." + endJahr);

			ReadData();
			OnPropertyChanged("DataTableArbeiten");
			OnPropertyChanged("VersuchHeader");
			SelectedRowIndex = -1;
		}

		#endregion



		#region Properties

		public object DCAuswahlListe 
		{
			get { return _vmVersuche; }
		}

		public int SelectedRowIndex
		{
			get { return _selectedRowIndex; }
			set 
			{ 
				_selectedRowIndex = value;
				OnPropertyChanged("SelectedRowIndex"); 
			}
		}


		public string VersuchHeader
		{ 
			get 
			{ 
				if	(_dtArbeiten == null)
					return "noch kein Versuch ausgewählt";

				DataRow row = _vmVersuche.DataTableVersuche.Rows[_vmVersuche.SelectedVersuch];
				return Convert.ToString(row["VerBez"])+" / "+Convert.ToString(row["FlaeBez"]);
				
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
			get { return _editedArbeit.Aktion; }
			set 
			{
				if(_editedArbeit.Aktion != value)
				{
					_editedArbeit.Aktion = value;
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
			
			if ((PropDatum < _propLimStart) ||(PropDatum > _propLimEnd))
				AddErrorMessage( "PropDatum", "Termin muss in den Versuchszeitraum fallen.");
			if (PropDatum > DateTime.Now)
				AddErrorMessage( "PropDatum", "Termin kann nicht in der Zukunft liegen.");

			if (String.IsNullOrWhiteSpace(PropPerson))
				AddErrorMessage( "PropPerson", "hier wird der Name eines Instituts-Mitarbeiters gebraucht."); 

			if (String.IsNullOrWhiteSpace(PropAktion))
				AddErrorMessage( "PropAktion", "wählen Sie hier eine Aktion aus.");
				
			switch(PropAktion)	
			{
				case "Anzucht":
							if (String.IsNullOrWhiteSpace(PropNotizen))
								AddErrorMessage( "PropNotizen", "Präzisieren Sie hier die Tätigkeit 'Anzucht': \nWas? Wo?"); break;
				case "Beregnung": break;
				case "Bodenbearbeitung":
							if (String.IsNullOrWhiteSpace(PropNotizen))
								AddErrorMessage( "PropNotizen", "Präzisieren Sie hier die Tätigkeit 'Bodenbearbeitung': \nWas genau? "); break;

				case "Düngung":
							if (String.IsNullOrWhiteSpace(PropNotizen))
								AddErrorMessage( "PropNotizen", "Präzisieren Sie hier die Tätigkeit 'Düngung': \nWas? Wieviel?"); break;
				case "Drillsaat": break;
				case "Ernte": break;
				case "Hacken": break;
				case "Herbizidbehandlung":
							if (String.IsNullOrWhiteSpace(PropNotizen))
								AddErrorMessage( "PropNotizen", "Präzisieren Sie hier die Tätigkeit 'Herbizidbehandlung': \nWas? Wieviel?"); break;
				case "Pflanzung": break;
				case "Pflanzenschutz":	
							if (String.IsNullOrWhiteSpace(PropNotizen))
								AddErrorMessage( "PropNotizen", "Präzisieren Sie hier die Tätigkeit 'Pflanzenschutz': \nWas? Wieviel? Wogegen?"); break;
								
				case "Pflegemaßnahmen":
							if (String.IsNullOrWhiteSpace(PropNotizen))
								AddErrorMessage( "PropNotizen", "Präzisieren Sie hier die Tätigkeit 'Pflegemaßnahmen': \nWelche Maßnahmen?"); break;
				case "Sonstiges":
							if (String.IsNullOrWhiteSpace(PropNotizen))
								AddErrorMessage( "Sonstiges", "Präzisieren Sie hier die Tätigkeit 'Sonstiges': \nWelche Aktion genau?"); break;

				default: break;
			
			}
			OnPropertyChanged("PropDatum");
			OnPropertyChanged("PropPerson");
			OnPropertyChanged("PropAktion");
			OnPropertyChanged("PropNotizen");



		}

		#endregion

		#region definition commands



		public ICommand AddCommand
		{
			get
			{
				if (_addCommand == null)
				{
					_addCommand = new RelayCommand(param => this.AddRow(), param => this.CanAdd);
				}
				return _addCommand;
			}
		}

		bool CanAdd
		{
			get { return ((GlobData.OperatorId >= 0) && (_dtArbeiten != null));  } // jemand angemeldet und Versuch ausgewählt?
		}

		public ICommand PropertiesCommand
		{
			get
			{
				if (_removeCommand == null)
				{
					_propertiesCommand = new RelayCommand(param => this.ShowProperties());
				}
				return _propertiesCommand;
			}
		}

		public ICommand RemoveCommand
		{
			get
			{
				if (_removeCommand == null)
				{
					_removeCommand = new RelayCommand(param => this.RemoveRow(), param => this.CanRemove);
				}
				return _removeCommand;
			}
		}

		bool CanRemove
		{
			get
			{ // Admin und Akteur dürfen löschen
				if (GlobData.IsAdminMode)
					return true;
				else
				{
					if (SelectedRowIndex < 0)
						return false;
					DataRow row = _dtArbeiten.Rows[SelectedRowIndex];
					return (GlobData.OperatorId == Convert.ToInt32(row["Person"]));
				}
			}
				
		}

		public ICommand PropUpdateCommand
		{
			get
			{
				if (_propUpdateCommand == null)
				{
					_propUpdateCommand = new RelayCommand(param => this.PropUpdate(), param => this.CanPropUpdate);
				}
				return _propUpdateCommand;
			}
		}

		bool CanPropUpdate
		{
			get { return _propChanged && CanApply;  }   

		}

		public ICommand PropCloseCommand
		{
			get
			{
				if (_propCloseCommand == null)
				{
					_propCloseCommand = new RelayCommand(param => this.PropClose());
				}
				return _propCloseCommand;
			}
		}


		#endregion
	
		#region command-execution

		void AddRow()
		{
			_propMode = Propertymode.Add;
			_propChanged = false;
	
			_editedArbeit = new Arbeit();
			_editedArbeit.Datum = DateTime.Now;
			_propPerson = (GlobData.CurrentUser != null) ? GlobData.CurrentUser.FullName : "";
			//_propLimStart= _vmVersuche
			
			Validate();

			_propVerBez = "Arbeit für: " + VersuchHeader;
			_propArbeit = new PropertyArbeit(this);
			_propArbeit.ShowDialog();
		}


		void ShowProperties()
		{
			_propMode = Propertymode.Edit;
			_propChanged = false;
			
			DataRow row = _dtArbeiten.Rows[SelectedRowIndex];

			_editedArbeit = new Arbeit();
			_editedArbeit.ArbeitId = Convert.ToInt32(row["ArbeitId"]);	
			_editedArbeit.Datum = Convert.ToDateTime(row["Datum"]);			
			_editedArbeit.Aktion = Convert.ToString(row["Aktion"]);
			_editedArbeit.Notizen = Convert.ToString(row["Notizen"]);
			_propPerson = Convert.ToString(row["PersonName"]); 
			//_propLimStart = Convert.ToInt32(row["Start"]);
			//_propLimEnd = Convert.ToInt32(row["Ende"]);
			_propVerBez = "Arbeit für: " + VersuchHeader;

			Validate();

			_propArbeit = new PropertyArbeit(this);
			_propArbeit.ShowDialog();


		}

		void PropUpdate()
		{
			_propArbeit.Close();
			if (!_propChanged)
				return;

			_editedArbeit.VersuchId = _idVersuch;
			_editedArbeit.FlaecheId = _idFlaeche;


			////Klartextangaben in ForeignKeys umrechnen
			_editedArbeit.PersonId = _dictUsers.FirstOrDefault(x => x.Value == _propPerson).Key;

			bool success = (_propMode == Propertymode.Add) ?
				SqlAccess.InsertNewArbeit(_editedArbeit) :
				SqlAccess.UpdateArbeit(_editedArbeit);
			
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
			DataRow row = _dtArbeiten.Rows[SelectedRowIndex];
			int id = Convert.ToInt32(row["ArbeitId"]);

			if(!SqlAccess.DeleteArbeit(id))
				MsgWindow.Show("Eintrag konnte nicht gelöscht werden.", SqlAccess.ErrorMsg, MessageLevel.Error);
			
			ReadData();
		}

		void UpdateDb()
		{
			//if (_propMode == Propertymode.Add)

			//if (!SqlAccess.InUpdateVersuche(_dtVersuche))
			//{
			//	MsgWindow.Show("Flächen-Daten können nicht aktualisiert werden.", SqlAccess.ErrorMsg, MessageLevel.Error);
			//	return;
			//}

			//ReadData(); // Refresh Anzeige
		}



		#endregion		
		}
}
