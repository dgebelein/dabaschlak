using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace dabaschlak
{

	public static class WebBrowserUtility
	{
		#region Uri
		public static readonly DependencyProperty BindableSourceProperty =
			 DependencyProperty.RegisterAttached("BindableSource", typeof(string), typeof(WebBrowserUtility), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

		public static string GetBindableSource(DependencyObject obj)
		{
			return (string)obj.GetValue(BindableSourceProperty);
		}

		public static void SetBindableSource(DependencyObject obj, string value)
		{
			obj.SetValue(BindableSourceProperty, value);
		}

		public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			WebBrowser browser = o as WebBrowser;
			if (browser != null)
			{
				string uri = e.NewValue as string;
				browser.Source = !String.IsNullOrEmpty(uri) ? new Uri(uri) : null;
			}
		}
		#endregion

		//#region html-code
		//public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
  //      "Html",
  //      typeof(string),
  //      typeof(WebBrowserUtility),
  //      new FrameworkPropertyMetadata(OnHtmlChanged));

		// [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
		// public static string GetHtml(WebBrowser d)
		// {
		//	  return (string)d.GetValue(HtmlProperty);
		// }

		// public static void SetHtml(WebBrowser d, string value)
		// {
		//	  d.SetValue(HtmlProperty, value);
		// }

		// static void OnHtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		// {
		//	  WebBrowser wb = d as WebBrowser;
		//	  if (wb != null)
		//			wb.NavigateToString(e.NewValue as string);
		// }
		//#endregion

	}
}