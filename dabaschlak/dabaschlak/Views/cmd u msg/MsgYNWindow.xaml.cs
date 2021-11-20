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
	/// <summary>
	/// Interaktionslogik für MsgYNWindow.xaml
	/// </summary>
	public partial class MsgYNWindow : Window
	{	
		private String _message;
		private String _detail;
		private MessageLevel _level;
		private bool _retVal;

		
		
		public MsgYNWindow()
		{
			InitializeComponent();
		}

		private MsgYNWindow(string message, String detail, MessageLevel level)
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

		public static bool Show(string message,String detail, MessageLevel level)
		{

			MsgYNWindow dlg = new MsgYNWindow(message, detail, level);
			dlg.ShowDialog();
			return dlg._retVal;
			
		}

		private void CmdYes(object sender, RoutedEventArgs e)
		{
			This._retVal = true;
			This.Close();
		}

		private void CmdNo(object sender, RoutedEventArgs e)
		{
			This._retVal = false;
			This.Close();
		}
	}
}
