using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dabaschlak
{
	public class ActionMailSender
	{
		List<ActionMail> _mails;

		public ActionMailSender()
		{
			_mails = new List<ActionMail>();
		}

		public void Execute()
		{
			if(!CreateEmptyMails())
				return;

			if(FillMails())
				SendMails();
		}
		
		private bool CreateEmptyMails()
		{
			DataTable persons = SqlAccess.GetPersonsTable();
			if (persons == null)
				return false;

			foreach(DataRow row in persons.Rows)
			{
				int id = Convert.ToInt32(row["PersonId"]);
				bool aktiv = Convert.ToBoolean(row["Aktiv"]);
				string email = Convert.ToString(row["Email"]);

				if(aktiv && !String.IsNullOrEmpty(email))
				{
					_mails.Add(new ActionMail(id, email, "dabaschlak"));
				}
			}
			return true;


		}

		private bool FillMails()
		{
			foreach(Aktion aktion in GlobData.AddedActions)	
			{
				if (aktion.VersuchId <= 0)
					continue;

				string mailText = aktion.ToString();
				if (string.IsNullOrEmpty(mailText))
					return false;

				List<int> personIds = SqlAccess.GetMailPersonsForAction(aktion.VersuchId); 
				foreach(int personId in personIds)
				{
					ActionMail m = _mails.Find(x => x.PersonId == personId);
					if (m != null)
						m.Text += mailText;
				}
			}

			return true;;
		}

		
		private void SendMails()
		{
			foreach(ActionMail m in _mails)
			{
				if (!string.IsNullOrWhiteSpace(m.Text))
					SendMailToPerson(m);

			}
		}

		private void SendMailToPerson(ActionMail mail)
		{

		}
		
	}
}
