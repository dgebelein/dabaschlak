using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Awesomium.Core;

namespace dabaschlak
{
	/// <summary>
	/// Interaktionslogik für ViewAbfrage.xaml
	/// </summary>
	public partial class ViewAbfrage : UserControl
	{
		public ViewAbfrage()
		{
			InitializeComponent();
		}

		// Anzeige Contextmenü unterbinden
		private void _aweBrowser_ShowContextMenu(object sender, Awesomium.Core.ContextMenuEventArgs e)
		{
			e.Handled = true;
		}

		protected void WindowUnloaded(object sender, RoutedEventArgs e)
		{
			// wichtig: weil sonst jede Seite einen neuen Prozess startet - bis zum absturz!

				_aweBrowser.Close();
				WebCore.Update();
		}
	}
}
