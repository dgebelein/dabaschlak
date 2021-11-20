using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dabaschlak
{
	class ActionMail
	{
		public int PersonId { get;  }
		public string MailTo { get; }
		public string MailFrom { get; }
		public string Text { get; set; }
		
		public ActionMail( int id, string to, string from)
		{
			PersonId = id;
			MailTo = to;
			MailFrom = from;
		}
	}
}
