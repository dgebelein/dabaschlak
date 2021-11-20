using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dabaschlak
{
	class VmExternFile : VmBase
	{
		public VmExternFile(string url)
		{
			ViewVisual = null;


			string[] urls = url.Split('|');
			string fn = urls[0];
			string userName= (urls.Length > 1) ? urls[1] : "";
			

			if (!fn.StartsWith("http"))
	      { 
				if (!File.Exists(fn))
				{
					MsgWindow.Show("Die Datei konnte nicht gefunden werden:", fn, MessageLevel.Error);
					return;
				}
			}

			string ext = Path.GetExtension(fn);
			if((ext==".xlsx") ||(ext== "docx"))
			{
				OfficeStarter.OpenFile(fn, userName);
				return;
			}

			System.Diagnostics.Process.Start(url);
			

		}


	}
}
