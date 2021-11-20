using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace dabaschlak
{
	class VmAbfrageTexte:VmBase
	{
		#region Variable

		string _kulturText = "";
		string _versuchText = "";
		string _aktionNotizenText = "";
		string _aktionVersuchText = "";
		EnSearchTarget _searchTarget;




		#endregion

		#region Properties

		public EnSearchTarget SearchTarget
		{
			get { return _searchTarget; }
			set
			{
				_searchTarget = value;
				OnPropertyChanged("VisVersuche");
				OnPropertyChanged("VisAktionen");
				OnPropertyChanged("HeaderSuchTexte");

			}
		}

		public string KulturSuchText
		{
			get { return _kulturText; }
			set
			{
				_kulturText = value;
				OnPropertyChanged("KulturSuchText");
				OnPropertyChanged("HeaderSuchTexte");
			}
		}

		public string VersuchSuchText
		{
			get { return _versuchText; }
			set
			{
				_versuchText = value;
				OnPropertyChanged("VersuchSuchText");
				OnPropertyChanged("HeaderSuchTexte");
			}
		}

		public string AktionNotizenSuchText
		{
			get { return _aktionNotizenText; }
			set
			{
				_aktionNotizenText = value;
				OnPropertyChanged("NotizenSuchText");
				OnPropertyChanged("HeaderSuchTexte");
			}
		}

		public string AktionVersuchSuchText
		{
			get { return _aktionVersuchText; }
			set
			{
				_aktionVersuchText = value;
				OnPropertyChanged("AktionVersuchSuchText");
				OnPropertyChanged("HeaderSuchTexte");
			}
		}

		public Visibility VisVersuche
		{
			get { return (SearchTarget == EnSearchTarget.Versuch) ? Visibility.Visible : Visibility.Collapsed; }
		}

		public Visibility VisAktionen
		{
			get { return (SearchTarget == EnSearchTarget.Aktion) ? Visibility.Visible : Visibility.Collapsed; }
		}

		public string HeaderSuchTexte
		{
			get
			{
				if (SearchTarget == EnSearchTarget.Versuch)
				{
					string s = "";

					if (!string.IsNullOrWhiteSpace(_versuchText))
					{
						s += $"Versuch = {_versuchText}";
					}

					if (!string.IsNullOrWhiteSpace(_kulturText))
					{
						if (!string.IsNullOrWhiteSpace(s))
							s += " + ";
						s = $"Kultur = {_kulturText}";
					}


					return s;
				}
				else
				{
					string s = "";

					if (!string.IsNullOrWhiteSpace(_aktionVersuchText))
					{
						s += $"Versuch = {_aktionVersuchText}";
					}

					if (!string.IsNullOrWhiteSpace(_aktionNotizenText))
					{
						if (!string.IsNullOrWhiteSpace(s))
							s += " + ";
						s = $"Notizen = {_aktionNotizenText}";
					}

					return s;

				}
			}
		}

		#endregion
	}
}
