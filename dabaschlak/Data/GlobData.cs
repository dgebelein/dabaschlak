using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dabaschlak
{
	public static class GlobData
	{
		#region variable

		
		static int _editYear;				// Jahr für neue Einträge
		static Person _user;					// aktueller Bearbeiter
		static bool _adminMode;
		static string _dbSource;
		static bool _hasUnsavedData;
		static bool _suspendMail;
		static List<Aktion> _addedActions;
		#endregion

		#region Constructor
		static GlobData()
		{
			_editYear = CurrentYear;
			_addedActions = new List<Aktion>();
		}

		#endregion

		#region Properties

		public static int CurrentYear
		{
			get {return DateTime.Now.Year; }
		}

		public static List<Aktion> AddedActions
		{
			get { return _addedActions; }
		}

		
		public static int EditYear
		{
			get {return _editYear; }
			set {_editYear = value; }
		}
				
		public static Person CurrentUser
		{
			get {return _user; }
			set 
			{
				_user = value;

				_adminMode = (_user== null) ?
					false :
					SqlAccess.IsAdmin(CurrentUser.PersonId);
			}
		}

		public static int OperatorId
		{ 
			get { return (CurrentUser == null)? -1 : CurrentUser.PersonId; } 
		}

		public static bool IsAdminMode
		{
			get { return _adminMode; }
			set { _adminMode = value; }
		}

		public static string DbSource
		{
			get { return _dbSource; }
			set { _dbSource = value; }

		}
		
		public static bool SuspendMail
		{
			get { return _suspendMail; }
			set { _suspendMail = value; }
		}

		public static bool HasUnsavedData
		{ 
			get { return _hasUnsavedData; }
			set { _hasUnsavedData = value; }
		}
		#endregion


	}
}
