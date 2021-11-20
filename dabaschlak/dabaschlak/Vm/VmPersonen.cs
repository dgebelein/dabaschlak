using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace dabaschlak
{
	class VmPersonen:VmBase
	{
		//UserControl _viewVis;
		DataTable _dtPersons = null;
		RelayCommand _updateCommand;


		public VmPersonen()
		{
			ViewVisual = new ViewPersonen();
		}

		void ReadData()
		{
			_dtPersons = SqlAccess.GetAllPersons();
			
			if (_dtPersons == null)
			{


			}
			// evtl Test auf null mit Fehlermeldung


		}

		void UpdateData()
		{
			if(!SqlAccess.UpdatePersons(_dtPersons))
			{
				MsgWindow.Show("Datensatz konnte nicht geschrieben werden",SqlAccess.ErrorMsg, MessageLevel.Error);

			}

		}

		public DataTable DataTablePersonen
		{
			get 
			{
				if (_dtPersons == null)
					ReadData();
				return _dtPersons;
			}
			set 
			{
				_dtPersons = value;
				OnPropertyChanged("DataTablePersonen");
			}
		}



		public ICommand UpdateCommand
		{
			get
			{
				if (_updateCommand == null)
					_updateCommand = new RelayCommand(
						param => this.OnUpdateCommand(),
						param => this.CanUpdate);

				return _updateCommand;
			}
		}

		bool CanUpdate
		{
			get {
				return (_dtPersons!= null) && GlobData.IsAdminMode;
			}
		}

		void OnUpdateCommand()
		{
			UpdateData();
		}

	}
}
