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
	/// Interaktionslogik für PropertyPerson.xaml
	/// </summary>
	public partial class PropertyPerson : Window
	{
		public PropertyPerson()
		{
			InitializeComponent();
		}


		public PropertyPerson(object dataSource)
		{
			InitializeComponent();
			DataContext = dataSource;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			((VmAllePersonen)DataContext).Validate();
		}
	}
}
