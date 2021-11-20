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
	/// Interaktionslogik für ViewArbeitenListe.xaml
	/// </summary>
	public partial class ViewArbeitenListe : UserControl
	{
		public ViewArbeitenListe()
		{
			InitializeComponent();
		}

		private void _dgArbeiten_ContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			if (_dgArbeiten.SelectedIndex < 0)
				e.Handled = true;
		}
	}
}
