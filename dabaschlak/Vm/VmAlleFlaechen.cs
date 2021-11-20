using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TTP.UiUtils;

namespace dabaschlak
{	
	enum Propertymode
	{
		Edit,
		Add
	};

	class VmAlleFlaechen : VmBase
	{
		object _parent;
		DataTable _dtFlaechen;
		DataRowView _selectedRow;	// für ViewAlle Flächen
		DataRowView _selectedFlaeche;	// für Auswahl bei Flächenarbeiten

		bool _doFilterTypes;
		string _filterFlaeTyp;

		Propertymode _propMode;
		bool _isRowChanged;
		Flaeche _editedFlaeche;
		List<string> _flaechentypen;
		PropertyFlaeche _propFlaeche;


		RelayCommand _removeCommand;
		RelayCommand _addCommand;
		RelayCommand _propertiesCommand;
		RelayCommand _propCloseCommand;
		RelayCommand _propUpdateCommand;

		public delegate void SelectionChangedHandler(object sender, FlaecheArgs e);
		public event SelectionChangedHandler SelectionChanged;


		#region construction and init

		public VmAlleFlaechen()
		{	
			ViewVisual = new ViewAlleFlaechen();
			Init();
			ReadData();
		}

		public VmAlleFlaechen(object parent)
		{
			_parent = parent;
			_selectedFlaeche = null;
			Init();
			ReadData();
		}

		void Init()
		{
			_addCommand = new RelayCommand(param => this.AddRow(), param => this.CanAdd);
			_removeCommand = new RelayCommand(param => this.RemoveRow(), param => this.CanRemove);
			_propertiesCommand = new RelayCommand(param => this.ShowProperties());
			_propUpdateCommand = new RelayCommand(param => this.PropUpdate(), param => this.CanPropUpdate);
			_propCloseCommand = new RelayCommand(param => this.PropClose());

			_doFilterTypes = false;
			_flaechentypen = Flaeche.GetFlaechentypen();
			_filterFlaeTyp = _flaechentypen[0];
		}
		#endregion

		#region ReadData + Eventbehandlung
		
		void ReadData()
		{
			DataTableFlaechen = SqlAccess.GetFlaechenTable( _doFilterTypes ? _filterFlaeTyp: "");

			if (_dtFlaechen == null)
			{
				MsgWindow.Show("Versuchsflächen-Daten konnten nicht gelesen werden", SqlAccess.ErrorMsg, MessageLevel.Error);
				return;
			}

			_dtFlaechen.Columns.Add("FlaeEigZeile");
			foreach (DataRow row in _dtFlaechen.Rows)
			{
				if (row["FlaeEig"] is System.DBNull)
					continue;
				 
				string s = (string)row["FlaeEig"];
				row["FlaeEigZeile"] = s.Replace(System.Environment.NewLine, " | ");
			}
		}

		protected void OnSelectionChanged(int flaechenId)
		{
			if (SelectionChanged != null)
				SelectionChanged(this, new FlaecheArgs {IdFlaeche=flaechenId});
		}

		#endregion

		#region properties

		public DataTable DataTableFlaechen
		{
			get
			{
				return _dtFlaechen;
			}
			set
			{
				_dtFlaechen = value;
				OnPropertyChanged("DataTableFlaechen");
			}
		}
		
		public bool DoFilterTypes
		{
			get { return _doFilterTypes;}
			set 
			{
				_doFilterTypes = value;
				OnPropertyChanged("DoFilterTypes");
				OnPropertyChanged("Header");
				ReadData();
			}
		}	
		
		public List<string> Flaechentypen
		{
			get { return _flaechentypen;}

		}

		public string FilterFlaeTyp
		{
			get { return _filterFlaeTyp; }
			set 
			{
				if (_filterFlaeTyp == value)
					return;

				_filterFlaeTyp = value;
				OnPropertyChanged("FilterFlaeTyp");
				OnPropertyChanged("Header");
				ReadData();
			 }

		}

		public DataRowView SelectedFlaeche
		{
			get { return _selectedFlaeche; }
			set 
			{
				if(_selectedFlaeche!=value && value != null)
				{ 
					_selectedFlaeche = value;
					DataRow row = _selectedFlaeche.Row;
					int idFlaeche = Convert.ToInt32(row["FlaeId"]);

					OnSelectionChanged(idFlaeche);
					//((VmArbeiten)_parent).SetAktuelleFlaeche(idFlaeche);
				}
				OnPropertyChanged("SelectedFlaeche"); 
				
			}
		}

		public DataRowView SelectedRow
		{
			get { return _selectedRow; }
			set { _selectedRow = value; }
		}
		
		public string PropHeader
		{
			get { return (_propMode == Propertymode.Add) ? "neue Versuchsfläche" : "Eigenschaften Versuchsfläche"; }

		}

		public string PropBezeichnung
		{
			get { return _editedFlaeche.FlaeBez; }
			set
			{
				if (value == _editedFlaeche.FlaeBez)
					return;

				_isRowChanged = true;
				_editedFlaeche.FlaeBez = value;
				Validate();

			}
		}

		public string PropEigenschaften
		{
			get { return _editedFlaeche.FlaeEig; }
			set
			{
				if (value == _editedFlaeche.FlaeEig)
					return;

				_isRowChanged = true;
				_editedFlaeche.FlaeEig = value;
				OnPropertyChanged("PropEigenschaften");
			}
		}

		public List<string>PropFlaechentypen
		{
			get { return _flaechentypen; }

		}

		public string PropFlaechentyp
		{
			get { return _editedFlaeche.FlaeTyp; }
			set 
			{ 
				if (value == _editedFlaeche.FlaeTyp)
					return;

				_isRowChanged = true;
				_editedFlaeche.FlaeTyp = value;
				Validate();
				
			}

		}

		public bool IsPropReadOnly
		{
			get { return !GlobData.IsAdminMode; }
		}

		public Visibility VisReadonly
		{
			get { return (IsPropReadOnly) ? Visibility.Visible : Visibility.Hidden; }
		}
		
		public ToolTip ReadonlyTooltip
		{
			get
			{
				List<String> lines = new List<string>();
				lines.Add("_tt__sb__sa_Schreibgeschützte Daten");
				lines.Add("Die Versuchsflächen-Stammdaten können nur ");
				lines.Add("mit Administrationsrechten geändert werden. ");
				
				return ItemTooltip.CreateFromText(lines, 270, "#90EE90");

			}
		}
		#endregion

		#region commands


		public ICommand AddCommand
		{
			get{return _addCommand;}
		}

		bool CanAdd
		{
			get { return GlobData.IsAdminMode; ; }
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
			get { return GlobData.IsAdminMode; }   // nur für Admin

		}

		public ICommand PropUpdateCommand
		{
			get{return _propUpdateCommand;}
		}

		bool CanPropUpdate
		{
			get { return _isRowChanged && CanApply; }   // nur für Admin

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
			_isRowChanged = false;
			_editedFlaeche = new Flaeche();

			_propFlaeche = new PropertyFlaeche(this);
			_propFlaeche.ShowDialog();
		}

		void ShowProperties()
		{
			_propMode = Propertymode.Edit;
			_isRowChanged = false;
			_editedFlaeche = new Flaeche(_selectedRow.Row);


			_propFlaeche = new PropertyFlaeche(this);
			Validate();
			
			_propFlaeche.ShowDialog();

		}

		void PropUpdate()
		{
			if(SqlAccess.ExistsFlaeche(_editedFlaeche))								
			{
				MsgWindow.Show("Standortdaten können nicht übernommen werden.", "Es existiert bereits ein Standort mit dieser Bezeichnung", MessageLevel.Error);
				return;
			}
			_propFlaeche.Close();
			if (!_isRowChanged)
				return;

			if (_propMode == Propertymode.Edit)
			{	
				DataRow row = _selectedRow.Row;
				row["FlaeBez"] = _editedFlaeche.FlaeBez;
				row["FlaeTyp"] = _editedFlaeche.FlaeTyp;
				row["FlaeEig"] = _editedFlaeche.FlaeEig;
			}
			else
			{
				var row = _dtFlaechen.NewRow();
				row["FlaeBez"] = _editedFlaeche.FlaeBez;
				row["FlaeTyp"] = _editedFlaeche.FlaeTyp;
				row["FlaeEig"] = _editedFlaeche.FlaeEig;
				_dtFlaechen.Rows.Add(row);
			}

			UpdateDb();
		}

		void PropClose()
		{
			_propFlaeche.Close();
		}

		void RemoveRow()
		{
			DataRow row = _selectedRow.Row;

			int flaechenIndex = Convert.ToInt32(row["FlaeId"]);
			
			if (!SqlAccess.CanDeleteFlaeche(flaechenIndex))
			{
				MsgWindow.Show("Datensatz kann nicht gelöscht werden.", SqlAccess.ErrorMsg, MessageLevel.Error);
				return;
			}

			SqlAccess.DeleteFlaeche(flaechenIndex);
			UpdateDb();
		}

		void UpdateDb()
		{
			if(!SqlAccess.UpdateFlaechen(_dtFlaechen))
			{
				MsgWindow.Show("Flächen-Daten können nicht aktualisiert werden.", SqlAccess.ErrorMsg, MessageLevel.Error);
				return;
			}

			ReadData();
		}

		#endregion

		#region Validierung

		public void Validate()
		{
			ResetErrorList();

			if (String.IsNullOrWhiteSpace(PropBezeichnung))
				AddErrorMessage( "PropBezeichnung", "Fläche muss eine Bezeichnung haben."); //  auf Einzigartigkeit prüfen
			else 
			{
				foreach(DataRow row in _dtFlaechen.Rows)
				{ 
					if ((Convert.ToString(row["FlaeBez"])== PropBezeichnung) && (Convert.ToInt32(row["FlaeId"])!= _editedFlaeche.FlaeId))
					AddErrorMessage( "PropBezeichnung", "Flächenbezeichnung wird bereits verwendet.");
				}
			}

			if (String.IsNullOrWhiteSpace(PropFlaechentyp))
				AddErrorMessage( "PropFlaechentypen", "Wählen Sie einen Versuchsflächen-Typ aus."); 

			

			OnPropertyChanged("PropBezeichnung");
			OnPropertyChanged("PropFlaechentyp");
			OnPropertyChanged("PropFlaechentypen");

		}

		#endregion
	}
}
