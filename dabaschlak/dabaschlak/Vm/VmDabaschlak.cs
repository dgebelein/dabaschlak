using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;

namespace dabaschlak
{
	public class VmDabaschlak : VmBase
	{
		VmBase _viewDataContext;

		Dictionary<int, string> _dictFullNames;
		Dictionary<int, string> _dictNetnames;

		List<string> _fullNames;	
		string _userName; // Name im Netz

		CmdMenu _mainMenu;
		CmdBase _selectedMenuItem;

		public VmDabaschlak()
		{
			_mainMenu = new CmdMenu();
			GlobData.DbSource = _mainMenu.GetFileUrl(0);// Datenbankname

			AnalyseCommandLine();

			_dictFullNames = SqlAccess.GetUserDict();
			_dictNetnames = SqlAccess.GetNetNamesDict();
			_fullNames = _dictFullNames.Values.ToList();
			_fullNames.Sort();


			_mainMenu = new CmdMenu();

			ViewVisualDataContext = new VmBrowser(_mainMenu.GetFileUrl(1)); // Intro
		}

		void AnalyseCommandLine()
		{
			Dictionary<string, string> dict = new Dictionary<string, string>();
			
			string[] args = Environment.GetCommandLineArgs();
			try { 
				for (int index = 1; index < args.Length; index += 2)
				{
					  dict.Add(args[index], args[index+1]);
				}
			}
			catch
			{
				MsgWindow.Show("Kommandozeilen-Parameter ungültig.", "Programm wird beendet", MessageLevel.Error);
				Application.Current.Shutdown();
			}

			string db;
			if (dict.TryGetValue("-db",out db))
			{
				GlobData.DbSource = db;
			}
			if(!File.Exists(GlobData.DbSource))
			{
				MsgWindow.Show("Datenbank wurde nicht gefunden:", GlobData.DbSource, MessageLevel.Error);
				Application.Current.Shutdown();
			}

			
			string userName;
			if (dict.TryGetValue("-user",out userName))
			{
				GlobData.CurrentUser = SqlAccess.FindPersonFromUsername(userName);
			}
			else
				GlobData.CurrentUser = SqlAccess.FindPersonFromUsername(System.Environment.UserName);
			
			
			if(GlobData.CurrentUser == null)
			{
				MsgWindow.Show("Sie sind nicht als Nutzer dieser Datenbank registriert.", "Programm wird beendet", MessageLevel.Error);
				Application.Current.Shutdown();
			}

			string mail;
			if (dict.TryGetValue("-mail",out mail))
			{
				GlobData.SuspendMail = (string.Compare("no",mail, true)== 0);
			}

		}

		
		public List<string> UserNames
		{
			get { return _fullNames; }
			set
			{
				_fullNames = value;
				OnPropertyChanged("UserNames");
			}
		}

		private string FindNetname(string FullName)
		{
			if (!_dictFullNames.ContainsValue(FullName))
				return null;

			var k = _dictFullNames.FirstOrDefault(x => x.Value == FullName).Key;
			return _dictNetnames[k];
		}

		public string UserName
		{
			get
			{
				return GlobData.CurrentUser.FullName;
			}
			set
			{
				_userName = FindNetname(value);
				GlobData.CurrentUser = SqlAccess.FindPersonFromUsername(_userName);
				if (GlobData.CurrentUser == null)
				{
					MsgWindow.Show("Sie sind nicht als Nutzer dieser Datenbank registriert.", "Programm wird beendet", MessageLevel.Error);
					Application.Current.Shutdown();
				}
			}
		}

		public Visibility VisUserSelect
		{
			get {return  (GlobData.IsAdminMode)? Visibility.Visible : Visibility.Collapsed; }
		}


		public Visibility VisUserLabel
		{
			get { return  (!GlobData.IsAdminMode)? Visibility.Visible : Visibility.Collapsed; }
		}

		public VmBase ViewVisualDataContext
		{
			get { return _viewDataContext; }
			set
			{
				_viewDataContext = value;
				OnPropertyChanged("ViewVisualDataContext");
			}
		}


		public List<CmdBase> MainMenuItems
		{
			get { return _mainMenu.Items; }
		}

		public CmdBase SelectedMenuItem
		{
			get { return _selectedMenuItem; }
			set
			{
				_selectedMenuItem = value;
				if (_selectedMenuItem is CmdItem)
				{
					HandleMenuItem(((CmdItem)_selectedMenuItem));
				}
			}
		}


		private void HandleMenuItem(CmdItem cmd)
		{
			if (ViewVisualDataContext is VmNachrichtenzuordnung && GlobData.HasUnsavedData)
			{
				((VmNachrichtenzuordnung)ViewVisualDataContext).RememberUpdate();
			}
			switch (cmd.Response)
			{
				case CmdResponse.VersuchsArbeitenDokumentieren: ViewVisualDataContext = new VmArbeiten( ArbeitenTyp.Versuchsarbeit); break;
				case CmdResponse.FlaechenArbeitenDokumentieren: ViewVisualDataContext = new VmArbeiten( ArbeitenTyp.Flaechenarbeit); break;
				case CmdResponse.ZeigeAlleVersuche:		ViewVisualDataContext = new VmAlleVersuche(); break;
				case CmdResponse.ZeigeAlleFlaechen:		ViewVisualDataContext = new VmAlleFlaechen(); break;
				case CmdResponse.ZeigeAllePersonen:		ViewVisualDataContext = new VmAllePersonen(); break;
				case CmdResponse.Nachrichtenzuordnung: ViewVisualDataContext = new VmNachrichtenzuordnung(); break;
				case CmdResponse.MeineNachrichten:		ViewVisualDataContext = new VmMeineNachrichten(); break;
				case CmdResponse.LetzteVersuchsplaene:	ViewVisualDataContext = new VmVersuchsplaene(); break;

				case CmdResponse.Browser:					ViewVisualDataContext = new VmBrowser(cmd.Url); break;
				case CmdResponse.ExternFile:				ViewVisualDataContext = new VmExternFile(cmd.Url); break;

				case CmdResponse.Versuchsprotokoll:		ViewVisualDataContext = new VmVersuchsprotokoll(); break;
				case CmdResponse.Abfrage:					ViewVisualDataContext = new VmAbfrage(); break;
				case CmdResponse.FlexItem:					ProcessFlexItem(cmd); break;

			}

		}

		void  ProcessFlexItem(CmdItem cmd)
		{
			var browserExt = new List<string>();
			browserExt.Add(".html");
			browserExt.Add(".htm");
			browserExt.Add(".svg");


			string[] urls = cmd.Url.Split('|');
			string fn = urls[0];
			

			string ext = Path.GetExtension(fn);
			if (browserExt.Contains(ext, StringComparer.OrdinalIgnoreCase))
				ViewVisualDataContext = new VmBrowser(cmd.Url);
			else
				ViewVisualDataContext = new VmExternFile(cmd.Url);
		}
	}
}
