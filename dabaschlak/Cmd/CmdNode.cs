using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace dabaschlak
{
	class CmdNode: CmdBase
	{
		//public string Text { get; set; }
		//public Brush TextColor { get; set; }
		//public int Heigth { get; set; }
		public List<CmdBase> Content { get; }

		public CmdNode()
		{
			_isNode = true;
			Content = new List<CmdBase>();
      }
	}
}
