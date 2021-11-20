using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TTP.UiUtils;

namespace dabaschlak
{
   class VmAlleVersuche:VmBase
   {
      #region Variable

      object _parent;
      DataTable _dtVersuche;
      DataRowView _selectedRow; // für  ViewAlle Versuche
      DataRowView _selectedVersuch;	// für Auswahl bei Versuchsarbeiten


      bool _doFilterYears;
      int _filterJahr;
      List<int> _versuchsjahre;

      bool _doFilterTypes;
      string _filterFlaeTyp;
      List<string> _flaechentypen;

      Propertymode _propMode;
      bool _isRowChanged;
      bool _propReadonly;
      Versuch _editedVersuch;
      PropertyVersuch _propVersuch;

      DataTable _dtStandorte;
      //List<int> _idsStandorte;
      PropertyAuswahlFlaechen _propSelFlaechen;

      Dictionary<int, string> _dictUsers;
      //Dictionary<int, string> _dictFlaechen;


      List<string> _propUsernames;
      //List<string> _propListVersuchsflaechen;


      RelayCommand _removeCommand;
      RelayCommand _addCommand;
      RelayCommand _showFileCommand;
      RelayCommand _propertiesCommand;
      RelayCommand _propCloseCommand;
      RelayCommand _propUpdateCommand;
      RelayCommand _selectFileCommand;
      RelayCommand _standorteSelectCommand;
      RelayCommand _standorteUpdateCommand;
      RelayCommand _standorteCloseCommand;




      public delegate void SelectionChangedHandler(object sender, VersuchArgs e);
      public event SelectionChangedHandler SelectionChanged;

      #endregion

      #region construction and init

      public VmAlleVersuche()
      {	
         ViewVisual = new ViewAlleVersuche();
         Init();
         ReadData();
      }

      public VmAlleVersuche(object parent)
      {
         _parent = parent;
         _selectedVersuch = null;
         Init();
         ReadData();
      }


      void Init()
      {
         //Foreign Keys Dicts für Klartexte
         _dictUsers = SqlAccess.GetUserDict();
         _propUsernames = _dictUsers.Values.ToList();
         _propUsernames.Sort();

         //_dictFlaechen = SqlAccess.GetFlaechenDict();
         //_propListVersuchsflaechen = _dictFlaechen.Values.ToList();
         //_propListVersuchsflaechen.Sort();

         // Filter
         _doFilterYears = true;
         _filterJahr = DateTime.Now.Year;
         _versuchsjahre = new List<int>(20);
         for (int n = 2010; n <= 2030; n++)
            _versuchsjahre.Add(n);

         _doFilterTypes = false;
         _flaechentypen = Flaeche.GetFlaechentypenMz();
         _filterFlaeTyp = _flaechentypen[0];
         
         // Commands
         _addCommand = new RelayCommand(param => this.AddRow(), param => this.CanAdd);
         _propertiesCommand = new RelayCommand(param => this.ShowProperties());
         _removeCommand = new RelayCommand(param => this.RemoveRow(), param => this.CanRemove);
         _propUpdateCommand = new RelayCommand(param => this.PropUpdate(), param => this.CanPropUpdate);
         _propCloseCommand = new RelayCommand(param => this.PropClose());
         _showFileCommand = new RelayCommand(param => this.ShowFile());
         _selectFileCommand = new RelayCommand(param => this.SelectFile());
         _standorteSelectCommand = new RelayCommand(param => this.StandorteSelect(), param => this.CanStandorteSelect);
         _standorteUpdateCommand = new RelayCommand(param => this.StandorteUpdate());
         _standorteCloseCommand = new RelayCommand(param => this.StandorteClose());


      }

      #endregion

      #region Data + Events

      void ReadData()
      {
         int fj = (_doFilterYears) ? _filterJahr : 0;
         string ft = (_doFilterTypes) ? _filterFlaeTyp : "";
         DataTableVersuche = SqlAccess.GetVersuchsTable(fj, ft);
         _selectedVersuch = null;

         if (_dtVersuche == null)
         {
            MsgWindow.Show("Versuchs-Daten konnten nicht gelesen werden", SqlAccess.ErrorMsg, MessageLevel.Error);
         }
         else
         {
            _dtVersuche.Columns.Add("Jahr", typeof(string));
            _dtVersuche.Columns.Add("Details", typeof(string));
            _dtVersuche.Columns.Add("VToolTip", typeof(ToolTip));

            foreach(DataRow dr in _dtVersuche.Rows)
            {
               dr["Jahr"] = (Convert.ToInt32(dr["Start"])==Convert.ToInt32(dr["Ende"])) ? dr["Start"] : dr["Start"] + " - " + dr["Ende"];
               dr["Details"] = "Versuchsfrage: \n"+ Convert.ToString(dr["Versuchsfrage"]);
               dr["VToolTip"] = GetToolTip(dr);
            }
         }
      }
      
      ToolTip  GetToolTip(DataRow row)
      {
         List<String> lines = new List<string>();


         if(_parent!=null) // Auswahl-Listen
         { 
            lines.Add("_bd__sb_" + "Versuchsleiter");
            lines.Add(Convert.ToString(row["Versuchsleiter"]));
            lines.Add("_bd__sb_" + "Versuchspflanze");
            lines.Add(Convert.ToString(row["Kultur"]));
         }


         lines.Add("_bd__sb_Versuchsfrage");
         lines.Add(Convert.ToString(row["Versuchsfrage"]));

         return ItemTooltip.CreateFromText(lines,300);
      
      }

      public Versuch GetVersuchFromId(int idVersuch)
      {
         foreach (DataRow row in _dtVersuche.Rows)
         {
            if(idVersuch==Convert.ToInt32(row["VersuchId"]))
            {
               Versuch v = new Versuch();
               v.Id = Convert.ToInt32(row["VersuchId"]); 
               v.Start = Convert.ToInt32(row["Start"]);
               v.Ende = Convert.ToInt32(row["Ende"]);
               v.VerBez = Convert.ToString(row["VerBez"]);
               v.Plan = Convert.ToString(row["Versuchsplan"]);
               v.Frage = Convert.ToString(row["Versuchsfrage"]);
               v.Kultur = Convert.ToString(row["Kultur"]);
               v.LeiterTxt = Convert.ToString(row["Versuchsleiter"]);
               v.Standorte = Convert.ToString(row["Standorte"]).Replace(",",", ");
               AssignStandortIds(v, true);
               return v;
            }
         }
         return null;
      }

      private void AssignStandortIds(Versuch v, bool doInit)
      {
         if(doInit) // aus der Datenbank
         { 
            v.IdsStandorte = new List<int>();
            _dtStandorte = SqlAccess.GetFlaechenzuordnungTable(v.Id,"");
            if (_dtStandorte == null)
            {
               MsgWindow.Show("Zuordnung des Versuchsstandortes konnte nicht gelesen werden", SqlAccess.ErrorMsg, MessageLevel.Error);
               return;
            }

            foreach (DataRow row in _dtStandorte.Rows)
            {
               if (Convert.ToInt32(row["doAssign"]) != 0)
                  v.IdsStandorte.Add(Convert.ToInt32(row["FlaeId"]));
            }
         }
         else // aus dem Versuch in Bearbeitung
         {
            if(_dtStandorte== null) // neuer Versuch!
               _dtStandorte = SqlAccess.GetFlaechenzuordnungTable(v.Id,"");	
            
               foreach (DataRow row in _dtStandorte.Rows)
            {
               row["doAssign"] = (v.IdsStandorte.Contains(Convert.ToInt32(row["FlaeId"]))) ? 1 : 0;
            }
         }
      }

      protected void OnSelectionChanged(int versuchId, int start, int ende)
      {
         if (SelectionChanged != null)
            SelectionChanged(this, new VersuchArgs {IdVersuch = versuchId, Start=start, Ende=ende});
      }

      #endregion

      #region properties

      public string Header
      {
         get
         {
            string h = "Versuche ";
            if (_doFilterYears) h += _filterJahr;
            if (_doFilterTypes) h += "  " + _filterFlaeTyp;
            return h;
         }
      }

      public DataTable DataTableVersuche
      {
         get
         {
            return _dtVersuche;
         }
         set
         {
            _dtVersuche = value;
            OnPropertyChanged("DataTableVersuche");
         }
      }

      public DataRowView SelectedRow
      {
         get { return _selectedRow; }
         set 
         { 
         _selectedRow = value;
         OnPropertyChanged("SelectedRow"); 
         }
      }

      public DataRowView SelectedVersuch
      {
         get { return _selectedVersuch; }
         set 
         {
            if(_selectedVersuch!=value && value != null)
            { 
               _selectedVersuch = value;
               DataRow row = _selectedVersuch.Row;
               int idVersuch = Convert.ToInt32(row["VersuchId"]);
               int start=Convert.ToInt32(row["Start"]);
               int ende=Convert.ToInt32(row["Ende"]);

               OnSelectionChanged(idVersuch, start, ende);
            }
            OnPropertyChanged("SelectedVersuch"); 
            
         }
      }

      public bool DoFilterYears
      {
         get { return _doFilterYears;}
         set 
         {
            _doFilterYears = value;
            OnPropertyChanged("DoFilterYears");
            OnPropertyChanged("Header");
            ReadData();
         }
      }	
      
      public List<int> Versuchsjahre
      {
         get { return _versuchsjahre;}

      }

      public int FilterJahr
      {
         get { return _filterJahr; }
         set 
         {
            if (_filterJahr == value)
               return;

            _filterJahr = value;
            OnPropertyChanged("FilterJahr");
            OnPropertyChanged("Header");
            ReadData();
          }

      }

      public bool DoFilterTypes
      {
         get { return _doFilterTypes; }
         set
         {
            _doFilterTypes = value;
            OnPropertyChanged("DoFilterTypes");
            OnPropertyChanged("Header");
            ReadData();
         }
      }

      public List<string> Flaechentypen
      {
         get { return _flaechentypen; }

      }

      public string FilterFlaeTyp
      {
         get { return _filterFlaeTyp; }
         set
         {
            if (_filterFlaeTyp == value)
               return;

            _filterFlaeTyp = value;
            OnPropertyChanged("FilterFlaeTyp");
            OnPropertyChanged("Header");
            ReadData();
         }

      }

      #endregion

      #region Properties PropDialog


      public bool PropReadOnly
      {
         get { return _propReadonly; }
      }

      public bool PropIsAdmin
      {
         get { return GlobData.IsAdminMode; }
      }

      public bool PropEnabled
      {
         get { return !_propReadonly; }
      }
      
      public Visibility VisReadonly
      {
         get { return (_propReadonly) ? Visibility.Visible : Visibility.Hidden; }
      }

      public string PropBezeichnung
      {
         get { return _editedVersuch.VerBez; }
         set 
         {
            if(_editedVersuch.VerBez != value)
            {
               _editedVersuch.VerBez = value;
               _isRowChanged = true;
               Validate();
            }
         }
      }

      public int PropStart
      {
         get { return _editedVersuch.Start; }
         set 
         {
            if(_editedVersuch.Start != value)
            { 
               _editedVersuch.Start = value;
               _isRowChanged = true;
               Validate();
            }
         }		 
      }
      
      public int PropEnde
      {
         get { return _editedVersuch.Ende; }
         set 
         {
            if(_editedVersuch.Ende != value)
            { 
               _editedVersuch.Ende = value;
               _isRowChanged = true;
               Validate();
            }
            
         }		 
      }

      public List<string> PropUsernames
      {
         get { return _propUsernames; }
      }

      public string PropVersuchsleiter
      {
         get { return _editedVersuch.LeiterTxt; }
         set 
         {
            if(_editedVersuch.LeiterTxt != value)
            {
               _editedVersuch.LeiterTxt = value;
               _isRowChanged = true;
               Validate();
            }
         }
      }

      public string PropStandorte
      {
         get { return _editedVersuch.Standorte; }
         set { 
            _editedVersuch.Standorte = value;
            Validate();
         }
      }

      public string PropVersuchsplan
      {
         get { return _editedVersuch.Plan; }
         set 
         {
            if(_editedVersuch.Plan != value)
            {
               _editedVersuch.Plan = value;
               _isRowChanged = true;
               Validate();
            }
         }
      }

      public string PropVersuchsfrage 
      {
         get { return _editedVersuch.Frage; }
         set 
         {
            if(_editedVersuch.Frage != value)
            {
               _editedVersuch.Frage = value;
               _isRowChanged = true;
               Validate();
            }
         }
      }

      public string PropKultur 
      {
         get { return _editedVersuch.Kultur; }
         set 
         {
            if(_editedVersuch.Kultur != value)
            {
               _editedVersuch.Kultur = value;
               _isRowChanged = true;
               Validate();
            }
         }
      }

      public ToolTip ReadonlyTooltip
      {
         get
         {
            List<String> lines = new List<string>();
            lines.Add("_tt__sb__sa_Schreibgeschützte Daten");
            lines.Add("Die Basisdaten eines Versuchs können nur ");
            lines.Add("vom 'Versuchsleiter' geändert werden. ");

            return ItemTooltip.CreateFromText(lines, 250, "#90EE90");

         }
      }

      public DataTable DataTableStandorte
      {
         get { return _dtStandorte; }
         set {
            _dtStandorte = value;
            //OnPropertyChanged("DataTableStandorte");
         }

      }


      #endregion
      
      #region Validierung

      void Validate()
      {
         ResetErrorList();


         if (String.IsNullOrWhiteSpace(PropBezeichnung))
            AddErrorMessage( "PropBezeichnung", "Ein neuer Versuch muss einen Namen haben."); // hier evtl auf Einzigartigkeit prüfen


         if (String.IsNullOrWhiteSpace(PropVersuchsleiter))
            AddErrorMessage( "PropVersuchsleiter", "Wählen Sie hier den (haupt-)verantwortlichen Wissenschaftler aus.");
         else
         {
            if (!PropUsernames.Contains(PropVersuchsleiter))
               AddErrorMessage( "PropVersuchsleiter", "Versuchsleiter muss ein Institutsmitarbeiter sein.");
         }

         if (String.IsNullOrWhiteSpace(PropStandorte))
            AddErrorMessage( "PropStandorte", "Wählen Sie einen Versuchsstandort aus.");


         if (String.IsNullOrWhiteSpace(PropVersuchsplan))
            AddErrorMessage( "PropVersuchsplan", "Geben Sie den Dateinamen des Versuchsplans ein. (Die Datei muss existieren und lesbar sein.)");
         else 
         {
            if(!File.Exists(PropVersuchsplan)) 
               AddErrorMessage( "PropVersuchsplan", "Die angegebene Datei existiert nicht.");
            else 
            {
             if(!new Uri(PropVersuchsplan).IsUnc)
               AddErrorMessage( "PropVersuchsplan", "Der Versuchsplan muss in einem Netzwerk-Share gespeichert sein.");
            }
         }

         if (PropStart<2000 || PropStart>2040 )
            AddErrorMessage( "PropStart", "Hier sind nur Jahreszahlen zwischen 2000 und 2040 zulässig");
         
         if (PropEnde<2000 || PropEnde>2040 )
            AddErrorMessage( "PropEnde", "Hier sind nur Jahreszahlen zwischen 2000 und 2040 zulässig");
         
         if(PropEnde <PropStart)
         { 
            AddErrorMessage( "PropStart", "Versuchsende kann nicht vor dem Versuchsbeginn sein");
            AddErrorMessage( "PropEnde", "Versuchsende kann nicht vor dem Versuchsbeginn sein");
         }

         if (String.IsNullOrWhiteSpace(PropVersuchsfrage))
            AddErrorMessage( "PropVersuchsfrage", "Beschreiben Sie hier in ein paar Worten, worum es geht.");

         if (String.IsNullOrWhiteSpace(PropKultur))
            AddErrorMessage( "PropKultur", "An welcher Pflanze findet der Versuch statt? (Mehrfachnennungen möglich)");

         OnPropertyChanged("PropBezeichnung");
         OnPropertyChanged("PropVersuchsleiter");
         OnPropertyChanged("PropStandorte");
         OnPropertyChanged("PropVersuchsplan");
         OnPropertyChanged("PropStart");
         OnPropertyChanged("PropEnde");
         OnPropertyChanged("PropVersuchsfrage");
         OnPropertyChanged("PropKultur");
      }

      #endregion

      #region  commands

      public ICommand AddCommand
      {
         get{return _addCommand;}
      }

      bool CanAdd
      {
         get { return true; ; }
      }

      public ICommand PropertiesCommand
      {
         get{return _propertiesCommand;}
      }

      public ICommand RemoveCommand
      {
         get{return _removeCommand;}
      }

      bool CanRemove
      {
         get // nur für Admin  und Versuchsleiter
         {
            return !CalcPropReadonly(); // 
         }   
      }

      public ICommand PropUpdateCommand
      {
         get{return _propUpdateCommand;}
      }

      bool CanPropUpdate
      {
         get { return _isRowChanged && CanApply && !_propReadonly; }   

      }

      public ICommand PropCloseCommand
      {
         get{return _propCloseCommand;}
      }

      public ICommand ShowFileCommand
      {
         get{return _showFileCommand;}
      }

      public ICommand SelectFileCommand
      {
         get{return _selectFileCommand;}
      }
      
      bool CanStandorteSelect
      {
         get { return !_propReadonly; }   
      }

      public ICommand StandorteSelectCommand
      {
         get{return _standorteSelectCommand;}
      }

      public ICommand StandorteUpdateCommand
      {
         get{return _standorteUpdateCommand;}
      }

      public ICommand StandorteCloseCommand
      {
         get{return _standorteCloseCommand;}
      }

      bool CalcPropReadonly()
      {
         if ((GlobData.CurrentUser == null)||(SelectedRow == null))
            return true;

         if (GlobData.IsAdminMode)
            return false;
      
         try 
         {
            DataRow row = SelectedRow.Row;
            int LeiterId = Convert.ToInt32(row["Leiter"]);
            return !( GlobData.CurrentUser.PersonId == LeiterId);
         }
         catch
         { return true; }// kann passieren, wenn Datensätze unvollständig sind
      }

      #endregion

      #region command-execution

      void SelectFile()
      {
         System.Windows.Forms.OpenFileDialog ofDlg = new System.Windows.Forms.OpenFileDialog();
         ofDlg.InitialDirectory =".";  
         ofDlg.Filter="(*.*)|*.*";
         ofDlg.FilterIndex = 1;
         ofDlg.Multiselect = false;

         if (ofDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
         {	
            PropVersuchsplan = ofDlg.FileName;
         }
      }

      void AddRow()
      {
         _propMode = Propertymode.Add;
         _isRowChanged = false;
         _propReadonly = false;

         _editedVersuch = new Versuch();
         _editedVersuch.Start = _editedVersuch.Ende = DateTime.Now.Year;
         _editedVersuch.LeiterTxt = (GlobData.CurrentUser != null) ? GlobData.CurrentUser.FullName : "";
         _editedVersuch.Standorte = "";
         _editedVersuch.IdsStandorte = new List<int>();

         Validate();

         _propVersuch = new PropertyVersuch(this);
         _propVersuch.ShowDialog();
      }


      void ShowProperties()
      {
         _propMode = Propertymode.Edit;
         _isRowChanged = false;
         _propReadonly = CalcPropReadonly();
         
         DataRow row = SelectedRow.Row;
         _editedVersuch = GetVersuchFromId(Convert.ToInt32(row["VersuchId"]));
         
         Validate();

         _propVersuch = new PropertyVersuch(this);
         _propVersuch.ShowDialog();

      }

      void PropUpdate()
      {
         if(!SqlAccess.IsValidVersuch(_editedVersuch))								
         {
            MsgWindow.Show("Versuchsdaten können nicht übernommen werden.", "Es existiert bereits ein Versuch mit dieser Bezeichnung", MessageLevel.Error);
            return;
         }

         _propVersuch.Close();
         if (!_isRowChanged)
            return;
         
         
         
         _editedVersuch.Leiter = _dictUsers.FirstOrDefault(x => x.Value == _editedVersuch.LeiterTxt).Key;//Klartextangaben in ForeignKey umrechnen

         bool success = (_propMode == Propertymode.Add) ?
            SqlAccess.InsertNewVersuch(_editedVersuch) :
            SqlAccess.UpdateVersuch(_editedVersuch);

         if (!success)
               MsgWindow.Show("Versuchsdaten können nicht übernommen werden.", SqlAccess.ErrorMsg, MessageLevel.Error);

         if(_propMode == Propertymode.Add)
            File.SetLastWriteTime(_editedVersuch.Plan, DateTime.Now); // Zeitstempel Versuchsplan neu setzen wegen Benachrichtigung

         ReadData();
      }

      void PropClose()
      {
         _propVersuch.Close();
      }

      void RemoveRow()
      {

         DataRow row = SelectedRow.Row;
         int versuchIndex = Convert.ToInt32(row["VersuchId"]);

         if (MsgYNWindow.Show("Versuch löschen", "Möchten Sie den Versuch wirklich löschen?", MessageLevel.Warning) == true)
         { 
            if (!SqlAccess.DeleteVersuch(versuchIndex))
            {
               MsgWindow.Show("Datensatz kann nicht gelöscht werden.", SqlAccess.ErrorMsg, MessageLevel.Error);
               return;
            }
            ReadData();
         }


      }

      void ShowFile()
      {
         DataRow row = (_parent == null) ? SelectedRow.Row : SelectedVersuch.Row;// Unterscheidung zw Basisdaten Versuch(== null) und Auswahlliste
         string fileName = (string)row["Versuchsplan"];
         string fileOwner = (string)row["Versuchsleiter"];
         //int rowId = (_parent== null)? SelectedRowIndex: SelectedVersuch;// Unterscheidung zw Basisdaten Versuch(== null) und Auswahlliste
         //string fileName = (string)_dtVersuche.Rows[rowId]["Versuchsplan"];

         OfficeStarter.OpenFile(fileName, fileOwner);
      


      }

      void StandorteSelect()
      {
         _propSelFlaechen = new PropertyAuswahlFlaechen(this);
         AssignStandortIds(_editedVersuch, false);
         _propSelFlaechen.ShowDialog();
      }

      void StandorteUpdate()
      {
         _propSelFlaechen.Close();
         string standorte= "";
         _editedVersuch.IdsStandorte.Clear();
         foreach (DataRow row in DataTableStandorte.Rows)
         {
            if(Convert.ToInt32(row["doAssign"]) != 0)
            {
               _editedVersuch.IdsStandorte.Add(Convert.ToInt32(row["FlaeId"]));

               if (!string.IsNullOrEmpty(standorte))
                  standorte += ", ";
               standorte += row["FlaeBez"];
            }

         }
         PropStandorte = standorte;
         _isRowChanged = true;
      
      }

      void StandorteClose()
      {

         _propSelFlaechen.Close();
      }

      #endregion

   }
}
