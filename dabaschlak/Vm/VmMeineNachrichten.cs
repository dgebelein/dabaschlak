using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dabaschlak
{
	class VmMeineNachrichten:VmBase
	{

		#region Variable

		DataTable _dtNachrichten;


		#endregion


		#region Construction and Init

		public VmMeineNachrichten()
		{
			ViewVisual = new ViewMeineNachrichten();
			ReadData();
		}

		#endregion

		#region DataIO

		void ReadData()
		{
			DataTableNachrichten = SqlAccess.GetMeineNachrichtenTable(GlobData.OperatorId);
			if (_dtNachrichten == null)
			{
				MsgWindow.Show("Versuchsarbeiten konnten nicht gelesen werden", SqlAccess.ErrorMsg, MessageLevel.Error);
				return;
			}

			_dtNachrichten.Columns.Add("NotizenLine");
			foreach(DataRow row in _dtNachrichten.Rows)
			{
				if (row["Notizen"] is System.DBNull)
					continue;
				
					row["NotizenLine"] = ((string) row["Notizen"]).Replace(System.Environment.NewLine, " | ");
			}

		}

		#endregion

		#region Properties

		public DataTable DataTableNachrichten
		{
			get { return _dtNachrichten; }
			set 
			{
				_dtNachrichten = value;
				OnPropertyChanged("DataTableNachrichten");
			}
		}

		public string HeaderPerson
		{
			get { return GlobData.CurrentUser.FullName; }
		}

		#endregion
	}
}
