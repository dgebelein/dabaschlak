using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dabaschlak
{
	public class VersuchArgs:EventArgs
	{
		public int IdVersuch { get; set; }
		//public int IdFlaeche { get; set; }
		public int Start { get; set; }
		public int Ende { get; set; }
	}
}
