using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace dabaschlak
{
	public class VmBase:INotifyPropertyChanged, IDataErrorInfo
	{
		protected UserControl _viewVisual;
		protected Dictionary<string, string> _errorMessages;

		//protected SqlAccess _sqlAccess;


		protected VmBase()
		{
			//_sqlAccess = new SqlAccess();
			_errorMessages = new Dictionary<string, string>();
		}

		protected void SetAdminMode(bool mode)
		{
			OnPropertyChanged("IsAdmin");
			OnPropertyChanged("IsNotAdmin");

		}

		public bool IsAdmin
		{
			get { return GlobData.IsAdminMode; }

		}

		public bool IsNotAdmin
		{
			get { return !GlobData.IsAdminMode; }

		}

		public UserControl ViewVisual
		{
			get { return _viewVisual; }
			set 
			{ 
				_viewVisual = value;
				OnPropertyChanged("ViewVisual");
			}
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			try
			{
					PropertyChangedEventHandler handler = this.PropertyChanged;
					if (handler != null)
						handler(this, new PropertyChangedEventArgs(propertyName));
			}
			catch{}// wegen  seltsamem Tooltip-Fehler: so wird exception abgefangen - aber tooltip bleibt leer
		

		}

		#endregion

	
		#region IDataErrorInfo- Validierung

		string IDataErrorInfo.Error
		{
			get { return null; }
		}

		protected virtual String GetValidationError(string propertyName)
		{
			return this.GetValidationString(propertyName);
		}


		string IDataErrorInfo.this[string propertyName]
		{
			get { return this.GetValidationError(propertyName); }
		}


		#endregion

		#region Validation

		public string  GetErrorMessage(String key)
		{
			string error;	
			if (!_errorMessages.TryGetValue(key, out  error))
				return null;
			else
				return error;
		}

		protected String GetValidationString(String ElemName)
		{
			return GetErrorMessage(ElemName);
		}

		protected void ResetErrorList()
		{
			_errorMessages.Clear();
		}

		public bool ContainsError(String key)
		{
			return  _errorMessages.ContainsKey(key);
		}

		public void AddErrorMessage(String elemKey, String msg)
		{	
			if(!ContainsError(elemKey))
				_errorMessages.Add(elemKey,msg);
		}

		public bool HasErrors
		{
			get { return (_errorMessages.Count > 0); }
		}

		public bool CanApply
		{
			get { return _errorMessages.Count == 0; }
		}

		#endregion
	}
}
