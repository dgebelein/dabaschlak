using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace dabaschlak
{
	class CmdMenu
	{
		public List<CmdBase> Items { get; }
		List<string> _fileUrls;
		List<CmdBase> _flexItems;

		public CmdMenu()
		{
			ReadFileUrls();
			ReadFlexNodes();

			CmdNode node;


			Items = new List<CmdBase>();
			node = new CmdNode { Text = "Versuche", TextColor = Brushes.DeepSkyBlue, Heigth = 30 };
			node.Content.Add(new CmdItem { Text = "Liste aller Versuche", TextColor = Brushes.White, Heigth = 25, Response = CmdResponse.ZeigeAlleVersuche });
			Items.Add(node);

			node = new CmdNode { Text = "Aktionen dokumentieren", TextColor = Brushes.DeepSkyBlue, Heigth = 30 };
			node.Content.Add(new CmdItem { Text = "an Versuchen", TextColor = Brushes.White, Heigth = 25, Response = CmdResponse.VersuchsArbeitenDokumentieren });
			node.Content.Add(new CmdItem { Text = "an Flächen (ohne Versuch)", TextColor = Brushes.White, Heigth = 25, Response = CmdResponse.FlaechenArbeitenDokumentieren});
			Items.Add(node);
			
			node = new CmdNode { Text = "Benachrichtigungssystem", TextColor = Brushes.DeepSkyBlue, Heigth = 30 };
			node.Content.Add(new CmdItem { Text = "Nachrichtenzuordnung", TextColor = Brushes.White, Heigth = 25, Response = CmdResponse.Nachrichtenzuordnung});
			node.Content.Add(new CmdItem { Text = "'Meine' Nachrichten", TextColor = Brushes.White, Heigth = 25, Response = CmdResponse.MeineNachrichten});
			node.Content.Add(new CmdItem { Text = "Neueste Versuchspläne", TextColor = Brushes.White, Heigth = 25, Response = CmdResponse.LetzteVersuchsplaene});
			Items.Add(node);
			

			node = new CmdNode { Text = "Berichte", TextColor = Brushes.DeepSkyBlue, Heigth = 30 };
			node.Content.Add(new CmdItem { Text = "Versuchsprotokoll erstellen", TextColor = Brushes.White, Heigth = 25, Response = CmdResponse.Versuchsprotokoll});
			node.Content.Add(new CmdItem { Text = "erweiterte Datenbankabfrage", TextColor = Brushes.White, Heigth = 25, Response = CmdResponse.Abfrage});
			Items.Add(node);


			node = new CmdNode { Text = "Planungshilfen", TextColor = Brushes.DeepSkyBlue, Heigth = 30 };
			foreach (CmdBase fn in _flexItems)
				node.Content.Add(fn);
			Items.Add(node);		
			
			node = new CmdNode { Text = "Stamm- und Basisdaten", TextColor = Brushes.DeepSkyBlue, Heigth = 30 };
			node.Content.Add(new CmdItem { Text = "Instituts-Mitarbeiter", TextColor = Brushes.White, Heigth = 25, Response = CmdResponse.ZeigeAllePersonen });
			node.Content.Add(new CmdItem { Text = "Versuchsflächen", TextColor = Brushes.White, Heigth = 25, Response = CmdResponse.ZeigeAlleFlaechen });
			Items.Add(node);

			node = new CmdNode { Text = "Hilfestellung", TextColor = Brushes.DeepSkyBlue, Heigth = 30 };
			node.Content.Add(new CmdItem { Text = "Dabaschlak Hilfestellung", TextColor = Brushes.White, Heigth = 25, Response = CmdResponse.Browser, Url = GetFileUrl(2)});

			Items.Add(node);




		}

		void ReadFlexNodes()
		{
			_flexItems = new List<CmdBase>();
			CmdNode currentNode = null;

			try
			{ 
				string fn = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "planungshilfen.cfg");
				var filestream = new System.IO.FileStream(fn,
															System.IO.FileMode.Open,
															System.IO.FileAccess.Read,
															System.IO.FileShare.ReadWrite);
				var file = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);
				string line;
				while ((line = file.ReadLine()) != null)
				{
					string s = line.Trim();
					if (string.IsNullOrWhiteSpace(line) || s.StartsWith(";"))// Zeilen mit ';' am Anfang sind Kommentare, Leerzeile überlesen
						continue;

					string[] words = s.Split(';');
					if (words.Length == 1)
					{
						if (currentNode != null)
						{ 
							_flexItems.Add(currentNode);
						}

						currentNode = new CmdNode{ Text = words[0].Trim(), TextColor = Brushes.LimeGreen, Heigth = 30 };

					}
					if (words.Length == 2)
					{
						CmdItem flexItem = new CmdItem { Text = words[0].Trim(), TextColor = Brushes.White, Heigth = 25, Response = CmdResponse.FlexItem, Url = words[1].Trim()};
						if (currentNode != null)
						{
							currentNode.Content.Add(flexItem);
						}
						else
							_flexItems.Add(flexItem);

					}
					
				}
				if (currentNode != null)	//letzten  Abschnitt nicht vergessen
				{ 
					_flexItems.Add(currentNode);
				}

			}
         catch { }
		}

		
		void ReadFileUrls()
		{
			_fileUrls = new List<string>();
			try
			{ 
				string fn = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dabaschlak.cfg");
				var filestream = new System.IO.FileStream(fn,
															System.IO.FileMode.Open,
															System.IO.FileAccess.Read,
															System.IO.FileShare.ReadWrite);
				var file = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);
				string line;
				while ((line = file.ReadLine()) != null)
				{
					string s = line.Trim();
					if (string.IsNullOrWhiteSpace(line) || s.StartsWith(";"))// Zeilen mit ';' sind kommentare, Leerzeile überlesen
						continue;	
					
					_fileUrls.Add(line.Trim());
				}
			}
         catch { }
			

		}

		public string GetFileUrl(int urlId)
		{
			if (urlId < _fileUrls.Count)
				return _fileUrls[urlId];
			else
				return string.Empty;

		}
	}
}
