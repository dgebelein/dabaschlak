using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace dabaschlak
{
	class OfficeStarter
	{
		public static void OpenFile(string fileName, string fileOwner)
		{
			if (!File.Exists(fileName))
			{
				MsgWindow.Show("Die Datei konnte nicht gefunden werden.", fileName, MessageLevel.Error);
				return;
			}

			bool isOwner;
			if (String.IsNullOrEmpty(fileOwner))
				isOwner = true;
			else
				isOwner =(string.Compare(fileOwner,GlobData.CurrentUser.FullName,true)==0);

			if (isOwner)
				Process.Start(fileName);
			else
			{
				switch(Path.GetExtension(fileName))
				{
					case ".docx":OpenWordReadOnly(fileName); break;
					case ".xlsx":OpenExcelReadOnly(fileName); break;
					default: break;
            }
			}

		}


		private static void OpenWordReadOnly(string fileName)
		{
			Microsoft.Office.Interop.Word.Application winword = null;
			try
			{
				winword = new Microsoft.Office.Interop.Word.Application();
				winword.Documents.Open(fileName, ReadOnly: true);
				winword.Visible = true;
				//winword.WindowState = WdWindowState.wdWindowStateMaximize;
				winword.Activate();
			}
			catch (Exception)
			{
				if (winword != null)
					winword.Quit();
			}

		}

		private static void OpenExcelReadOnly(string fileName)
		{
			Microsoft.Office.Interop.Excel.Application excelApp = null;
			try
			{
				excelApp = new Microsoft.Office.Interop.Excel.Application();
				excelApp.Workbooks.Open(fileName, ReadOnly: true);
				excelApp.Visible = true;
			}
			catch (Exception)
			{
				if (excelApp != null)
					excelApp.Quit();
			}

		}
	}
}

