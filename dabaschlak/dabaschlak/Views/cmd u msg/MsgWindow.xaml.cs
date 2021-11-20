using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dabaschlak
{
	public enum MessageLevel { Error,Warning, Info};
	
	/// <summary>
	/// Interaktionslogik für MsgWindow.xaml
	/// </summary>
	public partial class MsgWindow : Window
	{

		private String _message;
		private String _detail;

		private MessageLevel _level;

		public MsgWindow()
		{
			InitializeComponent();
		}


		private MsgWindow(string message, String detail, MessageLevel level)
		{
			InitializeComponent(); 

			_message = message;
			_detail = detail;
			_level = level;	
			switch (_level){
				case MessageLevel.Error: msgSymbol.Source = new BitmapImage(new Uri(@"/dabaschlak;component/resources/Images/error.png", UriKind.Relative)); break;
				case MessageLevel.Warning: msgSymbol.Source = new BitmapImage(new Uri(@"/dabaschlak;component/resources/Images/warning.png", UriKind.Relative)); break;
				case MessageLevel.Info: msgSymbol.Source = new BitmapImage(new Uri(@"/dabaschlak;component/resources/Images/info.png", UriKind.Relative)); break;

				//case MessageLevel.Info: msgSymbol.Source = new BitmapImage(new Uri(@"pack://application:,,,/TtpResources;component/Images/info.png")); break;
	
			}
		}

		private void This_Loaded(object sender, RoutedEventArgs e)
		{
			Message.Text = _message;
			Detail.Text = _detail;
		}
		//-----------------------------------------------------------------------

		public static void Show(string message,String detail, MessageLevel level)
		{
			//var mc =Mouse.OverrideCursor;
			//Mouse.OverrideCursor= null;
			MsgWindow dlg = new MsgWindow(message, detail, level);
			dlg.ShowDialog();
			//Mouse.OverrideCursor = mc;
			
		}

		private void CmdClose(object sender, RoutedEventArgs e)
		{
			Close();
		}


	}
}
