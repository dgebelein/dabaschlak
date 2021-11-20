using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TTP.UiUtils;

namespace dabaschlak
{
	class VmAllePersonen:VmBase
	{
		DataTable _dtPersonen = null;

		int _selectedRowIndex;
		DataRowView _selectedRow;
		Propertymode _propMode;
		bool _isRowChanged;
		Person _editedPerson;
		
		PropertyPerson _propPerson;

		RelayCommand _removeCommand;
		RelayCommand _addCommand;
		RelayCommand _propertiesCommand;
		RelayCommand _propCloseCommand;
		RelayCommand _propUpdateCommand;

		#region construction + init
		
		public VmAllePersonen()
		{	
			ViewVisual = new ViewAllePersonen();
			ReadData();
			Init();
		}

		void Init()
		{
			_addCommand = new RelayCommand(param => this.AddRow(), param => this.CanAdd);
			_removeCommand = new RelayCommand(param => this.RemoveRow(), param => this.CanRemove);
			_propertiesCommand = new RelayCommand(param => this.ShowProperties());
			_propUpdateCommand = new RelayCommand(param => this.PropUpdate(), param => this.CanPropUpdate);
			_propCloseCommand = new RelayCommand(param => this.PropClose());
		}
		#endregion
		
		#region ReadData
		
		void ReadData()
		{
			DataTablePersonen = SqlAccess.GetPersonsTable();
			
			if (_dtPersonen == null)
			{
				MsgWindow.Show("Mitarbeiter-Daten konnten nicht gelesen werden",SqlAccess.ErrorMsg, MessageLevel.Error);
			}
		}

		#endregion

		#region Properties


		public DataTable DataTablePersonen
		{
			get 
			{
				return _dtPersonen;
			}
			set 
			{
				_dtPersonen = value;
				OnPropertyChanged("DataTablePersonen");
			}
		}

		public DataRowView SelectedRow
		{ 
			get { return _selectedRow; }
			set { _selectedRow = value; }
		}

		public int SelectedRowIndex
		{
			get { return _selectedRowIndex; }
			set { _selectedRowIndex = value; }
		}
		
		public string PropHeader
		{
			get { return (_propMode == Propertymode.Add) ? "neuer Mitarbeiter" : "Mitarbeiter-Daten"; }

		}

		public string PropName
		{
			get { return _editedPerson.Name; }
			set
			{
				if (value == _editedPerson.Name)
					return;

				_isRowChanged = true;
				
				_editedPerson.Name = value.Trim();;
				Validate();

			}
		}

		public string PropVorname
		{
			get { return _editedPerson.Vorname; }
			set
			{
				if (value == _editedPerson.Vorname)
					return;

				_isRowChanged = true;
				_editedPerson.Vorname = value;
				Validate();
			}
		}

		public string PropNetzname
		{
			get { return _editedPerson.Netzname; }
			set
			{
				if (value == _editedPerson.Netzname)
					return;

				_isRowChanged = true;
				_editedPerson.Netzname = value;
				Validate();
			}
		}

		public string PropTel
		{
			get { return _editedPerson.Tel; }
			set
			{
				if (value == _editedPerson.Tel)
					return;

				_isRowChanged = true;
				_editedPerson.Tel = value;
				Validate();
			}
		}

		public string PropEmail
		{
			get { return _editedPerson.Email; }
			set
			{
				if (value == _editedPerson.Email)
					return;

				_isRowChanged = true;
				_editedPerson.Email = value.Trim();
				Validate();
			}
		}
		
		public bool PropAktiv
		{
			get { return _editedPerson.Aktiv; }
			set
			{
				if (value == _editedPerson.Aktiv)
					return;

				_isRowChanged = true;
				_editedPerson.Aktiv = value;
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
				lines.Add("Die Mitarbeiter-Stammdaten können nur mit");
				lines.Add("Administrationsrechten geändert werden. ");

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

		public ICommand RemoveCommand
		{
			get{return _removeCommand;}
		}

		bool CanRemove
		{
			get { return GlobData.IsAdminMode; }   // nur für Admin

		}

		public ICommand PropertiesCommand
		{
			get{return _propertiesCommand;}
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
			_editedPerson = new Person();

			_propPerson = new PropertyPerson(this);
			_propPerson.ShowDialog();
		}

		void ShowProperties()
		{
			_propMode = Propertymode.Edit;
			_isRowChanged = false;

			_editedPerson = new Person(_selectedRow.Row);

			_propPerson = new PropertyPerson(this);
			Validate();
			
			_propPerson.ShowDialog();

		}

		void PropUpdate()
		{
			if(!SqlAccess.IsValidPerson(_editedPerson))								
			{
				MsgWindow.Show("Mitarbeiterdaten können nicht übernommen werden.", "Es existiert bereits eine Person mit diesem Namen", MessageLevel.Error);
				return;
			}
			_propPerson.Close();
			if (!_isRowChanged)
				return;

			//todo:zusammenfassen

			if (_propMode == Propertymode.Edit)
			{
				DataRow row = _selectedRow.Row;
				row["Aktiv"] = _editedPerson.Aktiv;
				row["Name"] = _editedPerson.Name;
				row["Vorname"] = _editedPerson.Vorname;
				row["Netzname"] = _editedPerson.Netzname;
				row["Tel"] = _editedPerson.Tel;
				row["Email"] = _editedPerson.Email;

			}
			else
			{
				var row = _dtPersonen.NewRow();
				row["Aktiv"] = _editedPerson.Aktiv;
				row["Name"] = _editedPerson.Name;
				row["Vorname"] = _editedPerson.Vorname;
				row["Netzname"] = _editedPerson.Netzname;
				row["Tel"] = _editedPerson.Tel;
				row["Email"] = _editedPerson.Email;
				_dtPersonen.Rows.Add(row);
			}

			SqlAccess.UpdatePersons(_dtPersonen);
			ReadData();
		}

		void PropClose()
		{
			_propPerson.Close();
		}

		void RemoveRow()
		{
			DataRow row = _selectedRow.Row;
			int personIndex = Convert.ToInt32(row["PersonId"]);

			//int rowId = SelectedRowIndex;
			//int personIndex = Convert.ToInt32(_dtPersonen.Rows[rowId]["PersonId"]);
			
			if (!SqlAccess.CanDeletePerson(personIndex))
			{
				MsgWindow.Show("Datensatz kann nicht gelöscht werden.", SqlAccess.ErrorMsg, MessageLevel.Error);
				return;
			}

			SqlAccess.DeletePerson(personIndex);
			ReadData();
		}



		#endregion

		#region Validierung
		public void Validate()
		{
			ResetErrorList();

			if (String.IsNullOrWhiteSpace(PropName))
				AddErrorMessage( "PropName", "Person muss einen Name haben."); 

			if (String.IsNullOrWhiteSpace(PropVorname))
				AddErrorMessage( "PropVorname", "Person muss einen Vornamen haben.");

			if (String.IsNullOrWhiteSpace(PropNetzname))
				AddErrorMessage("PropNetzname", "Person muss einen Usernamen fürs Netzwerk haben.");

			if ( !String.IsNullOrWhiteSpace(PropEmail) && !IsValidEmailAddr(PropEmail))
				AddErrorMessage( "PropEmail", "Dies ist keine zulässige Email-Adresse");
			
			OnPropertyChanged("PropName"); 
			OnPropertyChanged("PropVorname");
			OnPropertyChanged("PropNetzname");
			OnPropertyChanged("PropTel"); 
			OnPropertyChanged("PropEmail"); 
			OnPropertyChanged("PropAktiv"); 

		}

		bool IsValidEmailAddr(string addr)
		{
			try
			{
				var a = new MailAddress(addr);
			}
			catch 
			{
				return false;
			}
			return true;
		}

		#endregion

	}
}
