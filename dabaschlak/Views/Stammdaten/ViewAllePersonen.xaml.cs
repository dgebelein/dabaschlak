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
	/// Interaktionslogik für ViewPersonen.xaml
	/// </summary>
	public partial class ViewAllePersonen : UserControl
	{
		public ViewAllePersonen()
		{
			InitializeComponent();
		}

		private void ContextMenu_Opening(object sender, ContextMenuEventArgs   e)
		{
			if(_dgPersonen.SelectedItem == null)
			{
				e.Handled = true;
				return;
			}

			_dgPersonen.ContextMenu.DataContext = DataContext;

		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			_dgPersonen.SelectedIndex = -1;
		}
	}
}
