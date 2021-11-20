﻿using Awesomium.Core;
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
	/// Interaktionslogik für ViewVersuchsprotokoll.xaml
	/// </summary>
	public partial class ViewVersuchsprotokoll : UserControl
	{
		public ViewVersuchsprotokoll()
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
