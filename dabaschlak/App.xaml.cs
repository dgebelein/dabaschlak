using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace dabaschlak
{
	/// <summary>
	/// Interaktionslogik für "App.xaml"
	/// </summary>
	public partial class App : Application
	{
		

		protected override void OnStartup(StartupEventArgs e)
		{
			
			//AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

			//DoExecute();
			//this.Properties["MRU"] = Settings.Default.MRU;

			//// Wenn eine Recorder-Projektdatei als Startparameter angegeben wurde, wird das Recording
			//// unmittelbar nach dem Laden der Projektdatei selbstständig gestartet
			//// Die Angabe einer projektdatei in der Kommandozeile impliziert also einen Autostart
			//this.Properties["CommandFile"] = (e.Args.Length > 0) ? e.Args[0] : "";


			//CultureInfo culture = CultureInfo.InvariantCulture;// keine nationalen besonderheiten wegen zahlenformaten ( parser)


			// *******************************************************************
			// TODO - Uncomment one of the lines of code that create a CultureInfo
			// in order to see the application run with localized text in the UI.
			// *******************************************************************



			//culture = new CultureInfo("it-IT");
			//culture = new CultureInfo("fr-CH");
			//culture = new CultureInfo("de-DE");

			//if (culture != null)
			//{
			//   Thread.CurrentThread.CurrentCulture = culture; 
			//   Thread.CurrentThread.CurrentUICulture = culture;
			//}

			// Ensure the current culture passed into bindings is the OS culture.
			// By default, WPF uses en-US as the culture, regardless of the system settings.
			//FrameworkElement.LanguageProperty.OverrideMetadata(
			//  typeof(FrameworkElement),
			//  new FrameworkPropertyMetadata(
			//      XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

			base.OnStartup(e);
		}

		//------------------------------------------------------------------------------------
		protected override void OnExit(ExitEventArgs e)
		{
			try
			{
				//Settings.Default.Save();
				DeleteTemporaryFiles();
				SendActionMails();
			}
			catch { }

			base.OnExit(e);
		}	

	// löscht alle html-Dateien im temporärenVerzeichnis
		private void DeleteTemporaryFiles()
		{
			String tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "dabaschlak");

			try {
				DirectoryInfo di = new DirectoryInfo(tempPath);
				FileInfo[] files = di.GetFiles();
				foreach (FileInfo file in files) {
					if (String.Compare(file.Extension, ".html", true) == 0) 
						System.IO.File.Delete(file.FullName);	
				}	
			}
			catch (Exception)
			{}
		}	
		
		private void SendActionMails()
		{
			ActionMailSender mailSender = new ActionMailSender();
			mailSender.Execute();
		}			
	}
}
