using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dabaschlak
{
	class VmBrowser:VmBase
	{
		//public string FileAdresse { get; set; }

		public VmBrowser(string Adresse)
		{
			//FileAdresse = Adresse;
			ViewVisual = new ViewWeb();
			if(string.IsNullOrWhiteSpace(Adresse))
			{
				MsgWindow.Show("für diesen Menüpunkt ist noch kein Link definiert!","", MessageLevel.Error);

			}
			else {
				var uri = new Uri(Adresse, UriKind.Absolute);
				((ViewWeb)ViewVisual)._aweBrowser.Source = uri; 
			}
		}
	}
}
