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
	/// Interaktionslogik für ViewAuswahlVersuch.xaml
	/// </summary>
	public partial class ViewAuswahlVersuch : UserControl
	{
		public ViewAuswahlVersuch()
		{
			InitializeComponent();
		}
		
		private void _dgVersuche_ContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			if (_dgVersuche.SelectedIndex < 0)
				e.Handled = true;
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			_dgVersuche.SelectedIndex = -1;
		}
	}
}
