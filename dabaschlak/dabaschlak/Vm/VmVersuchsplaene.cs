using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TTP.UiUtils;

namespace dabaschlak
{
	class VmVersuchsplaene:VmBase
	{
		#region Variable

		DataTable _dtVersuchsplaene;
		int _selectedRowIndex; 


		RelayCommand _showFileCommand;

		#endregion
		#region Construction and Init

		public VmVersuchsplaene()
		{
			ViewVisual = new ViewVersuchsplaene();
			_showFileCommand = new RelayCommand(param => this.ShowFile());

			ReadData();
		}

		#endregion
		#region DataIO

		void ReadData()
		{
			//DataTable dt = SqlAccess.GetVersuchsTable(GlobData.CurrentYear, "");
			DataTable dt = SqlAccess.GetVersuchsTable(0, ""); // 30.11.2020

			if (dt == null)
			{
				MsgWindow.Show("Versuchsplantabelle konnte nicht gelesen werden", SqlAccess.ErrorMsg, MessageLevel.Error);
				return;
			}

			dt.Columns.Add("Age",typeof(System.Int32));
			dt.Columns.Add("AgeText");
			dt.Columns.Add("Datum");
			dt.Columns.Add("VToolTip", typeof(ToolTip));


			foreach (DataRow row in dt.Rows)
			{
				if (row["Versuchsplan"] is System.DBNull)
					continue;
				
				string fileName = ((string) row["Versuchsplan"]);
				if (File.Exists(fileName))
				{ 
					int age = (DateTime.Today - File.GetLastWriteTime(fileName).Date).Days;
					row["Age"] = age;
					if (age > 365)
					{
						row["AgeText"] = "mehr als ein Jahr";
					}
					else
					{ 
						switch(age)
						{ 
							case 0:	row["AgeText"] = "heute"; break;
							case 1:	row["AgeText"] = "gestern"; break;
							case 2:	row["AgeText"] = "vorgestern"; break;
							default: row["AgeText"] = "vor " +age+ " Tagen"; break;
						}
					}


					row["Datum"] = File.GetLastWriteTime(fileName).ToString("dd.MM.yyyy");
					row["VToolTip"] = GetToolTip(row);
				}
			}
			dt.DefaultView.Sort = "Age ASC";
			
			DataTableVersuchsplaene = dt.DefaultView.ToTable();

		}

		ToolTip GetToolTip(DataRow row)
		{
			List<String> lines = new List<string>();
	
			lines.Add("_bd__sb_" + "Versuch");
			lines.Add(Convert.ToString(row["VerBez"]));

			lines.Add("_bd__sb_" + "Versuchsleiter");
			lines.Add(Convert.ToString(row["Versuchsleiter"]));

			lines.Add("_bd__sb_" + "Versuchspflanze");
			lines.Add(Convert.ToString(row["Kultur"]));
			
			lines.Add("_bd__sb_Versuchsfrage");
			lines.Add(Convert.ToString(row["Versuchsfrage"]));

			//Todo: Padding in Tooltip, Begrenzung der Breite
			return ItemTooltip.CreateFromText(lines, 300);

		}
		#endregion

		#region Properties

		public DataTable DataTableVersuchsplaene
		{
			get { return _dtVersuchsplaene; }
			set 
			{
				_dtVersuchsplaene = value;
				OnPropertyChanged("DataTableVersuchsplaene");
			}
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

		#endregion

		#region command-execution

		public ICommand ShowFileCommand
		{
			get{return _showFileCommand;}
		}

		void ShowFile()
		{
			string fileName = (string)_dtVersuchsplaene.Rows[SelectedRowIndex]["Versuchsplan"];
			string fileOwner = (string)_dtVersuchsplaene.Rows[SelectedRowIndex]["Versuchsleiter"];
			OfficeStarter.OpenFile(fileName, fileOwner);
		}

		#endregion
	}
}
