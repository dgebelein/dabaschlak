using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace dabaschlak
{
	class VmNeuerVersuch:VmBase
	{
		//UserControl _viewVis;
		Versuch _versuch;
		List<string> _userNames;
		string _versuchsLeiter;
		List<string> _versuchsFlaechen;
		string _versuchsFlaeche;

		RelayCommand _updateCommand;
		RelayCommand _selectFileCommand;



		public VmNeuerVersuch()
		{
			ViewVisual = new ViewNeuerVersuch();
			_versuch = new Versuch();
			_versuch.Start = _versuch.Ende = DateTime.Now.Year;
			UserNames = SqlAccess.GetUserNames();
			Versuchsleiter = (GlobData.CurrentUser != null) ? GlobData.CurrentUser.FullName : "";
			Versuchsflaechen = SqlAccess.GetFlaechenBezeichnungen();
			Validate();
		}

		#region Properties

		public string VerBez
		{
			get { return _versuch.VerBez; }
			set 
			{
				if(_versuch.VerBez != value)
				{
					_versuch.VerBez = value;
					Validate();
				}
			}
		}

		public int Start
		{
			get { return _versuch.Start; }
			set 
			{
				_versuch.Start = value;
				Validate();
			}		 
		}
		
		public int Ende
		{
			get { return _versuch.Ende; }
			set 
			{
				_versuch.Ende = value;
				Validate();
				
			}		 
		}

		public List<string> UserNames
		{
			get { return _userNames; }
			set
			{
				_userNames = value;
				OnPropertyChanged("UserNames");
			}
		}

		public string Versuchsleiter
		{
			get { return _versuchsLeiter; }
			set 
			{
				if(_versuchsLeiter != value)
				{
					_versuchsLeiter = value;
					Validate();
				}
			}
		}

		public List<string> Versuchsflaechen
		{
			get { return _versuchsFlaechen; }
			set
			{
				_versuchsFlaechen = value;
				OnPropertyChanged("Versuchsflaechen");
			}
		}

		public string Versuchsflaeche
		{
			get { return _versuchsFlaeche; }
			set 
			{
				if(_versuchsFlaeche != value)
				{
					_versuchsFlaeche = value;
					Validate();
				}
			}
		}

		public string Versuchsplan
		{
			get { return _versuch.Plan; }
			set 
			{
				if(_versuch.Plan != value)
				{
					_versuch.Plan = value;
					Validate();
				}
			}
		}

		public string Versuchsfrage
		{
			get { return _versuch.Frage; }
			set 
			{
				if(_versuch.Frage != value)
				{
					_versuch.Frage = value;
					Validate();
				}
			}
		}
		
		#endregion

		#region Validierung

		void Validate()
		{
			ResetErrorList();

			if (String.IsNullOrWhiteSpace(VerBez))
				AddErrorMessage( "VerBez", "Ein neuer Versuch muss einen Namen haben."); // hier evtl auf Einzigartigkeit prüfen


			if (String.IsNullOrWhiteSpace(Versuchsleiter))
				AddErrorMessage( "Versuchsleiter", "Wählen Sie hier den (haupt-)verantwortlichen Wissenschaftler aus.");
			else
			{
			if (!UserNames.Contains(Versuchsleiter))
					AddErrorMessage( "Versuchsleiter", "Versuchsleiter muss ein Institutsmitarbeiter sein.");

			}

			if (String.IsNullOrWhiteSpace(Versuchsflaeche))
				AddErrorMessage( "Versuchsflaeche", "Wählen Sie einen Schlag/Kabine aus.");

			if (String.IsNullOrWhiteSpace(Versuchsplan))
				AddErrorMessage( "Versuchsplan", "Geben Sie den Dateinamen des Versuchsplans ein. (Die Datei muss existieren und lesbar sein.)");
			else 
			{
				if(!File.Exists(Versuchsplan)) 
					AddErrorMessage( "Versuchsplan", "Die angegebene Datei existiert nicht.");
			}

			if (Start<2000 ||Start>2040 )
				AddErrorMessage( "Start", "Hier sind nur Jahreszahlen zwischen 2000 und 2040 zulässig");
			
			if (Ende<2000 ||Ende>2040 )
				AddErrorMessage( "Ende", "Hier sind nur Jahreszahlen zwischen 2000 und 2040 zulässig");
			
			if(Ende<Start)
			{ 
				AddErrorMessage( "Start", "Versuchsende kann nicht vor dem Versuchsbeginn sein");
				AddErrorMessage( "Ende", "Versuchsende kann nicht vor dem Versuchsbeginn sein");
			}

			if (String.IsNullOrWhiteSpace(Versuchsfrage))
				AddErrorMessage( "Versuchsfrage", "Beschreiben Sie hier in ein paar Worten, worum es geht.");

			OnPropertyChanged("VerBez");
			OnPropertyChanged("Versuchsleiter");
			OnPropertyChanged("Versuchsflaeche");
			OnPropertyChanged("Versuchsplan");
			OnPropertyChanged("Start");
			OnPropertyChanged("Ende");
			OnPropertyChanged("Versuchsfrage");

		}

		#endregion


		#region commands

		public ICommand SelectFileCommand
		{
			get
			{
				if (_selectFileCommand == null)
					_selectFileCommand = new RelayCommand(param => this.SelectFile());
						
				return _selectFileCommand;
			}
		}

		void SelectFile()
		{
			System.Windows.Forms.OpenFileDialog ofDlg = new System.Windows.Forms.OpenFileDialog();
			ofDlg.InitialDirectory =".";  
			ofDlg.Filter="(*.*)|*.*";
			ofDlg.FilterIndex = 1;
			ofDlg.Multiselect = false;

			if (ofDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{	
				Versuchsplan=ofDlg.FileName;
			}
		}

		public ICommand UpdateCommand
		{
			get
			{
				if (_updateCommand == null)
					_updateCommand = new RelayCommand(
						param => this.UpdateData(),
						param => this.CanUpdate);

				return _updateCommand;
			}
		}

		bool CanUpdate
		{
			get {
				return CanApply;
			}
		}

		void UpdateData()
		{
			// PersonId ergänzen

			Person pers = SqlAccess.FindPersonFromUsername(Versuchsleiter);
			if (pers == null)
			{
				MsgWindow.Show("Versuchsleiter muss ein Institutsmitarbeiter sein.", SqlAccess.ErrorMsg, MessageLevel.Error);
				AddErrorMessage("Versuchsleiter", "Versuchsleiter muss ein Institutsmitarbeiter sein.");
				CommandManager.InvalidateRequerySuggested();
				return;
			}


			_versuch.Leiter = 1; // pers.PersonId;
			if(!SqlAccess.InsertNewVersuch(_versuch))
			{
				MsgWindow.Show("Datensatz konnte nicht geschrieben werden",SqlAccess.ErrorMsg, MessageLevel.Error);
				return;
			}
			ViewVisual = null;
			MsgWindow.Show("Neuen Versuch in Datenbank übernommen","", MessageLevel.Info);
		}

		#endregion


	}
}
