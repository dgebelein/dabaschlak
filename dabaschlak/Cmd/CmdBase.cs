using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace dabaschlak
{
	public class CmdBase
	{
		public string Text { get; set; }
		public Brush TextColor { get; set; }
		public int Heigth { get; set; }
		public string TooltipText { get; set; }
		protected bool _isNode;

		public ToolTip CmdTooltip
		{ 
			get { return BuildTooltip(); }
		}

		private ToolTip BuildTooltip()
		{
			return  new ToolTip();
		}
	}
}
