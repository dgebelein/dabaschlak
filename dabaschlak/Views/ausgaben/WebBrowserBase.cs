using Awesomium.Core;
using Awesomium.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace dabaschlak
{
	//class WebBrowserBase
	//{
	//	protected WebControl _browser;
	
	//	public delegate void ExecuteDelegate();

	//	protected void WindowUnloaded(object sender, RoutedEventArgs e)
	//	{
	//		// wichtig: weil sonst jede Seite einen neuen Prozess startet - bis zum absturz!
		
	//		//if(IsClosing)	// zur Unterscheidung: ob Fenster wirklich geschlossen oder nur ein Wechsel der Tab-ansicht
	//		//{
	//			_browser.Close();
			
	//			Dispatcher.CurrentDispatcher.BeginInvoke(	// notwendig ???
	//				DispatcherPriority.ApplicationIdle,
	//				new ExecuteDelegate(WebcoreUpdate));
			
	//		//}
	//	}

	//	private void WebcoreUpdate()
	//	{
	//		WebCore.Update();
	//	}
	//}
}
